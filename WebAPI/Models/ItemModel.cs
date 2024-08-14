using BuckshoutApp.Items;

namespace Buckshout.Models
{
    public class ItemModel
    {
        public ItemModel() { }
        public ItemModel(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            TargetType = item.TargetType;
            TargetTeam = item.TargetTeam;
            Behavior = item.Behavior;
            Model = item.Model;
            Lore = item.Lore;
            Type = item.ItemType;
        }
        public string Id { get; set; } = "Empty";
        public string Name { get; set; } = "Empty";
        public string Description { get; set; } = "Empty"; 
        public string Lore { get; set; } = "";
        public string Model { get; set; } = "unknown";
        public TargetType TargetType { get; set; } = TargetType.NONE;
        public TargetTeam TargetTeam { get; set; } = TargetTeam.NONE;
        public ItemType Type { get; set; } = ItemType.DEFAULT;
        public ItemBehavior[] Behavior { get; set; } = [ItemBehavior.NO_TARGET];
    }
}
