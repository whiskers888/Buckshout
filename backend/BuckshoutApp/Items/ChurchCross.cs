using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class ChurchCross(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Крестик";
        public override string Description => $"Дает {CHANCE_EVASION}% шанс увернуться от заряженного патрона {QUANTITY_EVASION} раз до своего следующего хода.\n" +
                                              "Увернувшись при выстреле в самого себя, Вы сохраните право хода.";

        public override string Lore => "Перед дулом дробовика атеистов не бывает...";
        public override string Model => "church_cross";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "cross/church_bell" },
            // {ItemEvent.EFFECTED, "cross/hallelujah"},
        };

        public int CHANCE_EVASION = 50;
        public int QUANTITY_EVASION = 1;

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHURCH_CROSS);
            modifier.Value = CHANCE_EVASION;
            modifier.Apply(e.Initiator!);
            modifier.RemoveWhen(Event.RIFLE_SHOT, null, (e) =>
            {
                return (bool)e.Special["IS_MISSING"];
            });
        }
    }
}
