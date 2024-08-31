using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Stopwatch(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Секундомер";
        public override string Description => $"Время следующего хода каждого из противников уменьшается в {TURN_TIME_DIVIDER} раза.\n" +
                                               "Эффект не сбрасывается при окончании раунда.\n" +
                                               "Полностью сочетается с другими секундомерами.";
        public override string Lore => "Для тех, кто не любит долго ждать смерти.";
        public override string Model => "stopwatch";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "stopwatch/mechanism"},
            {ItemEvent.EFFECTED, "stopwatch/tick"}
        };

        public int TURN_TIME_DIVIDER = 2;

        public override void Effect(EventData e)
        {
            foreach (var player in Context.PlayerManager.Players)
            {
                if (player.Team != e.Initiator!.Team)
                {
                    var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_STOPWATCH);
                    modifier.Value = TURN_TIME_DIVIDER;
                    modifier.Apply(player);
                    // Писал этот баг тебе в тг, пока сделал так
                    modifier.Remove(Manager.Events.Event.ROUND_FINISHED);
                }
            }
        }
    }
}
