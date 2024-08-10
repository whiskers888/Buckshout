namespace BuckshoutApp.Items
{
    public class Handcuffs : Item
    {
        public GameContext Context { get; }
        
        public Handcuffs(GameContext context)
        {
            Context = context;
        }

        public string Id => Guid.NewGuid().ToString();
        public string Name => "Наручники";
        public string Description => "Противник пропускает свой следующий ход";
        public Action Action => Action.HandCuffs;
        public TargetType TargetType => TargetType.Other;
        public TargetTeam TargetTeam => TargetTeam.Enemy;
        public bool IsStealable => true;
        public override void Effect(UseItemModel args)
        {
            Context.QueueManager.SkipPlayer(args.target);
            
        }
    }
}
