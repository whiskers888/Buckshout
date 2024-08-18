using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Adrenaline : Item
    {
        public Adrenaline(GameContext context) : base(context)
        {
            Modifiers.Add(Context.ModifierManager.CreateModifier(ModifierKey.ITEM_CANNOT_BE_STOLEN));
        }
        public override string Name => "Адреналин";
        public override string Description => "Вы забираете выбранный предмет себе.\n" +
                                            "Запрещено применять на: Адреналин, Глина.\n" +
                                            "Украденный предмет исчезнет в конце хода, если его не использовать.";
        public override string Model => "adrenaline";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.ITEM;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;
        internal override void BeforeUse(EventData e)
        {
            Item item = (Item)e.special["TARGET_ITEM"];
            if (item.Is(ModifierState.ITEM_CANNOT_BE_STOLEN))
            {
                Disallow(e, $"{Name} нельзя применить на {item.Name}");
            }
        }
        public override void Effect(EventData e)
        {
            Item item = (Item)e.special["TARGET_ITEM"];

            e.target!.Inventory.Remove(item);
            Context.EventManager.Trigger(Event.ITEM_STOLEN, e);
            e.initiator!.AddItem(item);
            Context.EventManager.Once(Event.TURN_CHANGED, (_) =>
            {
                e.initiator.RemoveItem(item);
            });
        }
    }
}
