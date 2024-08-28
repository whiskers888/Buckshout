using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Blindfold(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Повязка на глаза";
        public override string Description => "Как только наступит ход выбранного игрока, его глаза будут закрыты повязкой.\n" +
                                              "Игрок с закрытыми глазами не может исмользовать предметы, и не может отличить игроков друг от друга (в том числе и себя).\n" +
                                              "Порядок игроков случайно перемешивается для игрока в повязке.";

        public override string Lore => "Этот предмет был украден у моей бабушки, хотел еще забрать прищепки на соски!";
        public override string Model => "blindfold";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "blindfold/put"},
            {ItemEvent.CANCELED, "blindfold/out"},
        };

        public override void Effect(EventData e)
        {
            var modifierAwaitBlindfold = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_AWAIT_BLINDFOLD);
            modifierAwaitBlindfold.Apply(e.Target!);

            modifierAwaitBlindfold.RemoveWhen(Event.TURN_CHANGED, null, (turnE) =>
            {
                if (e.Target == turnE.Target)
                {
                    var modifierBlindfold = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_BLINDFOLD);
                    modifierBlindfold.Apply(e.Target!);

                    modifierBlindfold.RemoveWhen(Event.TURN_CHANGED);
                    return true;
                }
                return false;
            });
        }
    }
}
