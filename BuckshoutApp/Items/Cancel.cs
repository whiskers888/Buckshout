using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Cancel : Item
    {
        public Cancel(GameContext context) : base(context) { }

        public override string Name => "Печать \"Переделать\"";

        public override string Description => "Все фигня, переделывай";

        internal override void BeforeUse(EventData e)
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.ItemState = ItemState.DELAYED;
        }

        public override void Effect(EventData e)
        {
            Console.WriteLine($"{e.initiator?.Name} применил {Name} на {e.target?.Name}  ");
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
