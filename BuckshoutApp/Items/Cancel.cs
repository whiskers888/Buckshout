using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Cancel(GameContext context) : Item(context)
    {
        public override string Name => "Печать \"Переделать\"";

        public override string Description => "Можно применить в течение " + context.Settings.ITEM_CHANNELING_TIME / 1000 + " сек. после того, как любой игрок попытался использовать предмет.\n" +
                                              "Отменяет эффект этого предмета.\n" +
                                              "Действие печати также можно отменить другой печатью, это приведет к тому, что действие предыдущего предмета не отменится.";

        public override string Lore => "Все х**ня, переделать!";

        public override string Model => "cancel";

        public override ItemBehavior[] Behavior => [ItemBehavior.NO_TARGET, ItemBehavior.CUSTOM];

        internal override void BeforeUse(EventData e)
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.ItemState = ItemState.DELAYED;
        }

        public override void Effect(EventData e)
        {
            Console.WriteLine($"{e.initiator?.Name} применил {Name} на {e.target?.Name}  ");
            Item target = Context.ItemManager.GetLastAfter(this);
            target?.Cancel();
        }

        internal override void BeforeCancel()
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.ItemState = ItemState.USING;
        }

    }
}
