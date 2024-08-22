using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Useless(GameContext context) : Item(context)
    {
        public override string Name => "Бесполезный предмет";
        public override string Description => "Он действительно бесполезный, и не делает ровным счетом ничего!\n" +
                                              "Вам просто не повезло его получить...\n" +
                                              "[Но ходят слухи, что вы узнаете истинную суть предмета, лишь забив ими весь свой инвентарь...]";
        public override string Lore => "Бесполезный предмет - верх запаян, а дна нет!";
        public override string Model => "useless";

        public override void Effect(EventData e)
        {
            var count = e.initiator.Inventory.Where(it => it.Name == Name).Count() + 1;
            if (count == Context.Settings.MAX_INVENTORY_SLOTS)
                e.special.Add("MESSAGE", "Ничего не произошло! Как, собственно, и всегда! Надеюсь, теперь Вам понятна истинная бесполезность этого предмета...");
            else if (count >= Context.Settings.MAX_INVENTORY_SLOTS / 2)
                e.special.Add("MESSAGE", $"Ничего не произошло, но что, если бы их было на {Context.Settings.MAX_INVENTORY_SLOTS - count} шт. больше?..");
            else
                e.special.Add("MESSAGE", "Ничего не произошло...");
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);
        }
    }
}
