using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Objects;

namespace BuckshoutApp.Manager
{
    public enum DirectionHealth
    {
        Up = 0,
        Down = 1
    }
    public class PlayerManager
    {
        private GameContext Context;

        public PlayerManager(GameContext context)
        {
            Context = context;
            Players = new List<Player>();
        }

        public List<Player> Players { get; set; }

        public Player Get(string id) => Players.First(it => it.UUID == id);
        public void AddPlayer(string id, string name)
        {
            Players.Add(new Player(Context, id, name));
        }
        public void DeletePlayer(string id)
        {
            Players.Remove(Players.First(it => it.UUID == id));
        }
        public void SetHealth(DirectionHealth direction, int count,Player player)
        {
            if (direction == DirectionHealth.Up)
                player.Health += count;
            else if (direction == DirectionHealth.Down)
                player.Health -= count;
        }
    }

    public class Player
    {
        private GameContext Context { get; set; }
        public Player(GameContext context, string uuid, string name)
        {
            Context = context;

            UUID = uuid;
            Name = name;
            Inventory = [new Cancel(Context), new Handcuffs(context)];
            if (Context.Mode == Mode.Default)
                Health = 4;
            else if (Context.Mode == Mode.Pro)
                Health = 2;
            else
                throw new Exception($"Error: Player.SetHealth(). No has mode. Don't set health. mode:{context.Mode},uuid:{uuid}");
        }
        public void UseItem(string itemId, string targetId)
        {
            Player target = Context.PlayerManager.Get(targetId);
            EventData e = new EventData() { initiator = this, target = target};
            Item item = Inventory.First(it => it.UUID == itemId);
            item.Use(e);
            Inventory.Remove(item);

        }
        public string UUID { get; set; }
        public string Name { get; set; }
        public List<Item> Inventory { get; set; }
        public int Health { get; set; }

    }
}
