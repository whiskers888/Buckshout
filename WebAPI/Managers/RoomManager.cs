using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using System.Security.Cryptography.X509Certificates;

namespace Buckshout.Managers
{
    public class Player
    { 
        public Player(string name, string connectionId)
        {
            Name = name;
            ConnectionId = connectionId;
        }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }

    public class Room
    {
        public Room(string roomName, GameContext gameContext) 
        {
            RoomName = roomName;
            GameContext = gameContext;
            Players = new List<Player>();
        }
        public string RoomName { get; set; }
        public List<Player> Players { get; set; }
        public GameContext GameContext { get; set; }
    }
    

    public class RoomManager
    {
        private static readonly Dictionary<string, Room> rooms = new Dictionary<string, Room>();

        public static void AddToRoom(string roomName, Player player)
        {
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = new Room(roomName, new GameContext());
            }
            rooms[roomName].GameContext.PlayerManager.AddPlayer(player.ConnectionId, player.Name);
            rooms[roomName].Players.Add(player);
        }

        public static void RemoveFromRoom(string roomName, string connectionId)
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
        public static Room GetRoom(string roomName)
        {
            return rooms.FirstOrDefault(it => it.Key == roomName).Value;
        }
        public static List<string> GetAllRooms()
        {
            return rooms.Keys.ToList();
        }
    }
}
