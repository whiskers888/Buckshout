using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class BloodPack(GameContext context) : Item(context)
    {
        public override string Name => "Переливание крови";
        public override string Description => "Когда любой другой игрок потеряет здоровье, вы восстановите себе такое же кол-во здоровья.\n" +
                                              "Начинает действовать только после окончания Вашего хода, и срабатывает один раз.\n" +
                                              "Сочетается с другими Переливаниями крови так, что каждый из предметов восстановит здоровье в полном объеме, не зависимо друг от друга.";

        public override string Lore => "Не пропадать же добру...";
        public override string Model => "blood_pack";

        public override ItemType Type => ItemType.TRAP;

        public override void Effect(EventData e)
        {
            e.Special.Add("MESSAGE", "[DEV] СКРЫТИЕ МОДИФИКАТОРОВ НЕ РЕАЛИЗОВАНО!");
            Context.EventManager.Trigger(Event.SECRET_MESSAGE, e);

            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_BLOOD_PACK);
            modifier.Apply(e.Initiator);

            Context.EventManager.Once(Event.TURN_CHANGED, (_) =>
            {
                modifier.RemoveWhen(Event.DAMAGE_TAKEN, null, (damageE) =>
                {
                    if (damageE.Target != e.Initiator)
                    {
                        e.Initiator.ChangeHealth(ChangeHealthType.Heal, (int)damageE.Special["VALUE"], e.Initiator);
                        return true;
                    }
                    return false;
                });
            });
        }
    }
}
