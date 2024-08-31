using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Manager
{
    public class ItemManager
    {
        private GameContext Context { get; set; }
        public ItemManager(GameContext context)
        {
            Context = context;
            Items = [];
            Queue = [];
            History = [];

            Context.EventManager.Subscribe(Event.ITEM_USED, (e) =>
            {
                History.Add((Item)e.Special["ITEM"]);
                Queue.Add((Item)e.Special["ITEM"]);
            });
            Context.EventManager.Subscribe(Event.ITEM_EFFECTED, ClearQueue);
            Context.EventManager.Subscribe(Event.ITEM_CANCELED, ClearQueue);
        }

        public void ClearQueue(EventData e) => Queue.Remove((Item)e.Special["ITEM"]);
        private List<Item> Items { get; set; }
        private List<Item> Queue { get; set; }
        private List<Item> History { get; set; }

        public List<Item> FillBox()
        {
            Items.Clear();
            Context.Settings.RefAvaliableItems.ForEach(item =>
            {
                for (var i = 0; i < Context.PlayerManager.Players.Count * Context.Settings.ITEMS_PER_PLAYER_COEF; i++)
                {
                    Items.Add((Item)Activator.CreateInstance(item, [Context])!);
                }
            });
            Items.Shuffle();
            return Items;
        }

        public Item NextItem()
        {
            if (Items.Count <= 0)
            {
                FillBox();
            }
            return Items.Pop();
        }

        public void GiveItems()
        {
            if (Context.Round > Context.Settings.FATIGUE_ROUND)
            {
                Context.EventManager.Trigger(Event.PLAY_SOUND, new EventData()
                {
                    Special = new Dictionary<string, object> { { "SOUND", "fatigue_ravens" } }
                });
                Context.PlayerManager.AlivePlayers.ForEach(player =>
                {
                    for (var i = 0; i < Context.Settings.FATIGUE_ITEMS_TO_LOSE; i++)
                    {
                        if (player.Inventory.Count == 0)
                        {
                            player.ChangeHealth(ChangeHealthType.Damage, Context.Settings.FATIGUE_DAMAGE_PER_ITEM, player, "FATIGUE");
                        }
                        else
                        {
                            var item = player.Inventory[Context.Random.Next(0, player.Inventory.Count - 1)];
                            player.RemoveItem(item);
                        }
                    }
                });
                Context.Settings.FATIGUE_ITEMS_TO_LOSE += Context.Settings.FATIGUE_ITEMS_TO_LOSE_INCREMENT;
            }

            for (var i = 0; i < Context.Settings.ITEMS_PER_ROUND; i++)
            {
                Context.PlayerManager.AlivePlayers.ForEach(player =>
                {
                    player.AddItem(NextItem());
                });
            }
            if (Context.Settings.ITEMS_PER_ROUND < Context.Settings.MAX_ITEMS_PER_ROUND)
            {
                Context.Settings.ITEMS_PER_ROUND += Context.Settings.ITEMS_PER_ROUND_INCREMENT;
            }
        }
        public Item GetLastAfter(Item item)
        {
            if (item is not null)
                return Queue.Last(it => it != item);
            return Queue[^1]/*Queue[Queue.Count - 1]*/;
        }
    }
}
