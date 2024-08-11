using BuckshoutApp.Items;
using BuckshoutApp.Manager;
using BuckshoutApp.Objects;
using BuckshoutApp.Objects.rifle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuckshoutApp.Context
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

            PlayerManager = new PlayerManager(this);
            EventManager = new EventManager(this);
            RifleManager = new RifleManager(this);

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

        public void StartGame(Mode mode)
        {

            QueueManager = new QueueManager(this);
            QueueManager.Queue.Shuffle();
            Mode = mode;
        }
        public void StartRound()
        {
            Round += 1;

            /*Player zelya = PlayerManager.Players.FirstOrDefault(it => it != QueueManager.Current);
            Console.WriteLine($"{zelya.Name} на самом деле зеля  ");

            Player shabloebla = PlayerManager.Players.FirstOrDefault(it => it != zelya);
            Console.WriteLine($"{shabloebla.Name} на самом деле саня  ");

            RifleManager.Shoot(zelya);

            Item item = zelya.Inventory.FirstOrDefault(it => it.Name == "Наручники");
            var a = new UseItemModel() { target = shabloebla, current = zelya};
            item.Use(a);

            Item item1 = shabloebla.Inventory.FirstOrDefault(it => it.Name == "Печать \"Переделать\"");
            var a1 = new UseItemModel() { target = zelya, current = shabloebla };
            item1.Use(a1);

            Item item2 = zelya.Inventory.FirstOrDefault(it => it.Name == "Печать \"Переделать\"");
            var a2 = new UseItemModel() { target = shabloebla, current = zelya };
            item2.Use(a2);*/
        }


    }
}
