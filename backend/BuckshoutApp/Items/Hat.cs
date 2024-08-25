using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Hat(GameContext context) : Item(context)
    {
        public override string Name => "Шляпа";
        public override string Description => $"Вы достаете себе {ITEMS_COUNT} случайных предмета.\n" +
                                              "Если если у Вас недостаточно ячеек, они просто исчезнут.";
        public override string Lore => "Ловкость рук и никакого мошенничества!";
        public override string Model => "hat";

        public int ITEMS_COUNT = 2;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "hat/magic"},
            {ItemEvent.CANCELED, "hat/clown"},
        };

        public override void Effect(EventData e)
        {
            for (int i = 0; i < ITEMS_COUNT; i++)
            {
                e.Initiator.AddItem(Context.ItemManager.NextItem());
            }
        }
    }
}
