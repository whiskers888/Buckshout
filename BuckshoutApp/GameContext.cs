using BuckshoutApp.Manager;
using BuckshoutApp.Objects;
using BuckshoutApp.Objects.rifle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp
{
    public enum Mode
    {
        Default = 0,
        Pro = 1
    }
    public class GameContext
    {
        public GameContext()
        {
            UUID = Guid.NewGuid().ToString();

            ItemBox = new ItemBox();

        }
        public Random Random => new Random();
        public string UUID;
        public int Round { get; set; }

        public PlayerManager PlayerManager { get; set; }
        public QueueManager QueueManager { get; set; }
        public RifleManager RifleManager { get; set; }
        public EventManager EventManager { get; set; }

        public ItemBox ItemBox { get; set; } // Здесь будет одна коробка на всех, достают по очереди
        public Mode Mode { get; set; }

        public void StartGame(Player[] playersId, int mode)
        {
            QueueManager = new QueueManager(this);
            EventManager = new EventManager(this);
            PlayerManager = new PlayerManager(this);
            RifleManager = new RifleManager(this);

            Round += 1;
            foreach (var player in playersId)
            {
                PlayerManager.AddPlayer(player.UUID, player.Name);
            }
            // Выбираем случайного игрока для очереди
            QueueManager.Queue.Shuffle();

            Mode = (Mode)mode;
        }
        public void StartRound()
        {
            Round += 1;
            Patron[] Patrons = RifleManager.CreatePatron();
            
        }

    }
}
