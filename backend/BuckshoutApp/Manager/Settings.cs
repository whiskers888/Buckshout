using BuckshoutApp.Items;
using BuckshoutApp.Items.Trap;

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
            Type hacksaw = typeof(Hacksaw);
            Type phone = typeof(Phone);
            Type handcuffs = typeof(Handcuffs);
            Type cigarettes = typeof(Cigarettes);
            Type medicine = typeof(ExpiredMedicine);
            Type magnifier = typeof(Magnifier);
            Type invertor = typeof(Invertor);

            Type stopwatch = typeof(Stopwatch);
            Type cross = typeof(ChurchCross);
            Type stamp = typeof(Stamp);
            Type useless = typeof(Useless);
            Type chain = typeof(Chain);
            Type dollar = typeof(Dollar);
            Type hat = typeof(Hat);
            Type clay = typeof(Clay);
            Type blindfold = typeof(Blindfold);
            Type milk = typeof(Milk);
            Type silencer = typeof(Silencer);

            Type bloodpack = typeof(BloodPack);
            Type mirror = typeof(Mirror);
            Type grenade = typeof(Grenade);
            Type defibrillator = typeof(Defibrillator);

            // Добавляем их в массив
            RefAvaliableItems.AddRange([
                /*adrenaline,
                beer,
                bloodpack,
                blindfold,
                chain,
                cigarettes,
                clay,
                cross,
                defibrillator,
                dollar,
                grenade,
                hacksaw,
                handcuffs,
                hat,
                invertor,
                magnifier,
                medicine,
                milk,
                mirror,
                phone,
                silencer,
                stamp,
                stopwatch,
                useless*/

                handcuffs,
                stopwatch
            ]);
        }
        public int SHOW_ACTION_TIME { get; set; } = 3000; // !!НЕ ДЕЛАТЬ >= ITEM_CHANNELING_TIME!! Время, в течение которого будут отображаться изменения (мигать хп бар, итемы и т.п) просто визуал
        public int ITEM_CHANNELING_TIME { get; set; } = 5000; // Время, в течение которого можно отменить итем при помощи Печати
        public int ITEM_CHANNELING_CHECK_INTERVAL { get; set; } = 100; // 

        public int MAX_TURN_DURATION { get; set; } = 60000;

        public int MAX_PLAYER_HEALTH { get; set; } = 8;
        public int INIT_PLAYER_HEALTH { get; set; } = 5;

        public int ALWAYS_HIDDEN_PLAYER_HEALTH { get; set; } = 2; // для большего интереса - часть хп можно скрыть

        public int MAX_PATRONS_IN_RIFLE { get; set; } = 8;

        public int MAX_INVENTORY_SLOTS { get; set; } = 8; // кол-во слотов
        public int ITEMS_PER_PLAYER_COEF { get; set; } = 10; // в стэк будет замешен итем каждого типа * кол-во игроков * на этот коэф

        public int ITEMS_PER_ROUND { get; set; } = 8; // каждый раунд выдается столько итемов
        public int ITEMS_PER_ROUND_INCREMENT { get; set; } = 1; // каждый раунд кол-во инемов за выдачу увеличивается
        public int MAX_ITEMS_PER_ROUND { get; set; } = 4; // но не больше этого значения

        public int FATIGUE_ROUND { get; set; } = 8; // начиная с этого раунда у игрока появляется усталость.
        public int FATIGUE_ITEMS_TO_LOSE { get; set; } = 1; // он начинает терять предметы
        public int FATIGUE_ITEMS_TO_LOSE_INCREMENT { get; set; } = 1; // с каждым раундом все больше
        public int FATIGUE_DAMAGE_PER_ITEM { get; set; } = 1; // и получает урон, если их не осталось

        public int RECCONECTION_TIME { get; set; } = 1000 * 60 * 5; // 5 минут


        internal List<Type> RefAvaliableItems { get; set; }
    }
}
