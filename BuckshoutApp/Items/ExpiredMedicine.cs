using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class ExpiredMedicine : Item
    {
        public ExpiredMedicine(GameContext context) : base(context)
        {
        }

        public override string Name => "Просроченые таблетки";
        public override string Description => $"Восстановит {MAX_HEAL_HEALTH} или отнимет {MAX_DAMAGE_HEALTH} от ед. здоровья.";
        public override string Model => "expired_medicine";
        public int MAX_HEAL_HEALTH = 2;
        public int MAX_DAMAGE_HEALTH = 1;

        public override void Effect(EventData e)
        {
            int healOrDamage = Context.Random.Next(0, 1);
            if (healOrDamage == 1)
                e.initiator.ChangeHealth(Manager.ChangeHealthType.Heal, MAX_HEAL_HEALTH, e.initiator);
            else
                e.initiator.ChangeHealth(Manager.ChangeHealthType.Damage, MAX_DAMAGE_HEALTH, e.initiator);
        }
    }
}
