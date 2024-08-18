using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    internal class Magnifier : Item
    {
        public Magnifier(GameContext context) : base(context)
        {
        }

        public override string Name => "Лупа";
        public override string Description => "Показывает какой патрон заряжен в данный момент в винтовку.";
        public override string Model => "magnifier";

        public override void Effect(EventData e)
        {
            e.special.Add("MESSAGE", Context.Rifle.NextPatron());
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e); ;
        }
    }
}
