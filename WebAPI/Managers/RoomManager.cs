using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;

namespace Buckshout.Managers
{
/*    public class UserModel(string name, string connectionId)
    {
        public string Name { get; set; } = name;
        public string ConnectionId { get; set; } = connectionId;
    }
*/
    public class Room(string roomName, GameContext gameContext, IClientProxy group)
    {
        public string RoomName { get; set; } = roomName;
        public IClientProxy Group { get; set; } = group;
        public GameContext GameContext { get; set; } = gameContext;
    }


    public class RoomManager
    {
        private readonly Dictionary<string, Room> rooms = [];

        public Room CreateRoom(string roomName, IClientProxy group)
        {
            var room = new Room(roomName, new GameContext(), group);
            if (!rooms.ContainsKey(roomName))
            {
                rooms[roomName] = room;
            }
            return room;
        }

        public void AddToRoom(string roomName, string id, string name)
        {
            if (!rooms.ContainsKey(roomName))
            {
                return;
            }
            rooms[roomName].GameContext.PlayerManager.AddPlayer(id, name);
            // rooms[roomName].Players.Add(player);
        }

        public bool RemoveFromRoom(string roomName, string connectionId)
        {
            if (rooms.ContainsKey(roomName))
            {
                rooms[roomName].GameContext.PlayerManager.DeletePlayer(connectionId);
                if (rooms[roomName].GameContext.PlayerManager.Players.Count <= 0)
                {
                    GetRoom(roomName).GameContext.FinishGame();
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
        public RoomModel[] GetAllRooms()
        {
            return rooms.Select(it => new RoomModel(it.Value)).ToArray();
        }
    }
}
