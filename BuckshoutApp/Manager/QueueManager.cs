using BuckshoutApp.Context;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Manager
{
    public class QueueManager
    {
        private readonly GameContext Context;
        public QueueManager(GameContext context)
        {
            Context = context;
            Queue = Context.PlayerManager.Players;
            Queue.Shuffle();
            Current = Context.PlayerManager.Players.First();
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public IDisposable? Timer { get; set; } = null;
        public void Next(Player player)
        {
            Timer?.Dispose();
            if (player.Is(ModifierState.PLAYER_DEAD))
            {
                Next();
                return;
            }
            Context.EventManager.Trigger(Events.Event.TURN_CHANGED, new Items.EventData()
            {
                target = player,
                special = new Dictionary<string, object>
                {
                    { "TIME", Context.Settings.MAX_TURN_DURATION }
                }
            });
            if (player.Is(ModifierState.PLAYER_STUNNED))
            {
                Context.EventManager.Trigger(Events.Event.TURN_SKIPPED, new Items.EventData()
                {
                    target = player,
                });
                int currentPlayerIndex = Queue.IndexOf(Current);

                Player nextPlayer;
                if (Queue.Count > 2)
                    nextPlayer = Queue.Skip(currentPlayerIndex).FirstOrDefault(it => it != Current && it != player) ?? Queue[0];
                else
                    nextPlayer = Current;

                Next(nextPlayer);
            }
            else
            {
                Current = player;
                Timer = TimerExtension.SetTimeout(() =>
                {
                    if (Context.Status == GameStatus.FINISHED) return;
                    Context.EventManager.Trigger(Events.Event.TURN_EXPIRED, new Items.EventData()
                    {
                        target = player
                    });
                    Next();
                }, Context.Settings.MAX_TURN_DURATION);
            }
        }
        public void Next()
        {
            int nextPlayerIndex = Queue.IndexOf(Current) + 1;
            if (nextPlayerIndex > Queue.Count - 1)
                nextPlayerIndex = 0;
            Player nextPlayer = Queue[nextPlayerIndex];
            if (nextPlayer != null)
                Next(nextPlayer);
        }
    }
}
