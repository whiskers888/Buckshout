using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Manager.Rifle;

namespace BuckshoutApp.Modifiers
{
    public enum ModifierTargetType
    {
        PLAYER,
        RIFLE,
        ITEM
    }
    public class Modifier
    {
        public GameContext Context { get; set; }
        public Modifier(GameContext context)
        {
            Context = context;
            OnApplied?.Invoke();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public Player Target { get; set; }
        public string Description { get; set; } = "";
        // public Dictionary<ModifierFunction, object> Functions { get; set; } = [];
        public int Duration { get; set; } = 0;
        public string Icon { get; set; } = "";
        public bool IsBuff { get; set; } = false;
        public int Value { get; set; } = 0;
        public int StackCount { get; set; } = 0;
        public ModifierTargetType TargetType { get; set; }
        public List<ModifierState> State { get; set; } = [];
        public Action? OnApplied { get; set; }
        public void Apply(Player target)
        {
            Target = target;
            string id = "";
            SetDuration(target);
            OnApplied?.Invoke();
            Target.AddModifier(this);
        }
        public void Apply(Player initiator, Item item)
        {
            Target = initiator;
            string id = "";
            SetDuration(initiator);
            OnApplied?.Invoke();
            item.AddModifier(this, initiator);
        }
        public void Apply(Player initiator, Rifle rifle)
        {
            Target = initiator;
            SetDuration(initiator);
            OnApplied?.Invoke();
            rifle.AddModifier(this, initiator);
        }
        public void Remove(Event @event, Player target, object entity = null)
        {
            Context.EventManager.Once(@event, (e) =>
            {
                switch (TargetType)
                {
                    case ModifierTargetType.PLAYER:
                        target.RemoveModifier(this); break;

                    case ModifierTargetType.RIFLE:
                        Context.Rifle.RemoveModifier(this); break;

                    case ModifierTargetType.ITEM:
                        ((Item)entity).RemoveModifier(this, target); break;
                }
            });
        }
        public void RemoveWhen(Event @event, Player target, object entity = null, Func<EventData, bool> checkState = null)
        {
            Context.EventManager.Subscribe(@event, (e) =>
            {
                if (checkState?.Invoke(e) != true) return;
                switch (TargetType)
                {
                    case ModifierTargetType.PLAYER:
                        target.RemoveModifier(this); break;

                    case ModifierTargetType.RIFLE:
                        Context.Rifle.RemoveModifier(this); break;

                    case ModifierTargetType.ITEM:
                        ((Item)entity).RemoveModifier(this, target); break;
                }
            });
        }
        private void SetDuration(Player target)
        {

            string id = "";
            if (Duration > 0)
            {
                id = Context.EventManager.SubscribeUniq(Manager.Events.Event.TURN_CHANGED, (e) =>
                {
                    if (e.target.Id == target.Id)
                    {
                        if (Duration == 0)
                        {
                            Target.RemoveModifier(this);

                            // Возможно здесь id будет пустой
                            Context.EventManager.Unsubscribe(Manager.Events.Event.TURN_CHANGED, id);
                        }
                        Duration--;
                    }
                });
            }
        }
        public void On()
        {

        }
    }
}

