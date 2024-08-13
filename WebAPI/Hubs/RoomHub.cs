using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace Buckshout.Controllers
{

    public class RoomHub(ApplicationContext _appContext, IDistributedCache cache) : BaseHub(_appContext, cache)
    {
        public async Task JoinRoom(UserConnection connection)
        {
            string? roomName = connection.roomName;
            if (connection.userName == null)
            {
                await SendCaller(Event.MESSAGE_RECEIVED, "Заполните имя");
                return;
            }

            if (!string.IsNullOrEmpty(connection.roomName))
                // проверит на разные имена UPD: Если из-за этого багов не будет то впринципе и не требуется
                await Groups.AddToGroupAsync(Context.ConnectionId, connection.roomName);
            else
            {
                roomName = connection.userName;
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }

            //Разбить на создание и добавление юзеров
            ApplicationContext.RoomManager.AddToRoom(roomName, new Player(connection.userName, Context.ConnectionId), Clients.Group(roomName));

            SetCache(connection.userName, roomName);

            await Send(roomName, Event.GAME_CREATED, new
            {
                roomName,
            });

            await SendFromSystem(roomName, "Комната была создана");

            await SendFromSystem(roomName, $"{connection.userName} присоединился к комнате");
        }
        public async Task StartGame()
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);

            gameContext.EventManager.OnEvent(async (e, data) =>
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
            var connection = await GetCache();

            if (connection is not null)
            {
                /*RemoveCache(connection.roomName);
                await Send(connection.roomName, Event.PLAYER_DISCONNECTED, new { exitedPlayer = connection.userName });
                ApplicationContext.RoomManager.RemoveFromRoom(connection.roomName, Context.ConnectionId);
                await SendFromSystem(connection.roomName, $"{connection.userName} покинул чат");*/
                Console.WriteLine($"{connection.roomName}:{connection.userName} exited");
            };
            await base.OnDisconnectedAsync(exception);
        }
    }
}
