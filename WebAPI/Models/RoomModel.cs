using Buckshout.Managers;

namespace Buckshout.Models
{
    public class RoomModel(Room room)
    {
        public string name { get; set; } = room.RoomName;
        public GameModel game { get; set; } = new GameModel(room.GameContext);
    }
}
