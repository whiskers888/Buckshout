using BuckshoutApp.Items;

namespace Buckshout.Models
{
    public class ItemModel(Item item)
    {
        public string Id { get; set; } = item.Id;
        public string Name { get; set; } = item.Name;
        public string Description { get; set; } = item.Description;
        public string Lore { get; set; } = item.Lore;
        public string Model { get; set; } = item.Model;
        public TargetType TargetType { get; set; } = item.TargetType;
        public TargetTeam TargetTeam { get; set; } = item.TargetTeam;
        public ItemType Type { get; set; } = item.ItemType;
        public ItemBehavior[] Behavior { get; set; } = item.Behavior;
    }
}
