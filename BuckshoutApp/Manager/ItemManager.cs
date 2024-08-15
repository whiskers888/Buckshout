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

            Context.EventManager.Subcribe(Event.ITEM_USED, (e) =>
            {
                History.Add((Item)e.special["ITEM"]);
                Queue.Add((Item)e.special["ITEM"]);
            });
            Context.EventManager.Subcribe(Event.ITEM_EFFECTED, ClearQueue);
            Context.EventManager.Subcribe(Event.ITEM_CANCELED, ClearQueue);
        }

        public void ClearQueue(EventData e) => Queue.Remove((Item)e.special["ITEM"]);
        private List<Item> Items { get; set; }
        private List<Item> Queue { get; set; }
        private List<Item> History { get; set; }

        public List<Item> FillBox()
        {
            Items.Clear();
            if (Context.Round > Context.Settings.FATIGUE_ROUND)
            {
                Context.PlayerManager.Players.ForEach(p =>
                {
                    for (var i = 0; i < Context.Settings.FATIGUE_ITEMS_TO_LOSE; i++ )
                    {
                        if (p.Inventory.Count == 0)
                        {
                            p.ChangeHealth(ChangeHealthType.Damage, Context.Settings.FATIGUE_DAMAGE_PER_ITEM, p);
                        }
                        p.Inventory.RemoveAt(Context.Random.Next(0, p.Inventory.Count - 1));
                    }
                });
            }
            Context.Settings.RefAvaliableItems.ForEach(item =>
            {
                for (var i = 0; i < Context.PlayerManager.Players.Count * Context.Settings.ITEMS_PER_PLAYER_COEF; i++)
                {
                    // TODO: ТУТ МОЖЕТ БЫТЬ ПИЗДА
                    Items.Add((Item)Activator.CreateInstance(item, new object[] { Context })!);
                }
            });
            Items.Shuffle();
            return Items;
        }

        public void GiveItems()
        {
            Context.PlayerManager.Players.ForEach(player =>
            {
                for (var i = 0; i < Context.Settings.ITEMS_PER_ROUND; i++)
                {
                    player.AddItem(Items.Pop());
                }
            });
            Context.Settings.ITEMS_PER_ROUND += Context.Settings.ITEMS_PER_ROUND_INCREMENT;
        }
        public Item GetLastAfter(Item item)
        {
            if (item is not null)
                return Queue.Last(it => it != item);
            return Queue[^1]/*Queue[Queue.Count - 1]*/;
        }
    }
}
