
using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;

namespace Buckshout.Controllers
{

    public class RoomHub : BaseHub
    {

        private readonly IDistributedCache _cache;

        public RoomHub(IDistributedCache cache, ApplicationContext _appContext) : base(_appContext)
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
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Groups(roomName).RoomCreated($"Комната {roomName} создана ");
            }

            RoomManager.AddToRoom(roomName, new Player(connection.userName, Context.ConnectionId));


            var stringConnection = JsonSerializer.Serialize(new UserConnection(connection.userName, roomName));

            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            await Send(roomName ?? "", $"{connection.userName} присоединился к комнате");
        }

        public async Task StartGame()
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            var gameContext = GetGameContext(connection.roomName);

            gameContext.StartGame(Mode.Default);
            await Clients.Groups(connection.roomName).GameStarted("WALL-E","Игра началась");

            gameContext.StartRound();
            await Clients.Groups(connection.roomName).RoundStarted("Старт раунда");
        }

        public async Task Shoot(int targetUser)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            var gameContext = GetGameContext(connection.roomName);
            var targetPlayer = gameContext.PlayerManager.Get(Context.ConnectionId);

            gameContext.RifleManager.Shoot(targetPlayer);
            await Clients.Groups(connection.roomName).Shoot("shootw");
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
            Console.WriteLine($"{connection.roomName}:{connection.userName} exited");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
