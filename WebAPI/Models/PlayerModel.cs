using BuckshoutApp.Manager;

namespace Buckshout.Models
{
    public class PlayerModel(Player player)
    {
        public string Id { get; set; } = player.Id;
        public string Name { get; set; } = player.Name;
        public int Health { get; set; } = player.Health;
        public ItemModel[] Inventory { get; set; } = player.Inventory.Select(it => new ItemModel(it)).ToArray();
        public PlayerModifierModel[] Modifiers { get; set; } = player.Modifiers.Select(it => new PlayerModifierModel(it)).ToArray();
        public string Team { get; set; } = player.Team;
        public int Avatar { get; set; } = player.Avatar;
    }
}
