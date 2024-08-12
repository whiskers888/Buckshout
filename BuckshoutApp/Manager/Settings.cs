using BuckshoutApp.Items;

namespace BuckshoutApp.Manager
{
    public class Settings
    {
        public Settings()
        {
            RefAvaliableItems = new List<Type>();

            // Описываем ссылки на классы
            Type adrenaline = typeof(Adrenaline);
            Type beer = typeof(Beer);
            Type cancel = typeof(Cancel);
            Type hackSaw = typeof(Hacksaw);
            Type handCuffs = typeof(Handcuffs);

            // Добавляем их в массив
            RefAvaliableItems.AddRange([adrenaline, beer, cancel, hackSaw, handCuffs]);

        }
        public int ITEM_CHANNELING_TIME = 3000;
        public int ITEM_CHANNELING_CHECK_INTERVAL = 100;
        public int MAX_TURN_DURATION = 100;
        public int ITEMS_PER_PLAYER_COEF = 10;


        public List<Type> RefAvaliableItems { get; set; }
    }
}
