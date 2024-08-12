using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Manager
{
    public class ItemManager
    {
        private GameContext _gameContext { get; set; }
        public ItemManager(GameContext context)
        {
            _gameContext = context;
            Items = new List<Item>();
            Queue = new List<Item>();
            History = new List<Item>();

            _gameContext.EventManager.Subcribe(Event.ITEM_USED, (e) =>
            {
                History.Add((Item)e.special["ITEM"]);
                Queue.Add((Item)e.special["ITEM"]);
            });
            _gameContext.EventManager.Subcribe(Event.ITEM_EFFECTED, ClearQueue);
            _gameContext.EventManager.Subcribe(Event.ITEM_CANCELED, ClearQueue);
        }
        public void ClearQueue(EventData e) => Queue.Remove((Item)e.special["ITEM"]);
        private List<Item> Items { get; set; }
        private List<Item> Queue { get; set; }
        private List<Item> History { get; set; }
        public Item GetLastAfter(Item item)
        {
            if (item is not null)
                return Queue.Last(it => it != item);
            return Queue[Queue.Count - 1];
        }
    }
}
