using BuckshoutApp.Items;

namespace Buckshout.Models
{
    public class ItemModel
    {
        public ItemModel() { }
        public ItemModel(Item item)
        {
            UUID = item.UUID;
            Name = item.Name;
            Description = item.Description;
            TargetType = item.TargetType;
            TargetTeam = item.TargetTeam;

        }
        public string UUID { get; set; } = "Empty";
        public string Name { get; set; } = "Empty";
        public string Description { get; set; } = "Empty";
        public bool IsStealable { get; set; }
        public TargetType TargetType { get; set; }
        public TargetTeam TargetTeam { get; set; }
    }
}
