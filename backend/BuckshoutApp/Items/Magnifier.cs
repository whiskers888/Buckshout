using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    internal class Magnifier(GameContext context) : Item(context)
    {
        public override string Name => "Лупа";
        public override string Description => "Показывает Вам, какой патрон заряжен в данный момент в дробовик.\n" +
                                              $"Эффект предмета отображается на самом дробовике в течение {DURATION / 1000} сек.";

        public override string Lore => "А потом он взял дробовик, засунул себе его прямо в рот и спустил курок, но это уже совсем другая история...";
        public override string Model => "magnifier";

        public int DURATION = 5000;

        public override void Effect(EventData e)
        {
            e.special.Add("IS_CHARGED", Context.Rifle.Patrons[^1].IsCharged);
            e.special.Add("DURATION", DURATION);
            Context.EventManager.Trigger(Event.RIFLE_CHECKED, e); ;
        }
    }
}
