
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
        public object[]? specialArgs { get; set; }
    }
    public abstract class Item
    {
        public GameContext Context { get; }
        public string Id => Guid.NewGuid().ToString();
        public string Name { get; }
        public string Description { get; }
        public bool IsStealable { get; }
        public TargetType TargetType { get; }
        public TargetTeam TargetTeam { get; }
        public Action Action { get;}

        public abstract void Effect(UseItemModel args);
        public void Use(UseItemModel args) 
        {
            Console.WriteLine("Начало выполнения программы");

            Timer timer = new Timer(async state =>
            {
                Console.WriteLine("Таймер сработал!");
                Effect(args);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(6));

           // тут должен быть eventmanager который будет смотреть отменили ли карту и применять строчку timer.Change
            // Остановка таймера
            timer.Change(Timeout.Infinite, Timeout.Infinite);

            Console.WriteLine("Конец выполнения программы");
        }


    }
}
