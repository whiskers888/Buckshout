﻿using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Dollar(GameContext context) : Item(context)
    {
        public override string Name => "100 Долларов";
        public override string Description => "Вы подкупаете выбранного противника, отдавая ему этот предмет, и следующий выстрел, направленный в Вас, он примет на себя.\n" +
                                              "Предмет никак не влияет на педедачу права хода.\n" +
                                              "Вы можете подкупить несколько разных игроков одновременно, но защита сработает на первый же выстрел у всех сразу.\n" +
                                              "Нельзя применить на игрока, которого уже кто-то подкупил.\n" +
                                              "Также, одним экземпляром этого предмета можно подкупить каждого игрока лишь раз, если не осталось игроков для подкупа, предмет исчезнет.\n";
        public override string Model => "dollar";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "dollar/bribery"},
            {ItemEvent.EFFECTED, "dollar/stonks"}
        };

        private List<Player> Corrupted { get; set; } = [];

        internal override void BeforeUse(EventData e)
        {
            if (Corrupted.Contains(e.target))
            {
                Disallow(e, "Этот игрок уже был подкуплен этим предметом!");
            }
            if (e.target.Is(ModifierState.PLAYER_CORRUPTED))
            {
                Disallow(e, "Этот игрок уже подкуплен!");
            }
        }

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DOLLAR);
            modifier.Description = $"Когда в игрока {e.initiator.Name} будут стрелять, этот игрок примет выстрел на себя.";
            modifier.Apply(e.target);

            modifier.RemoveWhen(Event.BEFORE_DAMAGE_TAKE, null, (damegeE) =>
            {
                if (modifier.Removed) return true;
                if (damegeE.target == e.initiator && damegeE.special["TYPE"] == "RIFLE")
                {
                    damegeE.Prevent = true;
                    e.target.ChangeHealth(ChangeHealthType.Damage, (int)damegeE.special["VALUE"], e.initiator, "DOLLAR");
                    return true;
                }
                return false;
            });
            modifier.RemoveWhen(Event.RIFLE_SHOT, null, (shotE) =>
            {
                if (shotE.target == e.initiator) return true;
                return false;
            });

            Corrupted.Add(e.target);
            if (Corrupted.Count < Context.PlayerManager.Players.Count)
                e.target.AddItem(this);
        }
    }
}