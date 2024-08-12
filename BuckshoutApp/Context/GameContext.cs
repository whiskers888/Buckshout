﻿using BuckshoutApp.Items;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;

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
            ItemManager = new ItemManager(this);
            Settings = new Settings();
        }


        public Random Random => new Random();
        public string UUID;
        public int Round { get; set; }

        public PlayerManager PlayerManager { get; set; }
        public QueueManager QueueManager { get; set; }
        public RifleManager RifleManager { get; set; }
        public EventManager EventManager { get; set; }

        public Settings Settings { get; set; }
        public ItemManager ItemManager { get; set; }
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
            RifleManager.CreatePatrons();
            /*Player zelya = PlayerManager.Players.First(it => it != QueueManager.Current);
            Console.WriteLine($"{zelya.Name} на самом деле зеля  ");

            Player shabloebla = PlayerManager.Players.First(it => it != zelya);
            Console.WriteLine($"{shabloebla.Name} на самом деле саня  ");

            RifleManager.Shoot(zelya);

            Item item = zelya.Inventory.First(it => it.Name == "Наручники");
            var a = new EventData() { target = shabloebla, initiator = zelya };
            item.Use(a);

            Item item1 = shabloebla.Inventory.First(it => it.Name == "Печать \"Переделать\"");
            var a1 = new EventData() { target = zelya, initiator = shabloebla };
            item1.Use(a1);

            Item item2 = zelya.Inventory.First(it => it.Name == "Печать \"Переделать\"");
            var a2 = new EventData() { target = shabloebla, initiator = zelya };
            item2.Use(a2);*/
        }


    }
}
