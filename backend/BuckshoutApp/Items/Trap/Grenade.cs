using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items.Trap
{
    public class Grenade(GameContext context) : Item(context)
    {
        public override string Name => "Граната";
        public override string Description => "Когда любой другой игрок попытается украсть ваш предмет или отменить его, \n" +
                                              $"он получит эту гранату себе в руку без чеки и получит урон в размере {DAMAGE} ед.\n";

        public override string Lore => "Так вот откуда пошла фраза \" Отдам в добрые руки\"";
        public override string Model => "grenade";
        public int DAMAGE = 1;
        public override ItemType Type => ItemType.TRAP;

        private bool ApplyTrap(EventData e, EventData itemE)
        {
            if (itemE.Target == e.Initiator)
            {
                e.Special["SOUND"] = "items/grenade/explosion";
                Context.EventManager.Trigger(Event.PLAY_SOUND, e);
                ShowTrap(e.Initiator!);
                itemE.Initiator!.ChangeHealth(ChangeHealthType.Damage, DAMAGE, e.Initiator!, "GRENADE");
                return true;
            }
            return false;
        }
        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_GRENADE);
            modifier.Apply(e.Initiator!);
            Context.EventManager.Once(Event.TURN_CHANGED, (_) =>
            {
                modifier.RemoveWhen(Event.ITEM_STOLEN, null, (itemE) =>
                {
                    return ApplyTrap(e, itemE);
                });

                modifier.RemoveWhen(Event.ITEM_CANCELED, null, (itemE) =>
                {
                    return ApplyTrap(e, itemE);
                });
            });

        }
    }
}
