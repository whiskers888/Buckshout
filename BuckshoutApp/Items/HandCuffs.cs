﻿using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Handcuffs : Item
    {
        public Handcuffs(GameContext context) : base(context)
        {
        }

        public override string Name => "Наручники";
        public override string Description => "Выбранный противник пропускает свой следующий ход.";
        public override string Model => "handcuffs";
        public override ItemBehavior[] Behavior { get; } = { ItemBehavior.UNIT_TARGET };
        public override TargetType TargetType => TargetType.PLAYER;
        public override TargetTeam TargetTeam => TargetTeam.ENEMY;

        public override void Effect(EventData e)
        {
            Console.WriteLine($"{e.initiator?.Name} применил {Name} на {e.target?.Name}  ");

            Context.PlayerModifiers["modifier_handcuffs"].Apply(e.target);

            /*Context.QueueManager.SkipPlayer(e.target);*/
        }
    }
}
