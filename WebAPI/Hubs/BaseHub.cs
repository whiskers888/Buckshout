using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Dynamic;
using System.Text.Json;

namespace Buckshout.Hubs
{
    
    public interface IClient
    {
        public Task ReceiveMessage (string userName, object data);
        public Task ExceptionMessage (string data);

        public Task RoomCreated (object data);
        public Task GameStarted(string userName, object data);
        public Task RoundStarted(object data);

        public Task Damage(object data);
        public Task Health(object data);

        public Task Shoot(object data);
        public Task MovePass(object data);
    }
    public class BaseHub :Hub
    {


        private readonly IDistributedCache _cache;
        public ApplicationContext ApplicationContext { get; set; }
        public RoomManager RoomManager => ApplicationContext.RoomManager;

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
            return connection;
        }

        internal async void RemoveCache(string roomName)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        internal dynamic GetCommon() => new ExpandoObject();
        internal async Task Send(string roomName, MessageType methodName, object? data = null)
        {
            await Clients.Group(roomName).SendAsync(methodName.ToStr(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }

        internal async Task SendCaller(MessageType methodName, object? data = null)
        {
            await Clients.Caller.SendAsync(methodName.ToStr(), new JsonResult(new
            {
                data,
                datetime = DateTime.Now.ToString()
            }));
        }

        internal async Task SendFromSystem(string roomName, object message )
        {
            await Clients.Group(roomName).SendAsync(MessageType.ReceiveMessage.ToStr(), new JsonResult(new
            {
                data = new
                {
                    sender ="WALL-E",
                    message = message
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
