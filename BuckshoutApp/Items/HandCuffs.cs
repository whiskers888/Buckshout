
using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Handcuffs : Item
    {
        public Handcuffs(GameContext context) : base(context)
        {
        }

        public override string Name => "Наручники";
        public override string Description => "Противник пропускает свой следующий ход";
        public override TargetType TargetType => TargetType.PLAYER;
        public override TargetTeam TargetTeam => TargetTeam.ENEMY;
        public override ItemBehavior[] Behavior { get; } = { ItemBehavior.UNIT_TARGET };

        public override void Effect(EventData args)
        {
            Console.WriteLine($"{args.initiator?.Name} применил {Name} на {args.target?.Name}  ");
            Context.QueueManager.SkipPlayer(args.target);
        }
    }
}
