using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items.Trap
{
    internal class Mirror(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Зеркало";
        public override string Description => "Имеется 2 типа использования:\n" +
                                              "- При использовании на себя: Следующий предмет, который будет использован на Вас другим игроком, применится и на него тоже.\n" +
                                              "- При использовании на врага: Эффект сдедующего ненаправленного предмета, примененного целью, будет скопирован Вами (если это возможно).\n" +
                                              "Не действует на ловушки и Печать \"Переделать\" при любом варианте использования.\n" +
                                              "Можно применить неограниченное кол-во раз, при этом каждое из зеркал подействует отдельно, на первый же подходящий предмет, и израсходуется, даже если его не удасться применить (например, вторые Наручники).";

        public override string Lore => "Если долго всматриваться в зеркало, что-то в зеркале начнет всматриваться в тебя.";
        public override string Model => "mirror";

        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override ItemType Type => ItemType.TRAP;

        private void ApplyTrap(EventData e, Modifier modifier, bool onSelf, Func<EventData, bool> checker)
        {
            modifier.RemoveWhen(Event.ITEM_EFFECTED, null, (itemE) =>
            {
                Item item = (Item)itemE.Special["ITEM"];
                if (itemE.Special.TryGetValue("IS_MIRROR", out object value))
                {
                    if ((bool)value == true) return false;
                }
                if (checker.Invoke(itemE))
                {
                    item.Use(new EventData()
                    {
                        Target = onSelf ? itemE.Initiator : e.Initiator,
                        Initiator = onSelf ? itemE.Target : e.Target,
                        Special = new Dictionary<string, object> { { "IS_MIRROR", true } }
                    }, true);
                    e.Special["SOUND"] = "items/mirror/break";
                    Context.EventManager.Trigger(Event.PLAY_SOUND, e);
                    ShowTrap(e.Initiator!);
                    return true;
                }
                return false;
            });
        }
        public override void Effect(EventData e)
        {
            Modifier modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_MIRROR);

            if (e.Initiator == e.Target)
            {
                modifier.Description = "Следующий предмет, который будет использован на Вас другим игроком, применится и на него тоже.";
                ApplyTrap(e, modifier, true, (itemE) =>
                {
                    var item = (Item)itemE.Special["ITEM"];
                    return itemE.Target == e.Initiator && itemE.Initiator != e.Initiator &&
                           item.TargetType == ItemTargetType.PLAYER && item.Type != ItemType.TRAP;
                });
            }
            else
            {
                modifier.Description = $"Эффект сдедующего ненаправленного предмета, примененного игроком {e.Target!.Name}, будет скопирован Вами.";
                ApplyTrap(e, modifier, false, (itemE) =>
                {
                    Item item = (Item)itemE.Special["ITEM"];
                    return itemE.Initiator == e.Target && item.TargetType == ItemTargetType.NONE &&
                           item.Type != ItemType.TRAP && !item.Behavior.Contains(ItemBehavior.CUSTOM);
                });
            }
            //HACK: Не убирать так как тогда не оверайдит описание
            modifier.Apply(e.Initiator!);
        }
    }
}
