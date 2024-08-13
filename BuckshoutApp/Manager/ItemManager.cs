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
            if (Context.Round > Context.Settings.ROUND_INVENTORIES_CLEAR)
            {
                Context.PlayerManager.Players.ForEach(p =>
                {
                    //TODO: Если вдруг игры слишком затяжные и имеется абуз курения, то здесь стоит дамажить при отсутствии итемов
                    // Возможно какая то логика как у предмета наркотика с сигами 
                    p.Inventory.RemoveAt(Context.Random.Next(0, p.Inventory.Count - 1));
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
            if (Context.Round > Context.Settings.ROUND_CHANGE_COUNT_ITEMS)
            {
                Context.Settings.COUNT_ITEMS_GIVE += 1;
            }
            Context.PlayerManager.Players.ForEach(player =>
            {
                for (var i = 0; i < Context.Settings.COUNT_ITEMS_GIVE; i++)
                {
                    player.AddItem(Items.Pop());
                }
            });
        }
        public Item GetLastAfter(Item item)
        {
            if (item is not null)
                return Queue.Last(it => it != item);
            return Queue[^1]/*Queue[Queue.Count - 1]*/;
        }
    }
}
