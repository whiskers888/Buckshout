using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;

namespace Buckshout.Managers
{
    public class Player(string name, string connectionId)
    {
        public string Name { get; set; } = name;
        public string ConnectionId { get; set; } = connectionId;
    }

    public class Room(string roomName, GameContext gameContext, IClientProxy group)
    {
        public string RoomName { get; set; } = roomName;
        public List<Player> Players { get; set; } = [];
        public IClientProxy Group { get; set; } = group;
        public GameContext GameContext { get; set; } = gameContext;
    }


    public class RoomManager
    {
        private readonly Dictionary<string, Room> rooms = [];

        public void AddToRoom(string roomName, Player player, IClientProxy group)
        {
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = new Room(roomName, new GameContext(), group);
            }
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
        public List<string> GetAllRooms()
        {
            return [.. rooms.Keys];
        }
    }
}
