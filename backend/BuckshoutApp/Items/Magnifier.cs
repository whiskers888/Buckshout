using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    internal class Magnifier(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Лупа";
        public override string Description => "Показывает, какой патрон заряжен в дробовик.\n" +
                                              $"Эффект отображается на дробовике в течение {DURATION / 1000} сек.";

        public override string Lore => "А потом он взял дробовик, засунул себе его прямо в рот и спустил курок, но это уже совсем другая история...";
        public override string Model => "magnifier";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "magnifier/theme"},
        };

        public int DURATION = 5000;

        public override void Effect(EventData e)
        {
            e.Special.Add("IS_CHARGED", Context.Rifle.Patrons[^1].IsCharged);
            e.Special.Add("DURATION", DURATION);
            Context.EventManager.Trigger(Event.RIFLE_CHECKED, e); ;
        }
    }
}
