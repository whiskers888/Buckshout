using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Cigarettes : Item
    {
        public Cigarettes(GameContext context) : base(context)
        {
        }

        public override string Name => "Сигаретты";
        public override string Description => $"Восстанавливает {COUNT_HEALTH} ед. здоровья.";
        public override string Model => "cigarettes";
        public int COUNT_HEALTH = 1;

        public override void Effect(EventData e)
        {
            e.initiator.ChangeHealth(Manager.ChangeHealthType.Heal, COUNT_HEALTH, e.initiator);
        }
    }
}
