using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Manager
{
    public class ModifierManager
    {
        public ModifierManager(GameContext context)
        {
            Context = context;
            Modifiers = [];
            CreateModifiers();
        }
        public GameContext Context { get; }

        public readonly Dictionary<ModifierKey, Modifier> Modifiers;
        private void CreateModifiers()
        {
            // Эффекты игрока
            Modifiers.Add(ModifierKey.PLAYER_DEAD, new(Context)
            {
                Name = "Мертв!",
                Description = "Больше никакие дефибрилляторы и переливания крови ему не помогут...",
                Duration = -1,
                State = [ModifierState.PLAYER_DEAD],
                TargetType = ModifierTargetType.PLAYER,
                Icon = "emoticon-dead-outline",
                IsBuff = false,

            });
            Modifiers.Add(ModifierKey.PLAYER_HANDCUFFS, new(Context)
            {
                Name = "Оцепенение",
                Description = "Игрок находится в наручниках, поэтому пропускает свой следующий ход",
                Duration = 1,
                State = [ModifierState.PLAYER_STUNED],
                TargetType = ModifierTargetType.PLAYER,
                Icon = "handcuffs",
                IsBuff = false,
            });

            // Эффекты предметов
            Modifiers.Add(ModifierKey.ITEM_CANNOT_BE_STOLEN, new(Context)
            {
                Name = "ПРЕДМЕТ 404",
                Description = "Этот предмет нельзя украсть",
                Duration = -1,
                State = [ModifierState.ITEM_CANNOT_BE_STOLEN],
                TargetType = ModifierTargetType.ITEM,
                Icon = "hand-back-left-off",
                IsBuff = true,
            });
            Modifiers.Add(ModifierKey.ITEM_ADRENALINE, new(Context)
            {
                Name = "Жулик, не воруй",
                Description = "Предмет был украден, поэтому он пропадет на следующий ход",
                Duration = 1,
                State = [ModifierState.ITEM_LOST_ON_TURN_ENDED],
                TargetType = ModifierTargetType.ITEM,
                Icon = "needle-off",
                IsBuff = false,
            });

            // Эффекты винтовки
            Modifiers.Add(ModifierKey.RIFLE_HACKSAW, new(Context)
            {
                Name = "Ножовка",
                Description = "Винтовка обрезана, будет нанесен двойной урон",
                Duration = -1,
                State = [ModifierState.RIFLE_BONUS_DAMAGE],
                TargetType = ModifierTargetType.RIFLE,
                Icon = "hand-saw",
                IsBuff = true,
                Value = 2
            });
        }

    }
}
