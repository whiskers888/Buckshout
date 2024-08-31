using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class ChurchCross(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Крестик";
        public override string Description => $"Дает {CHANCE_EVASION}% шанс увернуться от заряженного патрона {QUANTITY_EVASION} раз до своего следующего хода.\n" +
                                              "Увернувшись, при выстреле в самого себя, Вы сохраните право хода.";

        public override string Lore => "Перед дулом дробовика атеистов не бывает...";
        public override string Model => "church_cross";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "cross/church_bell" },
        };

        public int CHANCE_EVASION = 60;
        public int QUANTITY_EVASION = 1;

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHURCH_CROSS);
            modifier.Value = CHANCE_EVASION;
            modifier.Apply(e.Initiator!);

            var id = "";
            id = Context.EventManager.SubscribeUniq(Event.RIFLE_SHOT, shotE =>
            {
                if (shotE.Target == e.Initiator)
                {
                    if ((bool)shotE.Special["IS_MISSING"])
                    {
                        e.Special["SOUND"] = "items/cross/hallelujah";
                        Context.EventManager.Trigger(Event.PLAY_SOUND, e);
                        Context.EventManager.Unsubscribe(Event.RIFLE_SHOT, id);
                    }
                }
            });
        }
    }
}
