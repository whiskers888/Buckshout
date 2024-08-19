using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Stopwatch : Item
    {
        public Stopwatch(GameContext context) : base(context)
        {
        }

        public override string Name => "Секундомер";
        public override string Description => $"Время хода всех противников уменьшается в {TURN_TIME_DIVIDER} раза." +
                                               "Действует до вашего следующего хода.\n" +
                                               "Полностью сочетается с другими секундомерами.";
        public override string Model => "stopwatch";
        public int TURN_TIME_DIVIDER = 2;

        public override void Effect(EventData e)
        {
            foreach (var player in Context.PlayerManager.Players)
            {
                if (player != e.initiator)
                {
                    var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_STOPWATCH);
                    modifier.Value = TURN_TIME_DIVIDER;
                    modifier.Apply(player);
                }
            }
        }
    }
}
