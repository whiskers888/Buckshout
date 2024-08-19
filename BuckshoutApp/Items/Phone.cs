using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
namespace BuckshoutApp.Items
{
    public class Phone : Item
    {
        public Phone(GameContext context) : base(context)
        {
        }
        public override string Name => "Телефон";
        public override string Description => "Сообщает Вам, заряжен ли случайный патрон (счет идет от текущего).";
        public override string Lore => "Алло, ну как там с патронами?";
        public override string Model => "phone";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "phone/beeps"},
            {ItemEvent.CANCELED, "phone/connection_lost"}
        };

        public override void Effect(EventData e)
        {
            int countPatrons = Context.Rifle.Patrons.Count;
            int indexRndPatron = Context.Random.Next(0, countPatrons - 1);
            bool isCharged = Context.Rifle.Patrons[indexRndPatron].IsCharged;
            e.special.Add("MESSAGE", new string[] {
            "Привет...." ,
            "..Это твой бекендер....",
            "..Тут такой п*.....",
            $"..В общем {countPatrons - indexRndPatron} патрон... ",
            isCharged ? " заряжен" : " не заряжен",
            "..вроде...",
            "...Удачи!"
            });
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);
        }
        internal override void OnCanceled(EventData e)
        {
            e.special.Add("MESSAGE", new string[] {
                "Извините....",
                "..связь прервалась..." });
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);
        }
    }
}
