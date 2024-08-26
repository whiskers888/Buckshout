using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Stamp(GameContext context) : Item(context)
    {
        public override string Name => "Печать \"Переделать\"";

        public override string Description => "Можно применить только в течение " + context.Settings.ITEM_CHANNELING_TIME / 1000 + " сек. после того, как любой игрок попытался использовать предмет.\n" +
                                              "Отменяет эффект этого предмета.\n" +
                                              "Действие печати также можно отменить другой печатью, это приведет к тому, что действие предыдущего предмета не отменится.";

        public override string Lore => "Все х**ня, переделать!";

        public override string Model => "stamp";

        public override ItemBehavior[] Behavior => [ItemBehavior.NO_TARGET, ItemBehavior.CUSTOM];

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "stamp/stamp"},
        };

        internal override void BeforeUse(EventData e)
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.State = ItemState.DELAYED;
        }

        public override void Effect(EventData e)
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            target?.Cancel(e);
        }

        internal override void BeforeCancel()
        {
            Item target = Context.ItemManager.GetLastAfter(this);
            if (target is not null) target.State = ItemState.USING;
        }

    }
}
