using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Cigarettes(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Сигареты";
        public override string Description => $"Восстанавливает {COUNT_HEALTH} ед. здоровья.";
        public override string Lore => "Курение исцеляет.";
        public override string Model => "cigarettes";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "cigarettes/smoke"},
            {ItemEvent.CANCELED, "cigarettes/cough"}
        };

        public int COUNT_HEALTH = 1;

        public override void Effect(EventData e)
        {
            e.Initiator!.ChangeHealth(Manager.ChangeHealthType.Heal, COUNT_HEALTH, e.Initiator);
        }
    }
}
