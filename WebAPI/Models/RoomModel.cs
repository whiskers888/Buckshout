using Buckshout.Managers;

namespace Buckshout.Models
{
    public class RoomModel
    {
        public RoomModel(List<UserModel> players)
        {
            this.players = players.ToArray();
        }

        public string roomName { get; set; }
        public UserModel[] players { get; set; }
    }
}
