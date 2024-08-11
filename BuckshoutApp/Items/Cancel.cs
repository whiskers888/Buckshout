using BuckshoutApp.Context;
using BuckshoutApp.Manager;

namespace BuckshoutApp.Items
{
    public class Cancel : Item
    {
        public Cancel(GameContext context) : base(context) { }

        public override string Name => "Печать \"Переделать\"";

        public override string Description => "Все фигня, переделывай";

        public override bool IsStealable => false;

        public override TargetType TargetType => TargetType.Self;

        public override TargetTeam TargetTeam => TargetTeam.All;

        public override void Effect(UseItemModel args)
        {
            Console.WriteLine($"{args.current.Name} применил {Name} на {args.target.Name}  ");
            Context.EventManager.Trigger(new EventModel(Event.OnCancelItem, (string)args.specialArgs["cancel"]));
            Context.EventManager.ChangeCancelId();
        }

        public override void Use(UseItemModel args)
        {
            Console.WriteLine($"{args.current.Name} пытается применить {Name} на {args.target.Name}  ");
            args.specialArgs.Add("cancel", Context.EventManager.CancelId);
            var timer = Timer.SetTimeout(() =>
            {
                Effect(args);
            }, 6000);

            Context.EventManager.Subcribe(new EventModel(Event.OnCancelItem, Context.EventManager.CancelId), () =>
            {

                Console.WriteLine($"{args.current.Name} отменил {Name} на {args.target.Name}  ");
                timer.Dispose();
            });
        }
    }
}
