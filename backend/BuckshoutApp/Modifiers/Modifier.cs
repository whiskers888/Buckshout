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
    public class Modifier(GameContext context)
    {
        public GameContext Context { get; set; } = context;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Duration { get; set; } = 0;
        public string Icon { get; set; } = "";
        public bool IsBuff { get; set; } = false;
        public bool IsPurgableByItem { get; set; } = true;
        public bool IsPurgableByRound { get; set; } = true;
        public bool IsHidden { get; set; } = false;
        public int Value { get; set; } = 0;
        public int StackCount { get; set; } = 0;
        public Player Target { get; set; }
        public ModifierTargetType TargetType { get; set; }
        public List<ModifierState> State { get; set; } = [];
        public Action? OnApplied { get; set; }
        public Action? OnRemoved { get; set; }
        public bool Removed { get; private set; } = false;
        public void Apply(Player target)
        {
            Target = target;
            SetDuration();
            OnApplied?.Invoke();
            target.AddModifier(this);
        }
        public void Apply(Player initiator, Item item)
        {
            Target = initiator;
            SetDuration();
            OnApplied?.Invoke();
            item.AddModifier(this, initiator);
        }
        public void Apply(Player initiator, Rifle rifle)
        {
            Target = initiator;
            SetDuration();
            OnApplied?.Invoke();
            rifle.AddModifier(this, initiator);
        }
        public void SetRemoved()
        {
            Removed = true;
        }
        public void Remove(Event @event, Item item = null, Action<EventData>? callback = null)
        {

            Context.EventManager.Once(@event, (e) =>
            {
                if (!Removed)
                {
                    switch (TargetType)
                    {
                        case ModifierTargetType.PLAYER:
                            Target.RemoveModifier(this); break;

                        case ModifierTargetType.RIFLE:
                            Context.Rifle.RemoveModifier(this); break;

                        case ModifierTargetType.ITEM:
                            item.RemoveModifier(this, Target); break;
                    }
                    OnRemoved?.Invoke();
                    callback?.Invoke(e);
                }
                Removed = true;
            });
        }
        public void RemoveWhen(Event @event, Item item = null, Func<EventData, bool> checkState = null)
        {
            string id = "";
            id = Context.EventManager.SubscribeUniq(@event, (e) =>
            {
                if (!Removed)
                {

                    if (checkState != null && checkState.Invoke(e) != true) return;

                    switch (TargetType)
                    {
                        case ModifierTargetType.PLAYER:
                            Target.RemoveModifier(this); break;

                        case ModifierTargetType.RIFLE:
                            Context.Rifle.RemoveModifier(this); break;

                        case ModifierTargetType.ITEM:
                            item.RemoveModifier(this, Target); break;
                    }
                    OnRemoved?.Invoke();
                }

                Context.EventManager.Unsubscribe(@event, id);
                Removed = true;
            });
        }
        private void SetDuration()
        {
            string id = "";
            if (Duration >= 0)
            {
                id = Context.EventManager.SubscribeUniq(Event.TURN_CHANGED, (e) =>
                {
                    if (e.Target == Target)
                    {
                        if (Duration == 0)
                        {
                            Removed = true;
                            Target.RemoveModifier(this);
                            OnRemoved?.Invoke();
                            Context.EventManager.Unsubscribe(Event.TURN_CHANGED, id);
                        }
                        Duration--;
                    }
                });
            }
        }

        public string On(Event @event, Action<EventData> action)
        {
            return Context.EventManager.SubscribeUniq(@event, (e) =>
            {
                if (!Removed)
                    action(e);
            });
        }
    }
}

