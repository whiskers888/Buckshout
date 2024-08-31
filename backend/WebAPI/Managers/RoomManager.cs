using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;

namespace Buckshout.Managers
{
    public class Client(ISingleClientProxy clientProxy, string connectionId)
    {
        public string ConnectionId { get; set; } = connectionId;
        public ISingleClientProxy ClientProxy { get; set; } = clientProxy;
        public IDisposable TimerOnDeletePlayer { get; set; }
        public IDisposable TimerOnDeleteCache { get; set; }
    }
    public class Room(string roomName, GameContext gameContext, IClientProxy group)
    {
        public string Name { get; set; } = roomName;
        public IClientProxy Group { get; set; } = group;
        public Dictionary<string, Client> Clients { get; set; } = [];
        public GameContext GameContext { get; set; } = gameContext;
    }


    public class RoomManager
    {
        private readonly Dictionary<string, Room> rooms = new();
        private ApplicationContext applicationContext;
        public RoomManager(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
        public Room CreateRoom(string roomName, IClientProxy group)
        {

            var room = new Room(roomName, new GameContext(), group);
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = room;
            }
            return room;
        }

        public void AddToRoom(string roomName, ISingleClientProxy client, string id, string name, string connectionId)
        {
            if (!rooms.ContainsKey(roomName))
            {
                return;
            }
            Room room = GetRoom(roomName);
            if (room.GameContext.PlayerManager.Get(id) == null && !room.Clients.ContainsKey(id))
            {
                room.GameContext.PlayerManager.AddPlayer(id, name, connectionId);
                room.Clients.Add(id, new Client(client, connectionId));
            }
        }

        public bool RemoveFromRoom(string roomName, string id)
        {
            if (rooms.ContainsKey(roomName))
            {
                Room room = GetRoom(roomName);
                room.GameContext.PlayerManager.DeletePlayer(id);
                room.Clients.Remove(id);
                if (room.GameContext.PlayerManager.Players.Count <= 0)
                {
                    applicationContext.CacheManager.RemoveCache(id);
                    room.GameContext.FinishGame();
                    rooms.Remove(roomName);

                    return true;
                }
            }
            return false;
        }

        public Room? GetRoom(string roomName)
        {
            if (!rooms.TryGetValue(roomName, out Room? value)) return null;
            return value;
        }
        public Room? GetClientRoom(string id)
        {
            var room = rooms.FirstOrDefault(r => r.Value.Clients.Any(c => c.Key == id));
            if (room.Value is not null)
                return room.Value;
            return null;
        }
        public ISingleClientProxy? GetClientProxy(string id)
        {
            Room room = GetClientRoom(id);
            if (room is not null)
            {
                Client b;
                room.Clients.TryGetValue(id, out b);
                return b?.ClientProxy;
            }
            return null;
        }
        public Client? GetClient(string id)
        {
            Room room = GetClientRoom(id);
            if (room is not null)
            {
                Client b;
                room.Clients.TryGetValue(id, out b);
                return b;
            }
            return null;
        }
        public Client? UpdateClient(string id, ISingleClientProxy clientProxy, string connectionId)
        {
            Room room = GetClientRoom(id);
            if (room is not null)
            {
                room.Clients[id].ClientProxy = clientProxy;
                room.Clients[id].ConnectionId = connectionId;
                return room.Clients[id];
            }
            return null;

        }

        public Room[] GetAllRooms()
        {
            return rooms.Values.Select(it => it).ToArray();
        }
        public RoomModel[] GetAllRoomsModel()
        {
            return rooms.Select(it => new RoomModel(it.Value)).ToArray();
        }
    }
}
