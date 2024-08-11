
using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Handcuffs : Item
    {
        public Handcuffs(GameContext context) : base(context) { }

        public override string Name => "Наручники";
        public override string Description => "Противник пропускает свой следующий ход";
        public override TargetType TargetType => TargetType.Other;
        public override TargetTeam TargetTeam => TargetTeam.Enemy;
        public override bool IsStealable => true;

        public override void Effect(UseItemModel args)
        {
            Console.WriteLine($"{args.current.Name} применил {Name} на {args.target.Name}  ");
            Context.QueueManager.SkipPlayer(args.target);
            
        }
    }
}
