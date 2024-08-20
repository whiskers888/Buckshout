using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Cigarettes(GameContext context) : Item(context)
    {
        public override string Name => "Сигареты";
        public override string Description => $"Восстанавливает {COUNT_HEALTH} ед. здоровья.";
        public override string Lore => "Курение исцеляет.";
        public override string Model => "cigarettes";

        public int COUNT_HEALTH = 1;

        public override void Effect(EventData e)
        {
            e.initiator.ChangeHealth(Manager.ChangeHealthType.Heal, COUNT_HEALTH, e.initiator);
        }
    }
}
