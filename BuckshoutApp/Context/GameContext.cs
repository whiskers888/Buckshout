using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Manager.Rifle;
using BuckshoutApp.Modifiers;

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
            if (Status == GameStatus.FINISHED) return;
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
            EventManager.Once(Event.PLAYER_WON, (e) =>
            {
                FinishGame();
            });

            EventManager.Subscribe(Event.PLAYER_LOST, (e) =>
            {
                if (PlayerManager.Players.Where(it => !it.Is(ModifierState.PLAYER_DEAD)).Count() == 1)
                {
                    EventManager.Trigger(Event.PLAYER_WON, new Items.EventData()
                    {
                        target = PlayerManager.Players.FirstOrDefault(it => !it.Is(ModifierState.PLAYER_DEAD))
                    });
                }
            });
        }
        public void FinishGame()
        {
            Status = GameStatus.FINISHED;
        }
    }
}
