using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class ExpiredMedicine : Item
    {
        public ExpiredMedicine(GameContext context) : base(context)
        {
        }

        public override string Name => "Просроченные таблетки";
        public override string Description => $"С шансом {HEAL_CHANCE}% восстанавливает {HEAL_AMOUNT} или отнимет {DAMAGE_AMOUNT} ед. здоровья.";
        public override string Model => "expired_medicine";

        public int HEAL_CHANCE = 50;
        public int HEAL_AMOUNT = 2;
        public int DAMAGE_AMOUNT = 1;

        public override void Effect(EventData e)
        {
            int healOrDamage = Context.Random.Next(0, 100);
            if (healOrDamage >= HEAL_CHANCE)
                e.initiator.ChangeHealth(Manager.ChangeHealthType.Heal, HEAL_AMOUNT, e.initiator);
            else
                e.initiator.ChangeHealth(Manager.ChangeHealthType.Damage, DAMAGE_AMOUNT, e.initiator);
        }
    }
}
