using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Chain(GameContext context) : Item(context)
    {
        public override string Name => "Цепь";
        public override string Description => "Связывает цель со случайным игроком (в том числе и вы).\n" +
                                              "Когда один из связанных игроков каким-либо образом теряет здоровье, со вторым происходит то же самое.\n" +
                                              "Эффект применяется к каждой из целей отдельно, и наносит урон связанному игроку, развеевается после попытки выстрела в игрока.\n" +
                                              "Не может примениться на уже связанного игрока.";
        public override string Lore => "Да что ты как с цепи сорвался!?";
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

        private void ApplyModifiers(Player target, Player victim)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_CHAINED);
            modifier.Description = $"Если этот игрок получит урон, игрок {victim.Name} тоже пострадает.";
            modifier.Apply(target);

            var id = modifier.On(Event.DAMAGE_TAKEN, damageE =>
            {
                if (damageE.special.TryGetValue("TYPE", out object? value))
                {
                    if (value == "CHAIN") return;
                }
                if (damageE.target == target)
                {
                    victim.ChangeHealth(Manager.ChangeHealthType.Damage, (int)damageE.special["VALUE"], target, "CHAIN");
                }
            });
            modifier.OnRemoved = () =>
            {
                Context.EventManager.Unsubscribe(Event.DAMAGE_TAKEN, id);
            };
            modifier.RemoveWhen(Event.RIFLE_SHOT, null, (shotE) =>
            {
                if (shotE.target == target) return true;
                return false;
            });
        }

        public override void Effect(EventData e)
        {
            var target = Context.PlayerManager.AlivePlayers.Where(it => !it.Is(ModifierState.PLAYER_CHAINED) && it != e.target).ToList().RandomChoise();

            ApplyModifiers(e.target, target);
            ApplyModifiers(target, e.target);

            e.special["MESSAGE"] = $"Игроки {e.target.Name} и {target.Name} теперь связаны!";
            Context.EventManager.Trigger(Event.MESSAGE, e);
        }
    }
}
