

using BuckshoutApp.Items;

namespace BuckshoutApp.Objects
{
    public class Player
    {
        private GameContext GameContext;
        public Player(int uuid, string? name)
        {
            Inventory = new List<IItem>();
            UUID = uuid;
            Name = name;
            Inventory = new List<IItem>();
            Health = SetHealth();
        }
        public int UUID { get; set; }
        public string? Name { get; set; }
        public List<IItem>? Inventory { get; set; }
        public int Health { get; set; }

        private int SetHealth()
        {

            GameContext.Game.Mode
        }
    }
}
