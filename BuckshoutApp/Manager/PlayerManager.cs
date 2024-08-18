using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Modifiers;

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

        public Player Get(string id) => Players.First(it => it.Id == id);
        public void AddPlayer(string id, string name)
        {
            Player player = new Player(Context, id, name);
            Context.EventManager.Trigger(Events.Event.PLAYER_CONNECTED, new EventData() { target = player, initiator = player });
            Players.Add(player);
        }
        public void DeletePlayer(string id)
        {
            Player player = Players.First(it => it.Id == id);
            Context.EventManager.Trigger(Events.Event.PLAYER_DISCONNECTED, new EventData() { target = player, initiator = player });
            Players.Remove(player);
        }
    }

    public class Player
    {
        private GameContext Context { get; set; }
        public Player(GameContext context, string id, string name)
        {
            Context = context;

            Id = id;
            Name = name;
            Team = id; // TODO: сделать реализацию команд
            Avatar = Context.Random.Next(2, 50);
            Inventory = [/*new Cancel(Context), new Handcuffs(context), new Beer(context), new Hacksaw(context), new Phone(context), new Adrenaline(context)*/];
            Modifiers = [];
            if (Context.Mode == Mode.Default)
                Health = 4;
            else if (Context.Mode == Mode.Pro)
                Health = 2;
            else
                Context.EventManager.Trigger(Events.Event.MESSAGE_RECEIVED, new EventData()
                {
                    target = this,
                    special = new Dictionary<string, object>() { { "MESSAGE", $"Error: Player(). No has mode. Don't set health. mode:{context.Mode},uuid:{id}" } }
                });
        }
        public void UseItem(string itemId, string targetId, string? targetItemId = null)
        {
            Player target = Context.PlayerManager.Get(targetId);
            Item item = Inventory.First(it => it.Id == itemId);
            EventData e = new() { initiator = this, target = target };
            if (targetItemId != null && item.Behavior.Contains(ItemBehavior.UNIT_TARGET) && item.TargetType == TargetType.ITEM)
            {
                e.special["TARGET_ITEM"] = target.Inventory.First(it => it.Id == targetItemId);
            }
            var used = item.Use(e);
            if (used)
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
            e.special.Clear();
            if (Health <= 0)
            {
                Context.EventManager.Trigger(Events.Event.PLAYER_LOST, e);
                Context.ModifierManager.Modifiers[ModifierKey.PLAYER_DEAD].Apply(this);
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

        public void RemoveItem(Item item)
        {
            if (!Inventory.Contains(item)) return;
            Inventory.Remove(item);
            Context.EventManager.Trigger(Events.Event.ITEM_REMOVED, new EventData()
            {
                target = this,
                special = new Dictionary<string, object>() { { "ITEM", item } }
            });
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            Context.EventManager.Trigger(Events.Event.MODIFIER_APPLIED, new EventData()
            {
                target = this,
                special = { { "MODIFIER", modifier } }
            });
        }
        public void RemoveModifier(Modifier modifier)
        {
            Modifiers.Remove(modifier);
            Context.EventManager.Trigger(Events.Event.MODIFIER_REMOVED, new EventData()
            {
                target = this,
                special = { { "MODIFIER", modifier } }
            });
        }
        public void ClearModifiers()
        {
            Modifiers.Clear();
        }

        public bool Is(ModifierState state)
        {
            foreach (var modifier in Modifiers)
            {
                if (modifier.State.Contains(state)) return true;
            }
            return false;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public int Health { get; set; }
        public string Team { get; set; }
        public int Avatar { get; set; }
    }
}
