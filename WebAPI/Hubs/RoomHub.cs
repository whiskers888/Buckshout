using Buckshout.Hubs;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;

namespace Buckshout.Controllers
{

    public class RoomHub(ApplicationContext _appContext, IDistributedCache cache) : BaseHub(_appContext, cache)
    {
        public override async Task<Task> OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var sessionId = httpContext.Request.Cookies["SESSION_ID"];
            var data = GetCommon();
            data.rooms = ApplicationContext.RoomManager.GetAllRooms();
            if (sessionId is not null)
            {
                if (!ApplicationContext.Sessions.ContainsKey(sessionId))
                {
                    SetCache(sessionId, null);
                    ApplicationContext.Sessions.Add(sessionId, null);
                    await SendCaller("CONNECTED", data);
                }
                else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

                    // восстановить контекст игрока
                    await SendCaller("RECONNECTED", data);
                }
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpContext.Response.Cookies.Append("SESSION_ID", sessionId);
                await SendCaller("CONNECTED", data);
            }
            return base.OnConnectedAsync();
        }
        public async Task CreatedRoom(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.roomName);
            ApplicationContext.RoomManager.CreateRoom(connection.roomName, Clients.Group(connection.roomName));

            await SendAll("ROOM_CREATED", new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(connection.roomName).Players),
                connectionId = Context.ConnectionId,
                count_players = ApplicationContext.RoomManager.GetRoom(connection.roomName).Players.Count,
            });
        }
        public async Task JoinRoom(UserConnection connection)
        {
            string? roomName = connection.roomName;


            await Send(roomName, "JOINED_ROOM", new
            {
                roomName,
            });
        }
        public async Task StartGame()
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);

            GetGameContext(connection.roomName).EventManager.OnEvent(async (e, data) =>
            {
                if (e == Event.MESSAGE_INITIATOR_RECEIVED)
                    await SendPlayer(data.initiator!.UUID, e, new DataModel(data));
                else
                    await Send(connection.roomName, e, new DataModel(data));
            });

            gameContext.StartGame(Mode.Default);
            var dataGameStarted = GetCommon();
            dataGameStarted.players = gameContext.PlayerManager.Players.Select(it => new PlayerModel(it)).ToArray();
            gameContext.EventManager.Trigger(Event.GAME_STARTED, dataGameStarted);

            gameContext.StartRound();
        }

        public async Task Shoot(string targetId)
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);
            var targetPlayer = gameContext.PlayerManager.Get(targetId);
            /*var currentPlayer = gameContext.PlayerManager.Get(Context.ConnectionId);*/

            gameContext.Rifle.Shoot(targetPlayer);

            /*await SendFromSystem(connection.roomName, $"{currentPlayer.Name} стреляет в {targetPlayer}");
            await Send(connection.roomName, Event.RIFLE_SHOT, new
            {
                shoot
            });
            await SendFromSystem(connection.roomName, shoot == true ? "Патрон был заряжен." : "Патрон не был заряжен.");*/
        }

        public async Task Use(string itemId, string targetId)
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);

            gameContext.PlayerManager.Get(Context.ConnectionId).UseItem(itemId, targetId);

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            /*var connection = await GetCache();
            var httpContext = Context.GetHttpContext();
            var sessionId = httpContext.Request.Cookies["SESSION_ID"];
            Send()
            foreach (var session in ApplicationContext.Sessions)
            {
                if (session.Value == sessionId)
                {
                    TimerExtension.SetTimeout(async () =>
                    {
                        ApplicationContext.Sessions.Remove(sessionId);
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, session.Key);
                    }, 300 * 1000);
                    break;
                }
            }*/

            await base.OnDisconnectedAsync(exception);

            /*if (connection is not null)
            {
                *//*RemoveCache(connection.roomName);
                ApplicationContext.RoomManager.RemoveFromRoom(connection.roomName, Context.ConnectionId);
                await SendFromSystem(connection.roomName, $"{connection.userName} покинул чат");*//*
                Console.WriteLine($"{connection.roomName}:{connection.connectionId} exited");
            };
            await base.OnDisconnectedAsync(exception);*/
        }
    }
}
