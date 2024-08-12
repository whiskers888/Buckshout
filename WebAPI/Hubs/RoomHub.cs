using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace Buckshout.Controllers
{

    public class RoomHub : BaseHub
    {
        public RoomHub( ApplicationContext _appContext, IDistributedCache cache) : base(_appContext, cache) { }

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

            await Send(roomName, Event.GAME_CREATED, new
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

            gameContext.EventManager.OnEvent(async (e, data) =>
            {
                if (e == Event.MESSAGE_INITIATOR_RECEIVED)
                    SendPlayer(data.initiator.UUID, e, data);
                else
                    await Send(connection.roomName, e, data);
            });
            
            gameContext.StartGame(Mode.Default);
            var dataGameStarted = GetCommon();
            dataGameStarted.players = gameContext.PlayerManager.Players.Select(it => new PlayerModel(it)).ToArray();
            dataGameStarted.currentPlayer = Context.ConnectionId;
            await Send(connection.roomName, Event.GAME_STARTED, dataGameStarted);
            await SendFromSystem(connection.roomName, "Игра началась");

            gameContext.StartRound();

            var data = GetCommon();
            data.patrons = gameContext.RifleManager.CreatePatrons();
            await Send(connection.roomName, Event.ROUND_STARTED, data);
            await SendFromSystem(connection.roomName, "Начинаем игру... \n Раунд 1 начался");
        }

        public async Task Shoot(string targetId)
        {
            var connection = await GetCache();

            var gameContext = GetGameContext(connection.roomName);
            var targetPlayer = gameContext.PlayerManager.Get(targetId);
            var currentPlayer = gameContext.PlayerManager.Get(Context.ConnectionId);

            var shoot = gameContext.RifleManager.Shoot(targetPlayer);

            await SendFromSystem(connection.roomName, $"{currentPlayer.Name} стреляет в {targetPlayer}");
            await Send(connection.roomName, Event.RIFLE_SHOT, new
            {
                shoot
            });
            await SendFromSystem(connection.roomName, shoot == true ? "Патрон был заряжен." : "Патрон не был заряжен.");
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
                RemoveCache(connection.roomName);
                RoomManager.RemoveFromRoom(connection.roomName, Context.ConnectionId);
                await Send(connection.roomName, Event.PLAYER_DISCONNECTED, new { exitedPlayer = connection.userName });
                await SendFromSystem(connection.roomName, $"{connection.userName} покинул чат");
                Console.WriteLine($"{connection.roomName}:{connection.userName} exited");
            };
            await base.OnDisconnectedAsync(exception);
        }
    }
}
