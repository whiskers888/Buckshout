using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items.Trap
{
    public class Defibrillator(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Дефибриллятор";
        public override string Description => $"Когда Вы получаете летальный урон, Вы не умираете, а выживаете с {MIN_HEALTH} ед. здоровья.\n" +
                                              "Начинает действовать только после окончания Вашего хода, и срабатывает один раз.";

        public override string Lore => "Wraith King, а не рейтинг!";
        public override string Model => "defibrillator";
        public override ItemType Type => ItemType.TRAP;

        public int MIN_HEALTH = 1;

        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DEFIBRILLATOR);
            modifier.Apply(e.Initiator!);
            Context.EventManager.Once(Event.TURN_CHANGED, (_) =>
            {
                modifier.RemoveWhen(Event.BEFORE_DAMAGE_TAKE, null, (damageE) =>
                {
                    if (damageE.Target == e.Initiator)
                    {
                        int damage = (int)damageE.Special["VALUE"];
                        if (damage >= e.Initiator!.Health)
                        {
                            e.Special["SOUND"] = "items/defibrillator/charge";
                            Context.EventManager.Trigger(Event.PLAY_SOUND, e);
                            ShowTrap(e.Initiator!);
                            int countGiveHP = damage - e.Initiator!.Health + MIN_HEALTH;
                            e.Initiator.ChangeHealth(ChangeHealthType.Heal, countGiveHP, e.Initiator, "DEFIBRILLATOR");
                            return true;
                        }
                    }
                    return false;
                });
            });
        }
    }
}
