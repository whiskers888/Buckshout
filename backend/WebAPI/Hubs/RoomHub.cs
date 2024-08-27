using Buckshout.Hubs;
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
            var connectionId = GetHashIdentifier();

            Console.WriteLine($"Пользователь {connectionId} подключился к серверу");
            data.connectionId = connectionId;
            var cache = await CacheManager.GetCache(connectionId);

            if (await RestoreSession(data))
                return base.OnConnectedAsync();

            data.rooms = ApplicationContext.RoomManager.GetAllRoomsModel();
            await SendCaller(Event.CONNECTED, data);
            return base.OnConnectedAsync();
        }
        public async Task CreateRoom(string roomName)
        {

            var connectionId = GetHashIdentifier();
            var room = ApplicationContext.RoomManager.CreateRoom(roomName, Clients.Group(roomName));

            Console.WriteLine($"Пользователь {connectionId} создал комнату {roomName}");
            GetGameContext(roomName).EventManager.OnEvent(async (e, data) =>
            {
                SendEvents(e, data, roomName);
            });

            await SendAll(Event.ROOM_CREATED, new
            {
                room = new RoomModel(room),
                initiator = connectionId,
            });
        }
        public async Task JoinRoom(string playerName, string roomName)
        {
            var connectionId = GetHashIdentifier();
            var cache = await CacheManager.GetCache(connectionId);
            await CacheManager.UpdateCache(connectionId, new UserConnection(playerName, roomName));

            if (GetGameContext(roomName).Status != GameStatus.PREPARING) return;
            if (await CheckLengthName(playerName)) return;

            await Groups.AddToGroupAsync(connectionId, roomName);

            await SendCaller(Event.ROOM_JOINED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
                //settings
            });

            Console.WriteLine($"Пользователь {connectionId} подключился к {roomName}");
            ApplicationContext.RoomManager.AddToRoom(roomName, Clients.Caller, connectionId, playerName);

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });
        }

        public async Task LeaveRoom(string roomName)
        {
            var connectionId = GetHashIdentifier();
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
            var needRemoveRoom = ApplicationContext.RoomManager.RemoveFromRoom(roomName, connectionId);

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
            var connectionId = GetHashIdentifier();
            var gameContext = GetGameContext(roomName);
            gameContext.PlayerManager.Get(connectionId).Team = team;

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });
        }

        public async Task StartGame(string roomName)
        {
            var gameContext = GetGameContext(roomName);

            gameContext.StartGame(Mode.Default);

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
                    data.room = cache.roomName;
                    await SendCaller(Event.RECONNECTED, data);

                    Console.WriteLine($"Пользователь {data.connectionId} делает переподключение к {data.room}");
                    await JoinRoom(cache.playerName, cache.roomName);
                    return true;
                }
            }
            return false;
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = GetHashIdentifier();
            await SendCaller(Event.DISCONNECTED);


            var room = ApplicationContext.RoomManager.GetClientRoom(GetHashIdentifier());

            Console.WriteLine($"Пользователь {connectionId} отключился от  {room?.Name ?? "Без комнаты"}");
            IDisposable timer = null;
            if (room != null)
            {
                ApplicationContext.RoomManager.RemoveFromRoom(room.Name, connectionId);
                timer = TimerExtension.SetTimeout(async () =>
                {
                    var player = room.GameContext.PlayerManager.Players.FirstOrDefault(it => it.Id == connectionId);
                    if (player != null && player.Status != BuckshoutApp.Manager.PlayerStatus.CONNECTED)
                    {
                        await CacheManager.RemoveCache(connectionId);
                        room.GameContext.EventManager.Trigger(Event.LEAVE, new EventData()
                        {
                            Initiator = player,
                            Target = player,
                        });
                    }
                }, room.GameContext.Settings.RECCONECTION_TIME);
                room.GameContext.EventManager.Once(Event.PLAYER_CONNECTED, (e) =>
                {
                    if (e.Initiator!.Id == connectionId)
                        timer.Dispose();
                });
            }
            else
            {
                // хуйня полная, если чел допустим зашел и вышел, то его кэш через минуту автоматом отчистится
                timer = TimerExtension.SetTimeout(async () =>
                {
                    await CacheManager.RemoveCache(connectionId);
                }, 60000);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
