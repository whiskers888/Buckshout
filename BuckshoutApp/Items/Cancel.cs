using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Cancel : Item
    {
        public Cancel(GameContext context) : base(context) { }

        public override string Name => "Печать \"Переделать\"";

        public override string Description => "Все фигня, переделывай";

        internal override void BeforeUse(EventData args)
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.ItemState = ItemState.DELAYED;
        }

        public override void Effect(EventData args)
        {
            Console.WriteLine($"{args.initiator?.Name} применил {Name} на {args.target?.Name}  ");
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.Cancel();
        }

        internal override void BeforeCancel()
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.ItemState = ItemState.USING;
        }

    }
}
