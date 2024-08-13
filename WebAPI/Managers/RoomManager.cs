using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;

namespace Buckshout.Managers
{
    public class UserModel(string name, string connectionId)
    {
        public string Name { get; set; } = name;
        public string ConnectionId { get; set; } = connectionId;
        public bool CreatedRoom { get; set; } = false;
    }

    public class Room(string roomName, GameContext gameContext, IClientProxy group)
    {
        public string RoomName { get; set; } = roomName;
        public List<UserModel> Players { get; set; } = [];
        public IClientProxy Group { get; set; } = group;
        public GameContext GameContext { get; set; } = gameContext;
    }


    public class RoomManager
    {
        private readonly Dictionary<string, Room> rooms = [];

        public void CreateRoom(string roomName, IClientProxy group)
        {
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = new Room(roomName, new GameContext(), group);
            }
        }
        public void AddToRoom(string roomName, UserModel player, IClientProxy group)
        {
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = new Room(roomName, new GameContext(), group);
            }
            if (rooms[roomName].Players.Count == 1)
                player.CreatedRoom = true;
            rooms[roomName].GameContext.PlayerManager.AddPlayer(player.ConnectionId, player.Name);
            rooms[roomName].Players.Add(player);
        }

        public void RemoveFromRoom(string roomName, string connectionId)
        {
            if (rooms.ContainsKey(roomName))
            {
                rooms[roomName].Players.Remove(rooms[roomName].Players.First(it => it.ConnectionId == connectionId));
                if (rooms[roomName].Players.Count == 0)
                {
                    rooms.Remove(roomName);
                }
            }
        }
        public Room GetRoom(string roomName)
        {
            return rooms.FirstOrDefault(it => it.Key == roomName).Value;
        }
        public Dictionary<string, RoomModel> GetAllRooms()
        {
            return rooms.ToDictionary(room => room.Key, room => new RoomModel(room.Value.Players));
        }
    }
}
