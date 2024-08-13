using BuckshoutApp.Manager;

namespace Buckshout.Models
{
    public class PlayerModel
    {
        public PlayerModel() { }
        public PlayerModel(Player player)
        {
            UUID = player.UUID;
            Name = player.Name;
            Health = player.Health;
            Inventory = player.Inventory.Select(it => new ItemModel(it)).ToArray();
        }
        public string UUID { get; set; } = "Empty";
        public string Name { get; set; } = "Empty";
        public int Health { get; set; } = 0;
        public ItemModel[] Inventory { get; set; } = [];
    }
}
