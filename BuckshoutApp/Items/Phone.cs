using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
namespace BuckshoutApp.Items
{
    public class Phone:Item
    {
        public Phone(GameContext context) : base(context)
        {
        }

        public override string Name => "Телефон";
        public override string Description => "Расскрывает случайную пулю ( от текущей пули)";


        public override void Effect(EventData e)
        {
            int countPatrons = Context.RifleManager.Patrons.Length;
            int indexRndPatron = Context.Random.Next(0, countPatrons);
            bool isCharged = Context.RifleManager.Patrons[indexRndPatron].IsCharged;
            e.special.Add("MESSAGE", $"{indexRndPatron + 1} патрон" + (isCharged ? "заряжен" : "не заряжен")); 
            Context.EventManager.Trigger(Event.MESSAGE_INITIATOR_RECEIVED, e);;
        }
    }
}
