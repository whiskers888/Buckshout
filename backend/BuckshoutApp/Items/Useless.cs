using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Useless(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Бесполезный предмет";
        public override string Description => "Он действительно бесполезный, и не делает ровным счетом НИЧЕГО!\n" +
                                              "Вам просто не повезло его получить...\n" +
                                              "[Но ходят слухи, что Вы узнаете истинную суть предмета, лишь забив ими весь свой инвентарь...]";
        public override string Lore => "Бесполезный предмет - верх запаян, а дна нет!";
        public override string Model => "useless";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "useless/break"},
        };

        public override void Effect(EventData e)
        {
            var count = e.Initiator!.Inventory.Where(it => it.Model == Model).Count() + 1;
            if (count == Context.Settings.MAX_INVENTORY_SLOTS)
                e.Special.Add("MESSAGE", "НИЧЕГО не произошло! Как, собственно, и всегда! Надеюсь, теперь Вам понятна истинная БЕСОЛЕЗНОСТЬ этого предмета?!");
            else if (count >= Context.Settings.MAX_INVENTORY_SLOTS / 2)
                e.Special.Add("MESSAGE", $"Ничего не произошло, но что, если бы их было на {Context.Settings.MAX_INVENTORY_SLOTS - count} шт. больше?..");
            else
                e.Special.Add("MESSAGE", "Ничего не произошло...");
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);
        }
    }
}
