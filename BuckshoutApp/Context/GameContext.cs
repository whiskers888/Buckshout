using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Manager.Rifle;

namespace BuckshoutApp.Context
{
    public enum GameStatus
    {
        PREPARING,
        IN_PROGRESS,
        PAUSED,
        FINISHED,
    }

    public enum Mode
    {
        Default = 0,
        Pro = 1
    }

    public class GameContext
    {
        public GameContext()
        {
            Id = Guid.NewGuid().ToString();

            PlayerManager = new PlayerManager(this);
            EventManager = new EventManager(this);
            Rifle = new Rifle(this);
            ItemManager = new ItemManager(this);
            ModifierManager = new ModifierManager(this);
            Settings = new Settings();

        }


        public Random Random => new();
        public string Id;
        public int Round { get; set; }

        public PlayerManager PlayerManager { get; set; }
        public QueueManager QueueManager { get; set; }
        public Rifle Rifle { get; set; }
        public EventManager EventManager { get; set; }
        public ModifierManager ModifierManager { get; set; }

        public Settings Settings { get; set; }
        public ItemManager ItemManager { get; set; }
        public Mode Mode { get; set; }

        public GameStatus Status { get; set; } = GameStatus.PREPARING;


        public void StartGame(Mode mode)
        {
            QueueManager = new QueueManager(this);
            QueueManager.Queue.Shuffle();
            Mode = mode;
            Status = GameStatus.IN_PROGRESS;
        }
        public void StartRound()
        {
            EventManager.Trigger(Event.ROUND_STARTED, new Items.EventData
            {
                special = new Dictionary<string, object>
                {
                    { "ROUND", Round }
                }
            });
            Round += 1;
            PlayerManager.Players.ForEach(p => p.ClearModifiers());
            Rifle.LoadRifle();
            ItemManager.FillBox();

            ItemManager.GiveItems();
            QueueManager.Next();
            /*Player zelya = PlayerManager.Players.First(it => it != QueueManager.Current);
            Console.WriteLine($"{zelya.Name} на самом деле зеля  ");

            Player shabloebla = PlayerManager.Players.First(it => it != zelya);
            Console.WriteLine($"{shabloebla.Name} на самом деле саня");*/
            /*RifleManager.Shoot(zelya);*/

            /*Item item = zelya.Inventory.First(it => it.Name == "Наручники");
            var a = new EventData() { target = shabloebla, initiator = zelya };
            item.Use(a);

            Item item1 = shabloebla.Inventory.First(it => it.Name == "Печать \"Переделать\"");
            var a1 = new EventData() { target = zelya, initiator = shabloebla };
            item1.Use(a1);

            Item item2 = zelya.Inventory.First(it => it.Name == "Печать \"Переделать\"");
            var a2 = new EventData() { target = shabloebla, initiator = zelya };
            item2.Use(a2);*/
        }

        public void FinishGame()
        {
            Status = GameStatus.FINISHED;
        }
    }
}
