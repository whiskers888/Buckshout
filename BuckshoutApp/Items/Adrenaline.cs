using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        internal override void BeforeUse(EventData args) 
        {
            Player target = args.target;
            Item item = (Item)args.special["ITEM"];
            if (!target.Inventory.Contains(item))
                ItemState = ItemState.NOT_ALLOWED;
            if (!item.ItemModifier.Contains(Items.ItemModifier.CANNOT_BE_STOLEN))
                Context.EventManager.Trigger(Event.MESSAGE_RECEIVED, new EventData()
                {
                    special = { { "MESSAGE", $"{Name} нельзя применить на {item.Name}" } }
                });
            ItemState = ItemState.NOT_ALLOWED;
        }
        public override void Effect(EventData args)
        {
            Item item = (Item)args.special["ITEM"];
            args.target.Inventory.Remove(item);
            args.initiator.Inventory.Add(item);
            Context.EventManager.Once(Event.TURN_CHANGED, (e) =>
            {
                args.initiator.Inventory.Remove(item);
            });

        }
    }
}
