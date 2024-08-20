using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Chain(GameContext context) : Item(context)
    {
        public override string Name => "Цепь";
        public override string Description => "Связывает цель со случайным игроком (в том числе и вы).\n" +
                                              "Когда один из связанных игроков каким-либо образом теряет здоровье, со вторым происходит то же самое.\n" +
                                              "Эффект применяется к каждой из целей и наносит урон связанному игроку, при этом развеивается каждый из эффектов по отдельности, как только ход дойдет до игрока.\n" +
                                              "Не может примениться на уже связанного игрока.";
        public override string Model => "chain";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        internal override void BeforeUse(EventData e)
        {
            if (e.target.Is(ModifierState.PLAYER_CHAINED))
            {
                Disallow(e, "Этот игрок уже связан!");
            }
            if (Context.PlayerManager.AlivePlayers.Where(it => !it.Is(ModifierState.PLAYER_CHAINED) && it != e.target).Count() < 1)
            {
                Disallow(e, "Не с кем связать!");
            }
        }

        public override void Effect(EventData e)
        {
            var target = Context.PlayerManager.AlivePlayers.Where(it => !it.Is(ModifierState.PLAYER_CHAINED) && it != e.target).ToList().RandomChoise();
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHAINED);
            var modifier2 = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHAINED);
            modifier.Apply(e.target);
            modifier2.Apply(target);

            e.special["MESSAGE"] = $"Игроки {e.target.Name} и {target.Name} теперь связаны!";
            Context.EventManager.Trigger(Event.MESSAGE, e);

            var id = modifier.On(Event.DAMAGE_TAKEN, damageE =>
            {
                if (damageE.special.TryGetValue("TYPE", out object? value))
                {
                    if (value == "CHAIN") return;
                }
                if (damageE.target == e.target)
                {
                    target.ChangeHealth(Manager.ChangeHealthType.Damage, (int)damageE.special["VALUE"], damageE.target, "CHAIN");
                }
            });
            modifier.OnRemoved = () =>
            {
                Context.EventManager.Unsubscribe(Event.DAMAGE_TAKEN, id);
            };

            var id2 = Context.EventManager.SubscribeUniq(Event.DAMAGE_TAKEN, damageE =>
            {
                if (damageE.special.TryGetValue("TYPE", out object? value))
                {
                    if (value == "CHAIN") return;
                }
                else if (damageE.target == target)
                {
                    e.target.ChangeHealth(Manager.ChangeHealthType.Damage, (int)damageE.special["VALUE"], damageE.target, "CHAIN");
                };
            });

            modifier2.OnRemoved = () =>
            {
                Context.EventManager.Unsubscribe(Event.DAMAGE_TAKEN, id2);
            };
        }
    }
}
