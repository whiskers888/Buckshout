using BuckshoutApp.Modifiers;

namespace Buckshout.Models
{
    public class ModifierModel(Modifier modifier)
    {
        public string Id { get; set; } = modifier.Id;
        public string Name { get; set; } = modifier.Name;
        public string Description { get; set; } = modifier.Description;
        public string Icon { get; set; } = modifier.Icon;
        public int Duration { get; set; } = modifier.Duration;
        public bool IsBuff { get; set; } = modifier.IsBuff;
    }

    public class PlayerModifierModel(PlayerModifier modifier) : ModifierModel(modifier)
    {
        public PlayerModifierState[] State { get; set; } = modifier.State.ToArray();
    }
}
