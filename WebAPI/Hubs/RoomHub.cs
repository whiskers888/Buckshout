
using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;

namespace Buckshout.Controllers
{

    public class RoomHub : BaseHub
    {
        public RoomHub( ApplicationContext _appContext, IDistributedCache cache) : base(_appContext, cache)
        {
        }

        public async Task JoinRoom(UserConnection connection)
        {
            string? roomName = connection.roomName;
            if (connection.userName == null)
            {
                await SendCaller(MessageType.ExceptionMessage, "Заполните имя");
                return;
            }

            if (!string.IsNullOrEmpty(connection.roomName))
                // проверит на разные имена
                await Groups.AddToGroupAsync(Context.ConnectionId, connection.roomName);
            else
            {
                roomName = connection.userName;
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }

            await Send(roomName, MessageType.RoomCreated, new
            {
                roomName,
            });
            await SendFromSystem(roomName,  "Комната была создана");

            RoomManager.AddToRoom(roomName, new Player(connection.userName, Context.ConnectionId));

            SetCache(connection.userName, roomName);

            await SendFromSystem(roomName, $"{connection.userName} присоединился к комнате");
        }

        public async Task StartGame()
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);

            gameContext.StartGame(Mode.Default);

            await Send(connection.roomName, MessageType.GameStarted);
            await SendFromSystem(connection.roomName, "Игра началась");

            gameContext.StartRound();

            var data = GetCommon();
            data.patrons = gameContext.RifleManager.CreatePatrons();
            await Send(connection.roomName, MessageType.RoundStarted, data);
            await SendFromSystem(connection.roomName, "Начинаем игру... \n Раунд 1 начался");
        }

        public async Task Shoot(int targetUser)
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);
            var targetPlayer = gameContext.PlayerManager.Get(Context.ConnectionId);

            var shoot = gameContext.RifleManager.Shoot(targetPlayer);
            await Send(connection.roomName, MessageType.Shoot, new
            {
                shoot
            });
        }

        public async Task Use(int objectId)
        {

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await GetCache();

            if (connection is not null)
            {
                RemoveCache(connection.roomName);

                await SendFromSystem(connection.roomName, $"{connection.userName} покинул чат")
            };
            Console.WriteLine($"{connection.roomName}:{connection.userName} exited");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
