using BuckshoutApp.Context;
using BuckshoutApp.Items;

namespace BuckshoutApp.Manager.Events
{

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
        public EventManager(GameContext context)
        {
            Context = context;

            UniqEvents = new Dictionary<Event, List<Tuple<string, Action<EventData>>>>();
            UnlimitedEvents = new Dictionary<Event, List<Action<EventData>>>();
            OnEventActions = new List<Action<Event, EventData>>();
            DisposableEvents = new Dictionary<Event, List<Action<EventData>>>();
        }
        public Dictionary<Event, List<Action<EventData>>> UnlimitedEvents { get; set; }

        public Dictionary<Event, List<Tuple<string, Action<EventData>>>> UniqEvents { get; set; }

        public Dictionary<Event, List<Action<EventData>>> DisposableEvents { get; set; }

        public List<Action<Event, EventData>> OnEventActions { get; set; }

        public void OnEvent(Action<Event, EventData> action)
        {
            OnEventActions.Add(action);
        }
        public void Once(Event e, Action<EventData> action)
        {
            if (!DisposableEvents.ContainsKey(e))
                DisposableEvents.Add(e, new List<Action<EventData>>());
            DisposableEvents[e].Add(action);
        }
        public void Subscribe(Event e, Action<EventData> action)
        {
            if (!UnlimitedEvents.ContainsKey(e))
                UnlimitedEvents.Add(e, new List<Action<EventData>>());
            UnlimitedEvents[e].Add(action);
        }
        public string SubscribeUniq(Event e, Action<EventData> action)
        {
            string Id = Guid.NewGuid().ToString();
            if (!UniqEvents.ContainsKey(e))
                UniqEvents.Add(e, new List<Tuple<string, Action<EventData>>>());
            UniqEvents[e].Add(new Tuple<string, Action<EventData>>(Id, action));
            return Id;
        }
        public void Trigger(Event e, EventData? eventData = null)
        {
            OnEventActions.ForEach(action => action(e, eventData));
            if (eventData == null)
                eventData = new EventData();

            if (UnlimitedEvents.ContainsKey(e))
                UnlimitedEvents[e].ForEach(it => it(eventData));
            if (UniqEvents.ContainsKey(e))
                UniqEvents[e].ForEach(it => it.Item2.Invoke(eventData));
            if (DisposableEvents.ContainsKey(e))
            {
                DisposableEvents[e].ForEach(it => it(eventData));
                DisposableEvents[e].Clear();
            }
        }

        public void Remove(Event e, EventData? eventData)
        {
            OnEventActions.ForEach(action => action(e, eventData));
            if (!UnlimitedEvents.ContainsKey(e)) return;
            if (eventData == null)
                eventData = new EventData();
            UnlimitedEvents[e].ForEach(it => it(eventData));
        }
        public void Unsubscribe(Event e, string id)
        {
            Tuple<string, Action<EventData>> uniqEvent = UniqEvents[e].FirstOrDefault(it => it.Item1 == id);
            UniqEvents[e].Remove(uniqEvent);
        }

    }
}
