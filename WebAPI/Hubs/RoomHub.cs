
using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Buckshout.Controllers
{

    public class RoomHub : BaseHub
    {
        private readonly IDistributedCache _cache;
        private readonly IDistributedCache _game;

        public RoomHub(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task JoinRoom(UserConnection connection)
        {
            string? roomName = connection.roomName;
            if (connection.userName == null)
            {
                await Clients.Caller.ExceptionMessage("Заполните имя");
                return;
            }

            if (!string.IsNullOrEmpty(connection.roomName))
                // проверит на разные имена
                await Groups.AddToGroupAsync(Context.ConnectionId, connection.roomName);
            else
            {
                roomName = connection.userName;
                RoomManager.AddToRoom(connection.userName, Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Groups(roomName).RoomCreated($"Комната {connection.roomName} создана ");
            }

            var stringConnection = JsonSerializer.Serialize(connection);

            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            await Send(roomName ?? "", $"{connection.userName} присоединился к комнате");
        }

        public async Task StartGame()
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
            // new Game()
            await Clients.Groups(connection.roomName).RoomCreated($"Игра началась");
        }

        public async Task Shoot(int targetUser, int userMake)
        {

        }

        public async Task Use(int objectId)
        {

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection is not null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.roomName??"");

                await Clients
                    .Group(connection.roomName??"")
                    .ReceiveMessage("WALL-E", $"{connection.userName} покинул чат");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
