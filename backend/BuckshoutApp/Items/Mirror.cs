using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    internal class Mirror(GameContext context) : Item(context)
    {
        public override string Name => "Зеркало";
        // При использовании на себя: При применение на вас предмета, он так же применяется на кастера
        // При использовании на врага: Мы копируем no target эффекты(либо применение х2, либо на тебя кроме пилы)
        public override string Description => "Имеется 2 типа использования\n" +
                                              "При использовании на себя: Следующий предмет который будет использован на вас, применится и на врага\n" +
                                              "При использовании на врага: Копируется эффект любого предмета без цели примененный врагом(кроме пилы)";

        public override string Lore => "Кривое зеркало";
        public override string Model => "mirror";

        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.PLAYER;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ANY;

        public override ItemType Type => ItemType.TRAP;



        public override void Effect(EventData e)
        {
            var id = "";
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_MIRROR);
            modifier.Apply(e.Initiator);
            if (e.Initiator == e.Target)
            {
                modifier.RemoveWhen(Event.ITEM_EFFECTED, null, (itemE) =>
                {
                    Item item = (Item)itemE.Special["ITEM"];
                    if (itemE.Target == e.Initiator && item.TargetType == ItemTargetType.PLAYER)
                    {
                        modifier.Description = "Следующий предмет который будет использован на вас, применится и на врага";
                        item.Use(new EventData()
                        {
                            Target = itemE.Initiator,
                            Initiator = itemE.Target,
                        }, true);
                        return true;
                    }
                    return false;
                });
            }
            else
            {
                modifier.RemoveWhen(Event.ITEM_EFFECTED, null, (itemE) =>
                {
                    Item item = (Item)itemE.Special["ITEM"];
                    if (itemE.Initiator == e.Target &&
                        item.TargetType == ItemTargetType.NONE &&
                        item.Name != "Печать \"Переделать\"")
                    {

                        modifier.Description = $"Копируется эффект любого предмета без цели примененный на {e.Target!.Name}";
                        item.Use(new EventData()
                        {
                            Initiator = e.Initiator,
                            Target = e.Target,
                        }, true);
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}
