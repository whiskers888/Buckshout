﻿using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Dollar(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "100 Долларов";
        public override string Description => "Вы подкупаете выбранного противника, отдавая ему этот предмет, и следующий выстрел, направленный в Вас, он примет на себя.\n" +
                                              "Предмет никак не влияет на передачу права хода.\n" +
                                              "Вы можете подкупить несколько разных игроков одновременно, при этом первый же выстрел перенаправится во все цели сразу.\n" +
                                              "Нельзя применить на игрока, которого уже кто-то подкупил.\n" +
                                              "Одним экземпляром этого предмета можно подкупить каждого игрока лишь один раз, если не осталось игроков для подкупа, предмет исчезнет.";
        public override string Lore => "isgreedgood?";
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
            if (Corrupted.Contains(e.Target!))
            {
                Disallow(e, "Этот игрок уже был подкуплен этим предметом!");
            }
            if (e.Target!.Is(ModifierState.PLAYER_CORRUPTED))
            {
                Disallow(e, "Этот игрок уже подкуплен!");
            }
        }

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DOLLAR);
            modifier.Description = $"Когда в игрока {e.Initiator!.Name} будут стрелять, этот игрок примет выстрел на себя.";
            modifier.Apply(e.Target!);

            modifier.RemoveWhen(Event.BEFORE_DAMAGE_TAKE, null, (damageE) =>
            {
                if (modifier.Removed) return true;
                if (damageE.Target == e.Initiator && damageE.Special["TYPE"].ToString() == "RIFLE")
                {
                    damageE.Prevent = true;
                    e.Target!.ChangeHealth(ChangeHealthType.Damage, (int)damageE.Special["VALUE"], e.Initiator, "DOLLAR");
                    return true;
                }
                return false;
            });

            modifier.RemoveWhen(Event.RIFLE_SHOT, null, (shotE) =>
            {
                return shotE.Target == e.Initiator;
            });

            Corrupted.Add(e.Target!);
            if (Corrupted.Count < Context.PlayerManager.Players.Count)
                e.Target!.AddItem(this);
            /*В случае если будет вариант для создания нового объекта и потребуется поменять ID
             * {
                Item item = (Item)this.MemberwiseClone();
                item.Id = Guid.NewGuid().ToString();
                e.Target!.AddItem(item);
            }*/
        }
    }
}