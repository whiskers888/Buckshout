using BuckshoutApp.Context;
using BuckshoutApp.Items;

namespace BuckshoutApp.Manager
{
    public enum ChangeHealthType
    {
        Damage = 0,
        Heal = 1
    }
    public class PlayerManager(GameContext context)
    {
        private readonly GameContext Context = context;

        public List<Player> Players { get; set; } = [];

        public Player Get(string id) => Players.First(it => it.UUID == id);
        public void AddPlayer(string id, string name)
        {
            Players.Add(new Player(Context, id, name));
        }
        public void DeletePlayer(string id)
        {
            Players.Remove(Players.First(it => it.UUID == id));
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
                Context.EventManager.Trigger(Events.Event.MESSAGE_RECEIVED, new EventData()
                {
                    target = this,
                    special = new Dictionary<string, object>() { { "MESSAGE", $"Error: Player(). No has mode. Don't set health. mode:{context.Mode},uuid:{uuid}" } }
                });
        }
        public void UseItem(string itemId, string targetId)
        {
            Player target = Context.PlayerManager.Get(targetId);
            EventData e = new() { initiator = this, target = target };
            Item item = Inventory.First(it => it.UUID == itemId);
            item.Use(e);
            Inventory.Remove(item);

        }

        public void ChangeHealth(ChangeHealthType direction, int count, Player initiator)
        {
            EventData e = new()
            {
                initiator = initiator,
                target = this,
                special = new Dictionary<string, object>() { { "VALUE", count } }
            };
            switch (direction)
            {
                case ChangeHealthType.Heal:
                    Health += count;
                    Context.EventManager.Trigger(Events.Event.HEALTH_RESTORED, e);
                    break;
                case ChangeHealthType.Damage:
                    Health -= count;
                    Context.EventManager.Trigger(Events.Event.DAMAGE_TAKEN, e);
                    break;
            }
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
            Context.EventManager.Trigger(Events.Event.ITEM_RECEIVED, new EventData()
            {
                target = this,
                initiator = this,
                special = new Dictionary<string, object>() { { "ITEM", item } }
            });
        }
        public string UUID { get; set; }
        public string Name { get; set; }
        public List<Item> Inventory { get; set; }
        public int Health { get; set; }

    }
}
