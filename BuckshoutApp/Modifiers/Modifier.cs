using BuckshoutApp.Context;
using BuckshoutApp.Manager;

namespace BuckshoutApp.Modifiers
{
    public class Modifier
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public GameContext Context { get; set; }
        public Modifier(GameContext context)
        {
            Context = context;
            OnCreated.Invoke();
        }
        public string Name { get; set; } = "";
        public Player? Target { get; set; }
        public string Description { get; set; } = "";
        public Dictionary<ModifierFunction, object> Functions { get; set; } = new Dictionary<ModifierFunction, object>();
        public int Duration { get; set; } = 0;
        public string Icon { get; set; } = "";
        public bool IsBuff { get; set; } = false;
        public Action OnCreated { get; set; }
    }

    public class ItemModifier : Modifier
    {
        public ItemModifier(GameContext context) : base(context) { }


        public List<ItemModifierState> State { get; set; } = [];
    }
    public class PlayerModifier : Modifier
    {
        public PlayerModifier(GameContext context) : base(context)
        {
            Context = context;
            string id = "";
            if (Duration > 0)
            {
                id = Context.EventManager.SubscribeUniq(Manager.Events.Event.TURN_CHANGED, (e) =>
                {
                    if (e.target.Id == Target.Id)
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

            OnCreated.Invoke();
        }

        public List<PlayerModifierState> State { get; set; } = [];
    }
    public class RifleModifier : Modifier
    {
        public RifleModifier(GameContext context) : base(context)
        {
        }

        public List<RifleModifierState> State { get; set; } = [];
    }
}

