using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class ExpiredMedicine(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Просроченные таблетки";
        public override string Description => $"С шансом {HEAL_CHANCE}% восстанавливает {HEAL_AMOUNT} или отнимет {DAMAGE_AMOUNT} ед. здоровья.";
        public override string Lore => "Бесплатная медецина...";
        public override string Model => "expired_medicine";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "medicine/unpack" },
        };

        public int HEAL_CHANCE = 50;
        public int HEAL_AMOUNT = 2;
        public int DAMAGE_AMOUNT = 1;

        public override void Effect(EventData e)
        {
            int healOrDamage = Context.Random.Next(0, 100);
            if (HEAL_CHANCE >= healOrDamage)
            {
                e.Special["SOUND"] = "items/medicine/heal";
                e.Initiator!.ChangeHealth(Manager.ChangeHealthType.Heal, HEAL_AMOUNT, e.Initiator);
            }
            else
            {
                e.Special["SOUND"] = "items/medicine/damage";
                e.Initiator!.ChangeHealth(Manager.ChangeHealthType.Damage, DAMAGE_AMOUNT, e.Initiator);
            }

            Context.EventManager.Trigger(Event.PLAY_SOUND, e);
        }
    }
}
