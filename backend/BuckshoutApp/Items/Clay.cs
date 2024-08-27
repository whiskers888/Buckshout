using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Clay(GameContext context) : Item(context)
    {
        public override string Name => "Глина";
        public override string Description => $"Вы клонируете предмет который есть у ваших противников.\n" +
                                              "Если у Вас недостаточно ячеек, предмет просто исчезнет.\n" +
                                               "Слепленный предмет нельзя украсть";
        public override string Lore => "Слеплено из г**** и палок!... Из глины, там говорится про глину...!";
        public override string Model => "clay";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.ITEM;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            /*{ItemEvent.USED, "hat/magic"},
            {ItemEvent.CANCELED, "hat/clown"},*/
        };

        public override void Effect(EventData e)
        {
            var item = (Item)e.Special["TARGET_ITEM"];
            if (item != null)
            {
                var createdItem = (Item)Activator.CreateInstance(item.GetType(), [Context])!;
                createdItem.Modifiers.Add(Context.ModifierManager.CreateModifier(ModifierKey.ITEM_CANNOT_BE_STOLEN));
                e.Initiator!.AddItem(createdItem);
            }
        }
    }
}
