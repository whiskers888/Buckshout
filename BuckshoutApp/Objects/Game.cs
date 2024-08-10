
using BuckshoutApp.Objects.rifle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp.Objects
{
    public enum Mode
    {
        Default = 0,
        Pro = 1
    }

    public class Game
    {

        public Game(int mode)
        {
            Round = 0;
            ID = 1;
            Rifle = new Rifle();

            ItemBox = new ItemBox();

            Mode = mode;
        }

        public int ID;
        public int Round { get; set; }
        private List<Player>? Players { get; set; }

        public Rifle Rifle { get; set; }
        public ItemBox ItemBox { get; set; } // Здесь будет одна коробка на всех, достают по очереди
        public Mode Mode { get; set; }
        public int Queue { get; set; }


        public void AddPlayer(int id) { }
    }

}
