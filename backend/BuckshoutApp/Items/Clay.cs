using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Clay : Item
    {
        public Clay(GameContext context) : base(context)
        {
            Modifiers.Add(Context.ModifierManager.CreateModifier(ModifierKey.ITEM_CANNOT_BE_STOLEN));
        }
        public override string Name { get; set; } = "Глина";
        public override string Description => $"Вы создаете глиняную копию выбранного предмета.\n" +
                                               "Предмет, созданный таким образом, нельзя украсть.";

        public override string Lore => "Слеплено из г**** и палок!... Из глины, там говорится про глину...!";
        public override string Model => "clay";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.ITEM;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "clay/craft"},
        };

        public override void Effect(EventData e)
        {
            var item = (Item)e.Special["TARGET_ITEM"];
            if (item != null)
            {
                var createdItem = (Item)Activator.CreateInstance(item.GetType(), [Context])!;
                createdItem.Name += " из Глины";
                createdItem.Modifiers.Add(Context.ModifierManager.CreateModifier(ModifierKey.ITEM_CLAY));
                e.Initiator!.AddItem(createdItem);
            }
        }
    }
}
