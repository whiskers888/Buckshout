using BuckshoutApp.Items;

namespace BuckshoutApp.Manager
{
    public class Settings
    {
        public Settings()
        {
            RefAvaliableItems = [];

            // Описываем ссылки на классы
            Type adrenaline = typeof(Adrenaline);
            Type beer = typeof(Beer);
            Type stamp = typeof(Stamp);
            Type hacksaw = typeof(Hacksaw);
            Type phone = typeof(Phone);
            Type handcuffs = typeof(Handcuffs);

            Type cigarettes = typeof(Cigarettes);
            Type medicine = typeof(ExpiredMedicine);
            Type magnifier = typeof(Magnifier);
            Type invertor = typeof(Invertor);

            Type stopwatch = typeof(Stopwatch);
            Type cross = typeof(ChurchCross);

            Type useless = typeof(Useless);
            Type chain = typeof(Chain);
            Type bloodpack = typeof(BloodPack);
            Type mirror = typeof(Mirror);
            Type dollar = typeof(Dollar);

            Type hat = typeof(Hat);

            // Добавляем их в массив
            RefAvaliableItems.AddRange([
                adrenaline,
                beer,
                bloodpack,
                chain,
                cross,
                cigarettes,
                dollar,
                hacksaw,
                handcuffs,
                hat,
                invertor,
                mirror,
                magnifier,
                medicine,
                phone,
                stamp,
                stopwatch,
                useless
            ]);
        }
        public int ITEM_CHANNELING_TIME { get; set; } = 5000; // Время в течение которого можно отменить итем при помощи Печати
        public int ITEM_CHANNELING_CHECK_INTERVAL { get; set; } = 100; // 

        public int MAX_TURN_DURATION { get; set; } = 120000;

        public int MAX_PLAYER_HEALTH { get; set; } = 8;
        public int INIT_PLAYER_HEALTH { get; set; } = 5;

        public int MAX_PATRONS_IN_RIFLE { get; set; } = 8;

        public int MAX_INVENTORY_SLOTS { get; set; } = 8; // кол-во слотов
        public int ITEMS_PER_PLAYER_COEF { get; set; } = 10; // в стэк будет замешен итем каждого типа * кол-во игроков * на этот коэф

        public int ITEMS_PER_ROUND { get; set; } = 2; // каждый раунд выдается столько итемов
        public int ITEMS_PER_ROUND_INCREMENT { get; set; } = 1; // каждый раунд кол-во инемов за выдачу увеличивается

        public int FATIGUE_ROUND { get; set; } = 6; // начиная с этого раунда у игрока появляется усталость. предметы больше не выдаются
        public int FATIGUE_ITEMS_TO_LOSE { get; set; } = 1; // он начинает терять предметы
        public int FATIGUE_DAMAGE_PER_ITEM { get; set; } = 1; // и получает урон, если их не осталось

        public int RECCONECTION_TIME { get; set; } = 12 * 100 * 100;


        internal List<Type> RefAvaliableItems { get; set; }
    }
}
