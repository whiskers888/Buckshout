using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;

namespace Buckshout.Managers
{
    public class Room(string roomName, GameContext gameContext, IClientProxy group)
    {
        public string Name { get; set; } = roomName;
        public IClientProxy Group { get; set; } = group;
        public Dictionary<string, IClientProxy> Clients { get; set; } = [];
        public GameContext GameContext { get; set; } = gameContext;
    }


    public class RoomManager
    {
        private readonly Dictionary<string, Room> rooms = new();

        public Room CreateRoom(string roomName, IClientProxy group)
        {

            var room = new Room(roomName, new GameContext(), group);
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = room;
            }
            return room;
        }

        public void AddToRoom(string roomName, ISingleClientProxy client, string connectionId, string name)
        {
            if (!rooms.ContainsKey(roomName))
            {
                return;
            }
            Room room = GetRoom(roomName);
            room.GameContext.PlayerManager.AddPlayer(connectionId, name);
            room.Clients.Add(connectionId, client);
        }

        public bool RemoveFromRoom(string roomName, string connectionId)
        {
            if (rooms.ContainsKey(roomName))
            {
                Room room = GetRoom(roomName);
                room.GameContext.PlayerManager.DeletePlayer(connectionId);
                room.Clients.Remove(connectionId);
                if (room.GameContext.PlayerManager.Players.Count <= 0)
                {
                    room.GameContext.FinishGame();
                    rooms.Remove(roomName);
                    return true;
                }
            }
            return false;
        }

        public Room GetRoom(string roomName)
        {
            return rooms[roomName];
        }
        public Room GetClientRoom(string connectionId)
        {
            return rooms.FirstOrDefault(r => r.Value.Clients.Any(c => c.Key == connectionId)).Value;
        }
        public IClientProxy? GetClient(string connectionId)
        {
            return GetClientRoom(connectionId).Clients.FirstOrDefault(it => it.Key == connectionId).Value;
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
