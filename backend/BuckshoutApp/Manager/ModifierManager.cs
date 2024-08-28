using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Manager
{
    public class ModifierManager
    {
        public ModifierManager(GameContext context)
        {
            Context = context;
        }
        public GameContext Context { get; }
        public Modifier CreateModifier(ModifierKey key)
        {
            return key switch
            {
                // Эффекты игрока
                ModifierKey.PLAYER_DEAD => new(Context)
                {
                    Name = "Мертв!",
                    Description = "Больше никакие дефибрилляторы и переливания крови ему не помогут...",
                    Duration = -1,
                    State = [ModifierState.PLAYER_DEAD],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "emoticon-dead-outline",
                    IsBuff = false,

                },
                ModifierKey.PLAYER_HANDCUFFS => new(Context)
                {
                    Name = "Оцепенение",
                    Description = "Игрок находится в наручниках, поэтому пропускает свой следующий ход.",
                    Duration = 1,
                    State = [ModifierState.PLAYER_STUNNED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "handcuffs",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_TRAP => throw new NotImplementedException(),
                ModifierKey.PLAYER_STOPWATCH => new(Context)
                {
                    Name = "Дедлайн",
                    /*StackCount = 1,*/
                    Description = "Время хода этого игрока сокращено в 2 раза.",
                    Duration = 0,
                    State = [ModifierState.PLAYER_TURN_TIME_LIMITED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "timer",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_CHURCH_CROSS => new(Context)
                {
                    Name = "Вера",
                    Description = "Возможно, Всевышний поможет этому игроку защититься от следующей пули.",
                    Duration = 0,
                    State = [ModifierState.PLAYER_EVASION],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "cross",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_COVER => throw new NotImplementedException(),
                ModifierKey.PLAYER_DEFIBRILLATOR => new(Context)
                {
                    Name = "Дефибриллятор",
                    Description = "Получив летальный урон, этот игрок вернется к жизни.",
                    Duration = -1,
                    State = [],
                    IsHidden = true,
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "heart-flash",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_BLOOD_PACK => new(Context)
                {
                    Name = "В ожидании донора",
                    Description = "Как только прольется чья-то кровь, этот игрок восстановит себе здоровье.",
                    Duration = -1,
                    State = [],
                    IsHidden = true,
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "blood-bag",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_MIRROR => new(Context)
                {
                    Name = "Зеркало",
                    Description = "OVERRIDE",
                    Duration = -1,
                    State = [],
                    IsHidden = true,
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "mirror-variant",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_GRENADE => new(Context)
                {
                    Name = "Граната",
                    Description = "Когда кто-то попытается украсть или отменить предмет этого игрока, он получит в подарок гранату (без чеки).",
                    Duration = -1,
                    State = [],
                    IsHidden = true,
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "handshake",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_CHAINED => new(Context)
                {
                    Name = "Скованные одной цепью",
                    Description = "OVERRIDE",
                    Duration = -1,
                    State = [ModifierState.PLAYER_CHAINED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "link-variant",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_DOLLAR => new(Context)
                {
                    Name = "Подкуплен",
                    Description = "OVERRIDE",
                    Duration = -1,
                    State = [ModifierState.PLAYER_CORRUPTED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "currency-usd",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_AWAIT_BLINDFOLD => new(Context)
                {
                    Name = "Готовиться надеть повязку",
                    Description = "На своем следующем ходу этот игрок будет в повязке.",
                    Duration = -1,
                    State = [],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "eye-outline",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_BLINDFOLD => new(Context)
                {
                    Name = "В повязке",
                    Description = "Этот игрок ничего не видит, даже этот модификатор...",
                    Duration = -1,
                    State = [ModifierState.PLAYER_BLINDED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "eye-off-outline",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_DISORIENTATION => new(Context)
                {
                    Name = "Дезориентирован",
                    Description = "Этот игрок не видит кол-во своего здоровья до следующего раунда.",
                    Duration = -1,
                    State = [ModifierState.PLAYER_DISORIENTED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "crosshairs-question",
                    IsBuff = false,
                },
                // Эффекты предметов
                ModifierKey.ITEM_CANNOT_BE_STOLEN => new(Context)
                {
                    Name = "Жулик, не воруй!",
                    Description = "Этот предмет нельзя украсть.",
                    Duration = -1,
                    State = [ModifierState.ITEM_CANNOT_BE_STOLEN],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "hand-back-left-off",
                    IsBuff = true,
                },
                ModifierKey.ITEM_ADRENALINE => new(Context)
                {
                    Name = "Избавление от улик",
                    Description = "Этот предмет был украден, игрок сбросит его, как только завершит свой ход.",
                    Duration = 1,
                    State = [ModifierState.ITEM_LOST_ON_TURN_ENDED],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "needle-off",
                    IsBuff = false,
                },
                ModifierKey.ITEM_CLAY => new(Context)
                {
                    Name = "Глиняный предмет",
                    Description = "Этот предмет сделан из глины, и его невозможно украсть.",
                    Duration = -1,
                    State = [ModifierState.ITEM_CANNOT_BE_STOLEN, ModifierState.ITEM_CLAY],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "wall",
                    IsBuff = true,
                },

                // Эффекты дробовика
                ModifierKey.RIFLE_HACKSAW => new(Context)
                {
                    Name = "Ножовка",
                    Description = "Дробовик обрезан, при попадании будет нанесено больше урона!",
                    Duration = -1,
                    State = [ModifierState.RIFLE_BONUS_DAMAGE],
                    TargetType = ModifierTargetType.RIFLE,
                    Icon = "hand-saw",
                    IsBuff = true,
                },
                ModifierKey.RIFLE_SILENCER => new(Context)
                {
                    Name = "Глушитель",
                    Description = "Дробовик с глушителем, при попадании цель перестанет видеть свое здоровье!",
                    Duration = -1,
                    State = [ModifierState.RIFLE_SILENCED],
                    TargetType = ModifierTargetType.RIFLE,
                    Icon = "volume-off",
                    IsBuff = true,
                },
                _ => throw new NotImplementedException(),
            };
        }
    }
}
