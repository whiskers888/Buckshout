using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace Buckshout.Hubs
{

    public class BaseHub : Hub
    {
        public BaseHub(ApplicationContext applicationContext, IDistributedCache cache)
        {
            ApplicationContext = applicationContext;
        }
        internal ApplicationContext ApplicationContext { get; set; }
        internal RoomManager RoomManager => ApplicationContext.RoomManager;

        internal CacheManager CacheManager => ApplicationContext.CacheManager;

        internal string GetHashIdentifier() => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(GetUserIdentifier())));
        private string GetUserIdentifier()
        {
            var ipAddress = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            var userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return $"{ipAddress}_{userAgent}";
        }
        internal dynamic GetCommon() => new ExpandoObject();
        internal async Task<bool> CheckLengthName(string playerName)
        {
            var check = playerName.Length > 20;
            if (check)
                await SendCaller(Event.SECRET_MESSAGE, "Слишком длинный никнейм!");
            return check;
        }
        internal async void SendEvents(Event e, EventData data, string roomName)
        {
            /*вылетает баг*/
            if (e == Event.SECRET_MESSAGE || e == Event.RIFLE_CHECKED)
                await SendPlayer(data.Initiator!.Id, e, new DataModel(data));
            else if (e == Event.ITEM_RECEIVED)
            {
                await SendPlayer(data.Target.Id, e, new DataModel(data));
                await SendOther(data.Target.Id, e, new DataModel(data, true));
            }
            else if (e == Event.MODIFIER_APPLIED)
            {
                var client = GetGameContext(roomName).QueueManager.Current.Id;
                await SendPlayer(client, e, new DataModel(data));
                await SendOther(client, e, new DataModel(data, true));
            }
            else
                await Send(roomName, e, new DataModel(data));
        }
        internal async Task Send(string roomName, Event eventName, object? data = null)
        {
            await ApplicationContext.RoomManager.GetRoom(roomName).Group.SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        internal async Task SendOther(string connectionID, Event eventName, object? data = null)
        {

            var group = ApplicationContext.RoomManager.GetClientRoom(connectionID);
            foreach (var client in group.Clients.Where(it => it.Key != connectionID))
            {
                await client.Value.SendAsync(eventName.ToString(), new JsonResult(new
                {
                    data,
                    datetime = DateTime.Now.ToString()
                }));
            }

        }
        internal async Task SendPlayer(string connectionID, Event eventName, object? data = null)
        {

            await RoomManager.GetClient(connectionID)!.SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }

        // HACK: Методы ниже реализованы через интерфейс встроенного хаба в синглтон, на данный момент они работают
        // так как вызываются не из под контекста игрового модуля.
        /*[Obsolete(" Не стоит вызывать этот метод в контексте игрового процесса." +
                    "Если вызывать из RoomHub - проблем не будет")]*/
        internal async Task SendCaller(Event methodName, object? data = null)
        {
            await Clients.Caller.SendAsync(methodName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        /*[Obsolete(" Не стоит вызывать этот метод в контексте игрового процесса, так как ивенты отправляемые через него не имеют контекст хаба." +
            "Если вызывать из RoomHub - проблем не будет")]*/
        internal async Task SendAll(Event eventName, object? data = null)
        {
            await Clients.All.SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        internal GameContext GetGameContext(string roomName)
        {
            return ApplicationContext.RoomManager.GetRoom(roomName).GameContext;
        }
    }
}
