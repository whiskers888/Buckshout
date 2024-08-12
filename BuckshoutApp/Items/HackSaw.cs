using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Hacksaw:Item
    {
        public Hacksaw(GameContext context) : base(context) { }

        public override string Name => "Ножовка";
        public override string Description => "Увеличивает урон дробовика";


        public override void Effect(EventData e)
        {
            Context.RifleManager.Damage += 1;

            Context.EventManager.Once(Event.TURN_CHANGED, (e) =>
            {
                Context.RifleManager.Damage -= 1;
            });
        }
    }
}
