using BuckshoutApp.Modifiers;

namespace Buckshout.Models
{
    public class ModifierModel(Modifier modifier)
    {
        public string Id { get; set; } = modifier.Id;
        public string Name { get; set; } = modifier.Name;
        public string Description { get; set; } = modifier.Description;
        public int Duration { get; set; } = modifier.Duration;
        public string Icon { get; set; } = modifier.Icon;
        public bool IsBuff { get; set; } = modifier.IsBuff;
        public int Value { get; set; } = modifier.Value;

        public ModifierTargetType TargetType { get; set; } = modifier.TargetType;
        public ModifierState[] State { get; set; } = modifier.State.ToArray();
    }

}
