using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Dynamic;
using System.Text.Json;

namespace Buckshout.Hubs
{

    public class BaseHub(ApplicationContext applicationContext, IDistributedCache cache) : Hub
    {
        private readonly IDistributedCache _cache = cache;
        internal ApplicationContext ApplicationContext { get; set; } = applicationContext;
        internal RoomManager RoomManager => ApplicationContext.RoomManager;

        internal async void SetCache(string sessionId, string roomName)
        {
            var stringConnection = JsonSerializer.Serialize(new UserConnection(sessionId, roomName));
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

        /*        internal async Task<UserConnection> UpdateCache(UserConnection conn)
                {
                    var cache = GetCache();

                }*/

        internal async void RemoveCache(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await _cache.RemoveAsync(Context.ConnectionId);
        }

        internal dynamic GetCommon() => new ExpandoObject();
        internal async Task Send(string roomName, Event eventName, object? data = null)
        {
            await ApplicationContext.RoomManager.GetRoom(roomName).Group.SendAsync(eventName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        internal async Task Send(string roomName, string eventName, object? data = null)
        {

            await ApplicationContext.RoomManager.GetRoom(roomName).Group.SendAsync(eventName.ToString(), new JsonResult(new
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
        internal async Task SendCaller(string methodName, object? data = null)
        {
            await Clients.Caller.SendAsync(methodName.ToString(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }
        internal async Task SendAll(string eventName, object? data = null)
        {
            await Clients.All.SendAsync(eventName, new JsonResult(new
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
