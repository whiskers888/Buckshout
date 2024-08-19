using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Useless(GameContext context) : Item(context)
    {
        public override string Name => "Бесполезный предмет";
        public override string Description => "Он действительно бесполезный, и не делает ровным счетом ничего!\n" +
                                              "Вам просто не повезло его получить...";
        public override string Lore => "Бесполезный предмет - верх запаян, а дна нет!";
        public override string Model => "useless";

        public override void Effect(EventData e)
        {
            e.special.Add("MESSAGE", "Ничего не произошло...");
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);
        }
    }
}
