using BuckshoutApp.Items;
using BuckshoutApp.Modifiers;

namespace Buckshout.Models
{
    public class ItemModel(Item item)
    {
        public string Id { get; set; } = item.Id;
        public string Name { get; set; } = item.Name;
        public string Description { get; set; } = item.Description;
        public string Lore { get; set; } = item.Lore;
        public string Model { get; set; } = item.Model;
        public ItemType Type { get; set; } = item.Type;
        public ItemBehavior[] Behavior { get; set; } = item.Behavior;
        public ItemTargetTeam TargetTeam { get; set; } = item.TargetTeam;
        public ItemTargetType TargetType { get; set; } = item.TargetType;
        public ModifierState[] IgnoreTargetState { get; } = item.IgnoreTargetState;
        public ModifierModel[] Modifiers { get; set; } = item.Modifiers.Select(it => new ModifierModel(it)).ToArray();
        public Dictionary<ItemEvent, string> SoundSet { get; set; } = item.SoundSet;
    }
}
