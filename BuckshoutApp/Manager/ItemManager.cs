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
            Items = new List<Item>();
            Queue = new List<Item>();
            History = new List<Item>();

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
            Context.Settings.RefAvaliableItems.ForEach(item =>
            {
                for (var i = 0; i < Context.PlayerManager.Players.Count * Context.Settings.ITEMS_PER_PLAYER_COEF; i++)
                {
                    // TODO: ТУТ МОЖЕТ БЫТЬ ПИЗДА
                    Items.Add((Item)Activator.CreateInstance(item, new object[] { Context }));
                }
            });
            Items.Shuffle();
            return Items;
        }
        public Item GetLastAfter(Item item)
        {
            if (item is not null)
                return Queue.Last(it => it != item);
            return Queue[Queue.Count - 1];
        }
    }
}
