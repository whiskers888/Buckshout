﻿
using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using BuckshoutApp.Manager.Events;
using System.Numerics;

namespace BuckshoutApp.Items
{

    public class EventData
    {
        public Player? initiator { get; set; }
        public Player? target { get; set; }
        public Dictionary<string,object> special { get; set; } = new Dictionary<string,object>();
    }
    public class Item(GameContext context)
    {
        public GameContext Context { get; } = context;
        public string UUID => Guid.NewGuid().ToString();
        public virtual string Name { get; } = "Default item name";
        public virtual string Description { get; } = "Default item description";
        public virtual ItemBehavior[] Behavior { get; } = { ItemBehavior.NO_TARGET };
        public virtual TargetType TargetType { get; } = TargetType.NONE;
        public virtual TargetTeam TargetTeam { get; } = TargetTeam.NONE;
        public ItemType ItemType { get; } = ItemType.DEFAULT;
        public ItemModifier[] ItemModifier { get; } = {     };
        public ItemState ItemState { get; set; } = ItemState.IN_BOX;


        public virtual void Effect(EventData e) { }
        internal virtual void BeforeUse(EventData e) { }
        internal virtual void BeforeCancel() { }
        public void Use(EventData e) 
        {
            ItemState = ItemState.USING;
            e.special.Add("ITEM", this);
            BeforeUse(e);
            Console.WriteLine($"{e.initiator?.Name} применяет {Name} на {e.target?.Name}");
            if (ItemState == ItemState.NOT_ALLOWED)
            {
                ItemState = ItemState.IN_HAND;
                return;
            }
            Context.EventManager.Trigger(Event.ITEM_USED, e);
            int timer= 0;
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
            }, Context.Settings.ITEM_CHANNELING_CHECK_INTERVAL);
        }

        public void Cancel()
        {
            Context.EventManager.Trigger(
                Event.ITEM_CANCELED, 
                new EventData() { special = new Dictionary<string, object>() { { "ITEM", this } } 
            });
            BeforeCancel();
            Console.WriteLine($"{Name} был отменен");
            ItemState = ItemState.CANCELED;
        }
    }
}

