using Buckshout.Hubs;
using Buckshout.Managers;
using Buckshout.Models;
using BuckshoutApp;
using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace Buckshout.Controllers
{

    public class RoomHub(ApplicationContext _appContext, IDistributedCache cache) : BaseHub(_appContext, cache)
    {
        public override async Task<Task> OnConnectedAsync()
        {

            var data = GetCommon();

            // Сессии, еще допиливаются
            var id = GetHashIdentifier();

            Console.WriteLine($"Пользователь {id} подключился к серверу");
            data.connectionId = id;

            if (await RestoreSession(data))
                return base.OnConnectedAsync();

            data.rooms = ApplicationContext.RoomManager.GetAllRoomsModel();
            await SendCaller(Event.CONNECTED, data);
            return base.OnConnectedAsync();
        }
        public async Task CreateRoom(string roomName)
        {
            var id = GetHashIdentifier();
            var room = ApplicationContext.RoomManager.CreateRoom(roomName, Clients.Group(roomName));

            Console.WriteLine($"Пользователь {id} создал комнату {roomName}");
            GetGameContext(roomName).EventManager.OnEvent(async (e, data) =>
            {
                await SendEvents(e, data, roomName);
            });

            await SendAll(Event.ROOM_CREATED, new
            {
                room = new RoomModel(room),
                initiator = id,
            });
        }
        public async Task JoinRoom(string playerName, string roomName)
        {
            var id = GetHashIdentifier();
            await CacheManager.UpdateCache(id, new UserConnection(playerName, roomName));
            var game = GetGameContext(roomName);
            bool needAdd = true;

            if (game.Status == GameStatus.IN_PROGRESS && game.PlayerManager.Get(id) != null)
            {
                game.PlayerManager.ReconnectPlayer(id);
                needAdd = false;
            }
            else
            {
                if (game.Status != GameStatus.PREPARING) return;
                if (await CheckLengthName(playerName)) return;
                Console.WriteLine($"Пользователь {id} подключился к {roomName}");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await SendCaller(Event.ROOM_JOINED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
                //settings
            });

            if (needAdd)
                ApplicationContext.RoomManager.AddToRoom(roomName, Clients.Caller, id, playerName, Context.ConnectionId);

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });
        }

        public async Task LeaveRoom(string roomName)
        {
            var id = GetHashIdentifier();
            await Groups.RemoveFromGroupAsync(id, roomName);
            var needRemoveRoom = ApplicationContext.RoomManager.RemoveFromRoom(roomName, id);
            await CacheManager.RemoveCache(id);

            if (needRemoveRoom) await SendAll(Event.ROOM_REMOVED, new { name = roomName });
            else
            {
                var room = ApplicationContext.RoomManager.GetRoom(roomName);
                if (room != null)
                    await SendAll(Event.ROOM_UPDATED, new
                    {
                        room = new RoomModel(room)
                    });
            }
            await SendCaller(Event.ROOM_LEFT);
        }

        public async Task SetTeam(string roomName, string team)
        {
            var id = GetHashIdentifier();
            var gameContext = GetGameContext(roomName);
            gameContext.PlayerManager.Get(id).Team = team;

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });
        }

        public async Task StartGame(string roomName)
        {
            var gameContext = GetGameContext(roomName);

            gameContext.StartGame();

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });

            gameContext.EventManager.Trigger(Event.GAME_STARTED, new EventData
            {
                Special = new Dictionary<string, object>
                {
                    { "GAME", new GameModel(gameContext) }
                }
            });

            gameContext.StartRound();
        }

        public void TakeAim(string roomName, string targetId)
        {
            var gameContext = GetGameContext(roomName);
            var targetPlayer = gameContext.PlayerManager.Get(targetId);
            var currentPlayer = gameContext.PlayerManager.Get(GetHashIdentifier());

            gameContext.EventManager.Trigger(Event.RIFLE_AIMED, new EventData
            {
                Initiator = currentPlayer,
                Target = targetPlayer,
            });
        }


        public void Shoot(string roomName, string targetId)
        {
            var gameContext = GetGameContext(roomName);
            var targetPlayer = gameContext.PlayerManager.Get(targetId);

            gameContext.Rifle.Shoot(targetPlayer);
        }

        public void Use(string roomName, string itemId, string targetId, string? targetItemId = null)
        {
            var gameContext = GetGameContext(roomName);

            gameContext.PlayerManager.Get(GetHashIdentifier()).UseItem(itemId, targetId, targetItemId);
        }

        public async Task<bool> RestoreSession(dynamic data)
        {
            // Смертный, не бойся легаси, а примкни к нему, так что запомни:
            // data.connectionId - это Идентификатор пользователя по ип + юзер агент
            // Context.ConnectionID - это от библиотеки SignalR
            var cache = await CacheManager.GetCache(data.connectionId);
            if (cache?.playerName == null)
            {
                await CacheManager.SetCache(data.connectionId, new UserConnection());
                Console.WriteLine($"Пользователь {data.connectionId} создал кэш");
            }
            else
            {
                if (cache.roomName != null && RoomManager.GetRoom(cache.roomName) != null)
                {
                    Client client = RoomManager.UpdateClient(data.connectionId, Clients.Caller, Context.ConnectionId);
                    client.TimerOnDeletePlayer?.Dispose();
                    data.name = cache.playerName;
                    data.room = cache.roomName;
                    await SendCaller(Event.RECONNECTED, data);

                    Console.WriteLine($"Пользователь {data.connectionId} делает переподключение к {data.room}");
                    // await JoinRoom(cache.playerName, cache.roomName);
                    return true;
                }
            }
            return false;
        }

        private void DeletePlayerFromGame(string id, Room room, Client client)
        {
            var game = GetGameContext(room.Name);
            game.PlayerManager.DisconnectPlayer(id);

            client.TimerOnDeletePlayer = TimerExtension.SetTimeout(async () =>
            {
                var player = room.GameContext.PlayerManager.Players.FirstOrDefault(it => it.Id == id);
                if (player != null && player.Status != BuckshoutApp.Manager.PlayerStatus.CONNECTED)
                {
                    await CacheManager.RemoveCache(id);
                    game.PlayerManager.DeletePlayer(id);
                }
            }, room.GameContext.Settings.RECCONECTION_TIME);
            /*room.GameContext.EventManager.Once(Event.PLAYER_RECONNECTED, (e) =>
            {
                if (e.Initiator!.Id == id)
                    client.TimerOnDeletePlayer.Dispose();
            });*/
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var id = GetHashIdentifier();
            Client client = RoomManager.GetClient(id);
            var room = ApplicationContext.RoomManager.GetClientRoom(id);

            await SendCaller(Event.DISCONNECTED);

            if (room != null)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Name);

            Console.WriteLine($"Пользователь {id} отключился от {room?.Name ?? "Без комнаты"}");

            if (client is not null)
            {
                if (room is not null)
                {
                    DeletePlayerFromGame(id, room, client);
                }
                // Надо подумать в какой момент его удалять
                /*else
                {
                    client.TimerOnDeleteCache = TimerExtension.SetTimeout(async () =>
                    {
                        Console.WriteLine($"Пользователь {id} УДАЛЕН ИЗ КЭША");
                        await CacheManager.RemoveCache(id);
                    }, 1000 * 60 * 1);
                }*/
            }


            await base.OnDisconnectedAsync(exception);
        }
    }
}
