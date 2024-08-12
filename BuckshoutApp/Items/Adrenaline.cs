using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Adrenaline : Item
    {
        public Adrenaline(GameContext context) : base(context)
        {
        }

        public override string Name => "Адреналин";
        public override string Description => "Крадет предмет на выбор.\n" +
                                            "Запрещено брать адреналин и наручники\n" +
                                            "Используйте предмет, иначе он пропадет на следующий ход";
        public override ItemBehavior[] Behavior { get; } = { ItemBehavior.UNIT_TARGET }; 
        public override TargetType TargetType => TargetType.ITEM;
        public override TargetTeam TargetTeam => TargetTeam.ENEMY;


        internal override void BeforeUse(EventData e) 
        {
            Player target = e.target;
            Item item = (Item)e.special["ITEM"];
            if (!target.Inventory.Contains(item))
                ItemState = ItemState.NOT_ALLOWED;
            if (!item.ItemModifier.Contains(Items.ItemModifier.CANNOT_BE_STOLEN))
            {
                e.special.Add("MESSAGE", $"{Name} нельзя применить на {item.Name}");
                Context.EventManager.Trigger(Event.MESSAGE_RECEIVED, e);
            }
            ItemState = ItemState.NOT_ALLOWED;
        }
        public override void Effect(EventData e)
        {
            Item item = (Item)e.special["ITEM"];
            e.target.Inventory.Remove(item);
            e.initiator.Inventory.Add(item);
            Context.EventManager.Once(Event.TURN_CHANGED, (e) =>
            {
                e.initiator.Inventory.Remove(item);
            });

        }
    }
}
