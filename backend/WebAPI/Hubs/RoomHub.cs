using Buckshout.Hubs;
using Buckshout.Models;
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

            /* var userIdentifier = GetUserIdentifier();
             var cache = await CacheManager.GetCache(userIdentifier);
             if (cache == null)
                 await CacheManager.SetCache(userIdentifier, new UserConnection(Context.ConnectionId));
             else
             {
                 data.room = cache.roomName;
                 await SendCaller(Event.RECONNECTED, data);
                 if (cache.roomName != null)
                     await JoinRoom(cache.playerName, cache.roomName);
                 return base.OnConnectedAsync();
             }*/

            data.rooms = ApplicationContext.RoomManager.GetAllRoomsModel();
            await SendCaller(Event.CONNECTED, data);
            return base.OnConnectedAsync();
        }

        public async Task CreateRoom(string roomName)
        {
            var room = ApplicationContext.RoomManager.CreateRoom(roomName, Clients.Group(roomName));

            GetGameContext(roomName).EventManager.OnEvent(async (e, data) =>
            {
                /*вылетает баг*/
                if (e == Event.SECRET_MESSAGE || e == Event.RIFLE_CHECKED)
                    await SendPlayer(data.Initiator!.Id, e, new DataModel(data));
                else if (e == Event.ITEM_RECEIVED)
                {
                    await SendPlayer(data.Target.Id, e, new DataModel(data));
                    await SendOther(data.Target.Id, e, new DataModel(data, true));
                }
                else if (e == Event.MODIFIER_APPLIED)
                {
                    var client = GetGameContext(roomName).QueueManager.Current.Id;
                    await SendPlayer(client, e, new DataModel(data));
                    await SendOther(client, e, new DataModel(data, true));
                }
                else
                    await Send(roomName, e, new DataModel(data));
            });

            await SendAll(Event.ROOM_CREATED, new
            {
                room = new RoomModel(room),
                initiator = Context.ConnectionId,
            });
        }
        public async Task JoinRoom(string playerName, string roomName)
        {

            var userIdentifier = GetUserIdentifier();
            var cache = await CacheManager.GetCache(userIdentifier);
            await CacheManager.UpdateCache(GetUserIdentifier(), new UserConnection(Context.ConnectionId, cache.playerName, cache.roomName));

            if (GetGameContext(roomName).Status != GameStatus.PREPARING) return;
            if (playerName.Length > 20)
            {
                await SendCaller(Event.SECRET_MESSAGE, "Слишком длинный никнейм!");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            await SendCaller(Event.ROOM_JOINED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
                //settings
            });

            ApplicationContext.RoomManager.AddToRoom(roomName, Clients.Caller, Context.ConnectionId, playerName);

            await SendAll(Event.ROOM_UPDATED, new
            {
                room = new RoomModel(ApplicationContext.RoomManager.GetRoom(roomName))
            });
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            var needRemoveRoom = ApplicationContext.RoomManager.RemoveFromRoom(roomName, Context.ConnectionId);

            if (needRemoveRoom)
            {
                await SendAll(Event.ROOM_REMOVED, new
                {
                    name = roomName
                });
            }
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
            var gameContext = GetGameContext(roomName);
            gameContext.PlayerManager.Get(Context.ConnectionId).Team = team;

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
            var currentPlayer = gameContext.PlayerManager.Get(Context.ConnectionId);

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

            gameContext.PlayerManager.Get(Context.ConnectionId).UseItem(itemId, targetId, targetItemId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await SendCaller(Event.DISCONNECTED);
            /*var room = ApplicationContext.RoomManager.GetClientRoom(Context.ConnectionId);
            IDisposable timer = null;
            if (room != null)
            {
                ApplicationContext.RoomManager.RemoveFromRoom(room.Name, Context.ConnectionId);
                timer = TimerExtension.SetTimeout(async () =>
                {
                    var player = room.GameContext.PlayerManager.Players.FirstOrDefault(it => it.Id == Context.ConnectionId);
                    if (player != null && player.Status != BuckshoutApp.Manager.PlayerStatus.CONNECTED)
                    {
                        await CacheManager.RemoveCache(GetUserIdentifier());
                        room.GameContext.EventManager.Trigger(Event.LEAVE, new EventData()
                        {
                            Initiator = player,
                            Target = player,
                        });
                    }
                }, room.GameContext.Settings.RECCONECTION_TIME);
                room.GameContext.EventManager.Once(Event.PLAYER_CONNECTED, (e) =>
                {
                    if (e.Initiator!.Id == Context.ConnectionId)
                        timer.Dispose();
                });
            }*/
            await base.OnDisconnectedAsync(exception);
        }
    }
}
