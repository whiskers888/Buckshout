using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Manager
{
    public enum ChangeHealthType
    {
        Damage = 0,
        Heal = 1
    }
    public enum PlayerStatus
    {
        CONNECTED,
        DISCONECTED,
        LEAVE,
    }
    public class PlayerManager(GameContext context)
    {
        private readonly GameContext Context = context;

        public List<Player> Players { get; set; } = [];

        public List<Player> AlivePlayers => Players.Where(it => !it.Is(ModifierState.PLAYER_DEAD)).ToList();

        public Player Get(string id) => Players.First(it => it.Id == id);
        public void AddPlayer(string id, string name)
        {
            Player player = new Player(Context, id, name);
            Context.EventManager.Trigger(Events.Event.PLAYER_CONNECTED, new EventData() { Target = player, Initiator = player });
            Players.Add(player);
        }
        public void DeletePlayer(string id)
        {
            Player player = Players.First(it => it.Id == id);
            Context.EventManager.Trigger(Events.Event.PLAYER_DISCONNECTED, new EventData() { Target = player, Initiator = player });
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
            Team = name;
            Avatar = Context.Random.Next(2, 50);
            Inventory = [];
            Modifiers = [];
            Status = PlayerStatus.CONNECTED;
            if (Context.Mode == Mode.Default)
                Health = Context.Settings.INIT_PLAYER_HEALTH;
            else if (Context.Mode == Mode.Pro)
                Health = 2;
            else
                Context.EventManager.Trigger(Event.MESSAGE, new EventData()
                {
                    Target = this,
                    Special = new Dictionary<string, object>() { { "MESSAGE", $"Error: Player(). No has mode. Don't set health. mode:{context.Mode},uuid:{id}" } }
                });
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public int Health { get; set; }
        public string Team { get; set; }
        public int Avatar { get; set; }
        public PlayerStatus Status { get; set; } = PlayerStatus.CONNECTED;

        public void UseItem(string itemId, string targetId, string? targetItemId = null)
        {
            Player target = Context.PlayerManager.Get(targetId);
            Item item = Inventory.First(it => it.Id == itemId);
            EventData e = new() { Initiator = this, Target = target };
            if (targetItemId != null && item.Behavior.Contains(ItemBehavior.UNIT_TARGET) && item.TargetType == ItemTargetType.ITEM)
            {
                e.Special["TARGET_ITEM"] = target.Inventory.First(it => it.Id == targetItemId);
            }
            var used = item.Use(e);
            if (used)
                RemoveItem(item);
        }

        public void ChangeHealth(ChangeHealthType direction, int count, Player initiator, string type = "DEFAULT")
        {
            Console.WriteLine($"{initiator.Name} - {this.Name} выдал {direction} с кол-во {count} Тип {type}");
            if (this.Is(ModifierState.PLAYER_DEAD)) return;
            EventData e = new()
            {
                Initiator = initiator,
                Target = this,
            };
            e.Special.Add("VALUE", count);
            e.Special.Add("TYPE", type);
            if (direction == ChangeHealthType.Damage)
                Context.EventManager.Trigger(Event.BEFORE_DAMAGE_TAKE, e);
            if (e.Prevent) return;
            switch (direction)
            {
                case ChangeHealthType.Heal:
                    Health += count;
                    Context.EventManager.Trigger(Event.HEALTH_RESTORED, e);
                    break;
                case ChangeHealthType.Damage:
                    Health -= count;
                    Context.EventManager.Trigger(Event.DAMAGE_TAKEN, e);
                    break;
            }
            e.Special.Clear();
            if (Health <= 0)
            {
                Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DEAD).Apply(this);
                Context.EventManager.Trigger(Event.PLAYER_LOST, e);
                if (type != "RIFLE")
                    Context.QueueManager.Next();
            }
        }

        public void AddItem(Item item)
        {
            if (Inventory.Count >= Context.Settings.MAX_INVENTORY_SLOTS) return;
            Inventory.Add(item);
            Context.EventManager.Trigger(Event.ITEM_RECEIVED, new EventData()
            {
                Target = this,
                Initiator = this,
                Special = new Dictionary<string, object>() { { "ITEM", item } }
            });
        }
        public void RemoveItem(Item item)
        {
            if (!Inventory.Contains(item)) return;
            Inventory.Remove(item);
            Context.EventManager.Trigger(Event.ITEM_REMOVED, new EventData()
            {
                Target = this,
                Special = new Dictionary<string, object>() { { "ITEM", item } }
            });
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            Context.EventManager.Trigger(Event.MODIFIER_APPLIED, new EventData()
            {
                Target = this,
                Special = { { "MODIFIER", modifier } }
            });
        }
        public void RemoveModifier(Modifier modifier, bool forced = true, bool round = false, bool item = false)
        {
            if (!forced && ((round && !modifier.IsPurgableByRound) || (item && !modifier.IsPurgableByItem))) return;

            modifier.SetRemoved();
            if (Modifiers.Remove(modifier))
                Context.EventManager.Trigger(Event.MODIFIER_REMOVED, new EventData()
                {
                    Target = this,
                    Special = { { "MODIFIER", modifier } }
                });
        }
        public void ClearModifiers()
        {
            Modifiers.ToList().ForEach(m =>
            {
                RemoveModifier(m, false, true, false);
            });
        }

        public Modifier GetModifier(ModifierState state) => Modifiers.FirstOrDefault(it => it.State.Contains(state));
        public bool Is(ModifierState state)
        {
            foreach (var modifier in Modifiers)
            {
                if (modifier.State.Contains(state)) return true;
            }
            return false;
        }
    }
}
