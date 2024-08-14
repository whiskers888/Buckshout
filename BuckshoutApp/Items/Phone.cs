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
        public override string Description => "Сообщает вам, заряжен ли случайный патрон (счет идет от текущего).";
        public override string Model => "phone";
        public override void Effect(EventData e)
        {
            int countPatrons = Context.Rifle.Patrons.Count ;
            int indexRndPatron = Context.Random.Next(0, countPatrons - 1);
            bool isCharged = Context.Rifle.Patrons[indexRndPatron].IsCharged;
            e.special.Add("MESSAGE", $"{indexRndPatron + 1} патрон" + (isCharged ? "заряжен" : "не заряжен")); 
            Context.EventManager.Trigger(Event.MESSAGE_INITIATOR_RECEIVED, e);;
        }
    }
}
