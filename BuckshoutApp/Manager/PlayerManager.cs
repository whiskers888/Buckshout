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

        private List<Player> players { get; set; } = new List<Player>();

        public Player[] Players => players.ToArray();

        public void AddPlayer(int id, string name)
        {
            players.Add(new Player(Context, id, name));
        }
        public void DeletePlayer(int id)
        {
            players.Remove(players.FirstOrDefault(it => it.UUID == id));
        }
        public void SetHealth(DirectionHealth direction, int count, int userId)
        {
            if (direction == DirectionHealth.Up)
                players.FirstOrDefault(it => it.UUID == userId).Health += count;
            else if (direction == DirectionHealth.Down)
                players.FirstOrDefault(it => it.UUID == userId).Health -= count;
        }
    }

    public class Player
    {
        private GameContext Context { get; set; }
        public Player(GameContext context, int uuid, string? name)
        {
            Context = context;

            UUID = uuid;
            Name = name;
            Inventory = new List<IItem>();
            if (Context.Mode == Mode.Default)
                Health = 4;
            else if (Context.Mode == Mode.Pro)
                Health = 2;
            else
                throw new Exception($"Error: Player.SetHealth(). No has mode. Don't set health. mode:{context.Mode},uuid:{uuid}");
        }
        public int UUID { get; set; }
        public string? Name { get; set; }
        public List<IItem>? Inventory { get; set; }
        public int Health { get; set; }

    }
}
