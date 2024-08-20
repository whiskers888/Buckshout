using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class ChurchCross(GameContext context) : Item(context)
    {
        public override string Name => "Крестик";
        public override string Description => $"{CHANCE_EVASION}% шанс увернуться от заряженного патрона {QUANTITY_EVASION} раз до своего следующего хода.\n" +
                                              "Увернувшись, при выстреле в самого себя, Вы сохраните право хода.";
        public override string Model => "church_cross";

        public int CHANCE_EVASION = 50;
        public int QUANTITY_EVASION = 1;

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHURCH_CROSS);
            modifier.Value = CHANCE_EVASION;
            modifier.Apply(e.initiator);
            modifier.RemoveWhen(Event.RIFLE_SHOT, null, (e) =>
            {
                return (bool)e.special["IS_MISSING"];
            });
        }
    }
}
