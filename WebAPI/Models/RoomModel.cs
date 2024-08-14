using Buckshout.Managers;
using BuckshoutApp.Context;

namespace Buckshout.Models
{
    public class RoomModel
    {
        public RoomModel(Room room)
        {
            name = room.RoomName;
            game = new GameModel(room.GameContext);
        }

        public string name { get; set; }
        public GameModel game { get; set; }
    }
}
