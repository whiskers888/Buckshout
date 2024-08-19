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
                    Description = "Игрок находится в наручниках, поэтому пропускает свой следующий ход",
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
                    Description = $"Время хода сокращено в 2 раза.",
                    Duration = 0,
                    State = [ModifierState.PLAYER_TURN_TIME_LIMITED],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "timer",
                    IsBuff = false,
                },
                ModifierKey.PLAYER_CHURCH_CROSS => new(Context)
                {
                    Name = "Крестный отец",
                    Description = $" Возможно Всевышний поможет ему защититься от следующей пули",
                    Duration = 1,
                    State = [ModifierState.PLAYER_EVASION],
                    TargetType = ModifierTargetType.PLAYER,
                    Icon = "cross",
                    IsBuff = true,
                },
                ModifierKey.PLAYER_COVER => throw new NotImplementedException(),
                ModifierKey.PLAYER_DEFIBRILLATOR => throw new NotImplementedException(),
                ModifierKey.PLAYER_CHAINED => throw new NotImplementedException(),
                // Эффекты предметов
                ModifierKey.ITEM_CANNOT_BE_STOLEN => new(Context)
                {
                    Name = "ПРЕДМЕТ 404",
                    Description = "Этот предмет нельзя украсть",
                    Duration = -1,
                    State = [ModifierState.ITEM_CANNOT_BE_STOLEN],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "hand-back-left-off",
                    IsBuff = true,
                },
                ModifierKey.ITEM_ADRENALINE => new(Context)
                {
                    Name = "Жулик, не воруй",
                    Description = "Предмет был украден, поэтому он пропадет на следующий ход",
                    Duration = 1,
                    State = [ModifierState.ITEM_LOST_ON_TURN_ENDED],
                    TargetType = ModifierTargetType.ITEM,
                    Icon = "needle-off",
                    IsBuff = false,
                },
                // Эффекты винтовки
                ModifierKey.RIFLE_HACKSAW => new(Context)
                {
                    Name = "Ножовка",
                    Description = "Винтовка обрезана, будет нанесен двойной урон",
                    Duration = -1,
                    State = [ModifierState.RIFLE_BONUS_DAMAGE],
                    TargetType = ModifierTargetType.RIFLE,
                    Icon = "hand-saw",
                    IsBuff = true,
                    Value = 2
                },
                _ => throw new NotImplementedException(),
            };
        }
    }
}
