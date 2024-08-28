using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Chain(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Цепь";
        public override string Description => "Связывает цель c другим случайным игроком (в том числе и Вы), но в приоритете еще не связанным.\n" +
                                              "Когда один из связанных игроков каким-либо образом теряет здоровье, со вторым происходит то же самое.\n" +
                                              "Эффект применяется к каждой из целей отдельно, и развеивается, как только нанесет урон.\n" +
                                              "Урон от Цепи не может быть передан Цепью.";
        public override string Lore => "Да что ты как с цепи сорвался!?";
        public override string Model => "chain";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "chain/throw"},
            {ItemEvent.EFFECTED, "chain/wind_gust" },
            {ItemEvent.CANCELED, "chain/break"}
        };

        internal override void BeforeUse(EventData e)
        {
            /*if (e.Target!.Is(ModifierState.PLAYER_CHAINED))
            {
                Disallow(e, "Этот игрок уже связан!");
            }
            if (!Context.PlayerManager.AlivePlayers.Where(it => !it.Is(ModifierState.PLAYER_CHAINED) && it != e.Target).Any())
            {
                Disallow(e, "Не с кем связать!");
            }*/

            if (!Context.PlayerManager.AlivePlayers.Where(it => it != e.Target).Any())
            {
                Disallow(e, "Не с кем связать!");
            }
        }

        private void ApplyModifiers(Player target, Player victim)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHAINED);
            modifier.Description = $"Если {target.Name} получит урон, это игрок тоже пострадает.";
            modifier.Apply(victim);

            modifier.RemoveWhen(Event.DAMAGE_TAKEN, null, damageE =>
            {
                if (damageE.Special.TryGetValue("TYPE", out object? value))
                {
                    if (value.ToString() == "CHAIN") return false;
                }
                if (damageE.Target == target)
                {
                    victim.ChangeHealth(ChangeHealthType.Damage, (int)damageE.Special["VALUE"], target, "CHAIN");
                    return true;
                }
                return false;
            });
            modifier.RemoveWhen(Event.PLAYER_LOST, null, loseE =>
            {
                return loseE.Target == target;
            });
        }

        public override void Effect(EventData e)
        {
            var targets = Context.PlayerManager.AlivePlayers.Where(it => !it.Is(ModifierState.PLAYER_CHAINED) && it != e.Target).ToList();
            if (targets.Count == 0)
            {
                targets = Context.PlayerManager.AlivePlayers.Where(it => it != e.Target).ToList();
            }
            var target = targets.RandomChoise();

            ApplyModifiers(e.Target!, target);
            ApplyModifiers(target, e.Target!);

            e.Special["MESSAGE"] = $"Игроки {e.Target!.Name} и {target.Name} теперь связаны!";
            Context.EventManager.Trigger(Event.MESSAGE, e);
        }
    }
}
