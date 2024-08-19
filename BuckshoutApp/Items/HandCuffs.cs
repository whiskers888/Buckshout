using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Handcuffs : Item
    {
        public Handcuffs(GameContext context) : base(context)
        {
        }

        public override string Name => "Наручники";
        public override string Description => "Выбранный противник пропускает свой следующий ход.";
        public override string Model => "handcuffs";
        public override ItemBehavior[] Behavior { get; } = { ItemBehavior.UNIT_TARGET };
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;

        internal override void BeforeUse(EventData e)
        {
            if (e.target.Is(ModifierState.PLAYER_STUNNED))
            {
                Disallow(e, "Этот игрок уже пропускает ход!");
            }
        }

        public override void Effect(EventData e)
        {
            Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_HANDCUFFS).Apply(e.target);
        }
    }
}
