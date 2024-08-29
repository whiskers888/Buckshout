using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Handcuffs(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Наручники";
        public override string Description => "Сковывет выбранного противника, из-за чего тот пропускает свой следующий ход.\n" +
                                              "Цель не сможет выстрелить или использовать предметы, но все эффекты, касающиеся наступления хода этого игрока, сработают.\n" +
                                              "Нельзя применить на игрока, который уже скован, пока он не походит.";
        public override string Lore => "И постарайтесь НЕ потерять ключ.";
        public override string Model => "handcuffs";
        public override ItemBehavior[] Behavior { get; } = { ItemBehavior.UNIT_TARGET };
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "handcuffs/shackling"},
            {ItemEvent.CANCELED, "handcuffs/break"}
        };

        internal override void BeforeUse(EventData e)
        {
            if (e.Target!.Is(ModifierState.PLAYER_STUNNED))
            {
                Disallow(e, "Этот игрок уже пропускает ход!");
            }
        }

        public override void Effect(EventData e)
        {
            Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_HANDCUFFS).Apply(e.Target);
        }
    }
}
