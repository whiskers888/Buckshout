using BuckshoutApp.Manager;

namespace Buckshout.Models
{
    public class PlayerModel
    {
        public PlayerModel() { }
        public PlayerModel(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Health = player.Health;
            Inventory = player.Inventory.Select(it => new ItemModel(it)).ToArray();
            Team = player.Team;
            Avatar = player.Avatar;
        }
        public string Id { get; set; } = "Empty";
        public string Name { get; set; } = "Empty";
        public int Health { get; set; } = 0;
        public ItemModel[] Inventory { get; set; } = [];
        public string Team { get; set; } = "";
        public int Avatar { get; set; }
    }
}
