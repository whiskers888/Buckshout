using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items.Trap
{
    public class Defibrillator(GameContext context) : Item(context)
    {
        public override string Name => "Дефибриллятор";
        public override string Description => $"Когда вы получаете летальный урон, вы не умираете, а выживаете с {COUNT_HEAL_HP} ед. здоровья";

        public override string Lore => "Wraith King, а не рейтинг!";
        public override string Model => "defibrillator";
        public int COUNT_HEAL_HP = 1;
        public override ItemType Type => ItemType.TRAP;

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
                            int countGiveHP = damage - e.Initiator!.Health + COUNT_HEAL_HP;
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
