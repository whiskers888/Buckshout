using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Beer:Item
    {
        public Beer(GameContext context) : base(context)
        {
        }

        public override string Name => "Пееевооо";
        public override string Description => "Сбрасывает одну пулю, не теряя ход.";


        public override void Effect(EventData e)
        {
            e.special.Add("MESSAGE", Context.RifleManager.NextPatron());
            Context.EventManager.Trigger(Event.MESSAGE_RECEIVED , e);
        }
    }
}
