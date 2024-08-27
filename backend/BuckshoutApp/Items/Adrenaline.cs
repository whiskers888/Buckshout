using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Adrenaline : Item
    {
        public Adrenaline(GameContext context) : base(context)
        {
            Modifiers.Add(Context.ModifierManager.CreateModifier(ModifierKey.ITEM_CANNOT_BE_STOLEN));
        }
        public override string Name { get; set; } = "Адреналин";
        public override string Description => "Вы забираете выбранный предмет себе.\n" +
                                              "Запрещено применять на: Адреналин, Глина.\n" +
                                              $"Предмет появится в Вашем инвентаре спустя {Context.Settings.SHOW_ACTION_TIME / 1000} сек. после удачного применения.\n" +
                                              "Украденный предмет исчезнет в конце хода, если его не использовать.";
        public override string Lore => "Не пойман - не вор.";
        public override string Model => "adrenaline";
        public override ItemBehavior[] Behavior { get; } = [ItemBehavior.UNIT_TARGET];
        public override ItemTargetType TargetType => ItemTargetType.ITEM;
        public override ItemTargetTeam TargetTeam => ItemTargetTeam.ENEMY;
        public override ModifierState[] IgnoreTargetState { get; } = [ModifierState.ITEM_CANNOT_BE_STOLEN];
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "adrenaline/heartbeat"},
            {ItemEvent.EFFECTED, "adrenaline/steal"},
            {ItemEvent.CANCELED, "adrenaline/slap"}
        };

        internal override void BeforeUse(EventData e)
        {
            Item item = (Item)e.Special["TARGET_ITEM"];
            if (item.Is(ModifierState.ITEM_CANNOT_BE_STOLEN))
            {
                Disallow(e, $"{Name} нельзя применить на {item.Name}");
            }
        }
        public override void Effect(EventData e)
        {
            Item item = (Item)e.Special["TARGET_ITEM"];

            e.Target!.RemoveItem(item);
            Context.EventManager.Trigger(Event.ITEM_STOLEN, e);
            TimerExtension.SetTimeout(() =>
            {
                e.Initiator!.AddItem(item);
                var modifier = Context.ModifierManager.CreateModifier(ModifierKey.ITEM_ADRENALINE);
                modifier.Apply(e.Initiator, item);
                modifier.RemoveWhen(Event.ITEM_USED, item, (useE) =>
                {
                    return useE.Special["ITEM"] == item;
                });
                modifier.Remove(Event.TURN_CHANGED, item, (_) =>
                {
                    e.Initiator.RemoveItem(item);
                });
            }, Context.Settings.SHOW_ACTION_TIME);
        }
    }
}
