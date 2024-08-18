﻿using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{

    public class EventData
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public Player? initiator { get; set; }
        public Player? target { get; set; }
        public Dictionary<string, object> special { get; set; } = [];
    }
    public class Item(GameContext context)
    {
        public GameContext Context { get; } = context;
        public string Id { get; } = Guid.NewGuid().ToString();
        public virtual string Name { get; } = "UNKNOWN";
        public virtual string Description { get; } = "UNKNOWN";
        public virtual string Lore { get; } = "";
        public virtual string Model { get; } = "unknown";
        public virtual ItemBehavior[] Behavior { get; } = [ItemBehavior.NO_TARGET];
        public virtual TargetType TargetType { get; } = TargetType.NONE;
        public virtual TargetTeam TargetTeam { get; } = TargetTeam.NONE;
        public ItemType ItemType { get; } = ItemType.DEFAULT;
        public List<Modifier> Modifiers { get; } = [];
        public ItemState ItemState { get; set; } = ItemState.IN_BOX;


        public virtual void Effect(EventData e) { }
        internal virtual void BeforeUse(EventData e) { }
        internal virtual void BeforeCancel() { }
        public bool Use(EventData e)
        {
            ItemState = ItemState.USING;
            e.special.Add("ITEM", this);
            BeforeUse(e);
            Console.WriteLine($"{e.initiator?.Name} применяет {Name} на {e.target?.Name}");
            if (ItemState == ItemState.NOT_ALLOWED)
            {
                ItemState = ItemState.IN_HAND;
                return false;
            }
            Context.EventManager.Trigger(Event.ITEM_USED, e);
            int timer = 0;
            int progress = 0;

            timer = TimerExtension.SetInterval(() =>
            {
                if (ItemState == ItemState.DELAYED) return;
                else progress += Context.Settings.ITEM_CHANNELING_CHECK_INTERVAL;

                if (progress >= Context.Settings.ITEM_CHANNELING_CHECK_INTERVAL || ItemState == ItemState.CANCELED)
                {
                    if (ItemState == ItemState.USING)
                    {
                        Context.EventManager.Trigger(Event.ITEM_EFFECTED, e);
                        Effect(e);
                    }
                    ItemState = ItemState.REMOVED;
                    TimerExtension.ClearInterval(timer);
                }
            }, Context.Settings.ITEM_CHANNELING_TIME);
            return true;
        }
        public void Disallow(EventData e, string msg)
        {
            ItemState = ItemState.NOT_ALLOWED;
            e.special.Add("MESSAGE", msg);
            Context.EventManager.Trigger(Event.MESSAGE_RECEIVED, e);
        }
        public void Cancel()
        {
            Context.EventManager.Trigger(
                Event.ITEM_CANCELED,
                new EventData()
                {
                    special = new Dictionary<string, object>() { { "ITEM", this } }
                });
            BeforeCancel();
            Console.WriteLine($"{Name} был отменен");
            ItemState = ItemState.CANCELED;
        }

        public void AddModifier(Modifier modifier, Player initiator)
        {
            Modifiers.Add(modifier);
            Context.EventManager.Trigger(Event.MODIFIER_APPLIED, new EventData()
            {
                target = initiator,
                special = { { "MODIFIER", modifier }, { "ITEM", this } }
            });
        }
        public void RemoveModifier(Modifier modifier, Player target)
        {
            Modifiers.Remove(modifier);
            Context.EventManager.Trigger(Event.MODIFIER_REMOVED, new EventData()
            {
                target = target,
                special = { { "MODIFIER", modifier }, { "ITEM", this } }
            });
        }
        public bool Is(ModifierState state)
        {
            foreach (var modifier in Modifiers)
            {
                if (modifier.State.Contains(state)) return true;
            }
            return false;
        }
        /*public void ClearModifiers()
        {
            Modifiers.Clear();
        }*/
    }
}

