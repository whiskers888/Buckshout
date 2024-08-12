using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Buckshout.Hubs
{
    
    public class BaseHub :Hub
    {
        private readonly IDistributedCache _cache;
        internal ApplicationContext ApplicationContext { get; set; }
        internal RoomManager RoomManager => ApplicationContext.RoomManager;

        public BaseHub(ApplicationContext applicationContext, IDistributedCache cache)
        {
            ApplicationContext = applicationContext;
            _cache = cache;
        }
        internal async void SetCache(string userName, string roomName)
        {
            var stringConnection = JsonSerializer.Serialize(new UserConnection(userName, roomName));
            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
        }

        internal async Task<UserConnection> GetCache()
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            UserConnection connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
            if (connection is not null)
                return connection;
            else
                throw new Exception("Невозможно достать из кэша подключение пользователя");
        }

        internal async void RemoveCache(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await _cache.RemoveAsync(Context.ConnectionId);
        }

        internal dynamic GetCommon() => new ExpandoObject();
        internal async Task Send(string roomName, Event eventName, object? data = null)
        {
            await Clients.Group(roomName).SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        internal async Task SendPlayer(string connectionID, Event eventName, object? data = null)
        {
            await Clients.Client(connectionID).SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }

        internal async Task SendCaller(Event methodName, object? data = null)
        {
            await Clients.Caller.SendAsync(methodName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }

        internal async Task SendFromSystem(string roomName, object message )
        {
            await Clients.Group(roomName).SendAsync(Event.MESSAGE_RECEIVED.ToString(), new JsonResult(new
            {
                data = new
                {
                    sender ="WALL-E",
                    message
                },
                datetime = DateTime.Now.ToString()
            }));
        }

        internal GameContext GetGameContext(string roomName)
        {
            return RoomManager.GetRoom(roomName).GameContext;
        }
    }
}
