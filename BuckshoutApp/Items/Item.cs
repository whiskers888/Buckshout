
using BuckshoutApp.Context;
using BuckshoutApp.Manager;

namespace BuckshoutApp.Items
{

    public enum TargetType
    {
        Self = 0,
        Any = 1,
        Other = 2,
    }
    public enum TargetTeam
    {
        Friendly = 0,
        Enemy = 1,
        All = 2
    }
    public class UseItemModel
    {
        public int itemId { get; set; }
        public Player? current { get; set; }
        public Player? target { get; set; }
        public Dictionary<string,object>? specialArgs { get; set; } = new Dictionary<string,object>();
    }
    public abstract class Item
    {

        public Item(GameContext context) 
        {
            Context = context;
        }
        public GameContext Context { get; }
        public string Id => Guid.NewGuid().ToString();
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsStealable { get; }
        public abstract TargetType TargetType { get; }
        public abstract TargetTeam TargetTeam { get; }

        public abstract void Effect(UseItemModel args);

        public virtual void Use(UseItemModel args) 
        {

            Console.WriteLine($"{args.current.Name} пытается применить {Name} на {args.target.Name}  ");

            var timer = Timer.SetTimeout(() =>
            {
                Console.WriteLine("Таймер сработал!");
                Effect(args);
            }, 10000);

            Context.EventManager.Subcribe(new EventModel(Event.OnCancelItem,Context.EventManager.CancelId), () =>
            {

                Console.WriteLine($"{args.current.Name} отменил {Name} на {args.target.Name}  ");
                timer.Dispose();
            });
        }


    }
}
