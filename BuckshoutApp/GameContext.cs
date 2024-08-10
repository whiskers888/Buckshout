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
            ID = Random.Next(10000000, 99999999);
            RifleManager = new RifleManager(this);

            ItemBox = new ItemBox();

        }
        public Random Random => new Random();
        public int ID;
        public int Round { get; set; }
        public PlayerManager PlayerManager { get; set; }

        public RifleManager RifleManager { get; set; }
        public ItemBox ItemBox { get; set; } // Здесь будет одна коробка на всех, достают по очереди
        public Mode Mode { get; set; }
        public int Queue { get; set; }

        public void StartGame(Player[] playersId, int mode)
        {
            Round += 1;
            foreach (var player in playersId)
            {
                PlayerManager.AddPlayer(player.UUID, player.Name);
            }
            // Выбираем случайного игрока для очереди
            Queue = PlayerManager.Players[Random.Next(0, PlayerManager.Players.Length)].UUID;

            Mode = (Mode)mode;
        }
        public void StartRound()
        {
            Round += 1;
            Patron[] Patrons = RifleManager.CreatePatron();

        }
    }
}
