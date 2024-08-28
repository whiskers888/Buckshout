using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Milk(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Ведро молока";
        public override string Description => "Снимает все отрицательные эффекты, если целью является союзник, или все положительние эффекты, если целью является противник.";
        public override string Lore => "";
        public override string Model => "milk";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "milk/drink"},
        };

        public override void Effect(EventData e)
        {
            if (e.Target!.Team == e.Initiator!.Team)
            {
                e.Target.Modifiers.ToList().ForEach(m =>
                {
                    if (!m.IsBuff)
                    {
                        e.Target.RemoveModifier(m);
                    }
                });
            }
            else
            {
                e.Target.Modifiers.ToList().ForEach(m =>
                {
                    if (m.IsBuff)
                    {
                        e.Target.RemoveModifier(m);
                    }
                });
            };
        }
    }
}
