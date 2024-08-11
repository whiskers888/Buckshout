using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using BuckshoutApp.Context;

namespace BuckshoutApp.Manager
{
    public enum Event
    {
        OnCancelItem = 0,
        OnTimeLimit = 1,
        OnDeath = 2,
        OnDamageTake = 3,
    }

    public class EventModel
    {
        public EventModel(Event e, string id)
        {
            Event = e;
            ID = id;
        }
        public Event Event { get; set; }
        public string ID { get; set; }
    }
    public class EventManager
    {
        public GameContext Context;
        public EventManager (GameContext context)
        {
            Context = context;
            EventActions = new Dictionary<EventModel, List<Action>> ();
        }
        public Dictionary<EventModel, List<Action>> EventActions { get; set; }
        public string CancelId { get; set; } = Guid.NewGuid().ToString();
        public void Subcribe(EventModel e, Action action) 
        {
            if (!EventActions.ContainsKey(e))
                EventActions.Add(e, new List<Action>());
            EventActions[e].Add(action);
        }
        public void ChangeCancelId() => CancelId = Guid.NewGuid().ToString();
        public void Trigger (EventModel e)
        {
            var @event = EventActions.FirstOrDefault(it => it.Key.ID == e.ID && it.Key.Event == e.Event);
            @event.Value.ForEach(it =>
            {
                it.Invoke();
            });
             
            @event.Value.Clear();
        }
    }
}
