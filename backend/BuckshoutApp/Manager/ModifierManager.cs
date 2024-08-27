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
                    Description = "Вам не страшен летальный урон, так как вы возродитесь",
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
                    Description = "Когда попытаются у вас украсть предмет или отменить ваш предмет, вы оторвете чеку и передадите граната в руки другому игроку",
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
                    Name = "Готовиться одеть повязку",
                    Description = "На следующем своем ходе вы будете в повязке",
                    Duration = -1,
                    State = [ModifierState.PLAYER_BLINDED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "blind",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_BLINDFOLD => new(Context)
                {
                    Name = "В повязке",
                    Description = "Вы ничего не видите, так как на вас повязке",
                    Duration = -1,
                    State = [ModifierState.PLAYER_BLINDED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "blind",
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
                    Description = "Предмет был украден, игрок сбросит его, как только его ход завершится.",
                    Duration = 1,
                    State = [ModifierState.ITEM_LOST_ON_TURN_ENDED],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "needle-off",
                    IsBuff = false,
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
                _ => throw new NotImplementedException(),
            };
        }
    }
}
