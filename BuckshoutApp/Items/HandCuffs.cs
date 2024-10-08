﻿using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Handcuffs(GameContext context) : Item(context)
    {
        public override string Name => "Наручники";
        public override string Description => "Выбранный противник пропускает свой следующий ход.\n" +
                                              "Нельзя применить несколько раз подряд на одного и того же игрока.";
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
            if (e.target.Is(ModifierState.PLAYER_STUNNED))
            {
                Disallow(e, "Этот игрок уже пропускает ход!");
            }
        }

        public override void Effect(EventData e)
        {
            Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_HANDCUFFS).Apply(e.target);
        }
    }
}
