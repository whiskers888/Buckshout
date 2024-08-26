using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Blindfold(GameContext context) : Item(context)
    {
        public override string Name => "Повязка на глаза";
        public override string Description => "Завязывает глаза игроку, в следствии чего он не видит не игроков, не их предметы." +
                                              "Начинает действовать только с хода цели.";

        public override string Lore => "Этот предмет был украден у моей бабушки, хотел еще забрать прищепки на соски!";
        public override string Model => "blindfold";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;

        public override void Effect(EventData e)
        {
            var modifierAwaitBlindfold = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_AWAIT_BLINDFOLD);
            modifierAwaitBlindfold.Apply(e.Initiator!);

            Context.EventManager.Once(Event.TURN_CHANGED, (_) =>
            {
                modifierAwaitBlindfold.RemoveWhen(Event.TURN_CHANGED, null, (_) =>
                {
                    if (e.Initiator == Context.QueueManager.Current)
                    {
                        var modifierBlindfold = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_BLINDFOLD);
                        modifierBlindfold.Apply(e.Initiator!);

                        modifierBlindfold.RemoveWhen(Event.TURN_CHANGED);

                        return true;
                    }
                    return false;
                });
            });
        }
    }
}
