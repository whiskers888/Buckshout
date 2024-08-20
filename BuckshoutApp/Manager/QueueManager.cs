using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;
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

            Context.EventManager.Subscribe(Event.PLAYER_LOST, (e) =>
            {
                if (e.target == Current)
                    Next();
            });
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public IDisposable? Timer { get; set; } = null;

        public void CheckWinner()
        {
            if (Context.PlayerManager.AlivePlayers.Count == 1)
            {
                Context.EventManager.Trigger(Event.PLAYER_WON, new EventData()
                {
                    target = Context.PlayerManager.Players.FirstOrDefault(it => !it.Is(ModifierState.PLAYER_DEAD))
                });
            }
            else if (Context.PlayerManager.AlivePlayers.Count == 0)
            {
                var e = new EventData();
                e.special.Add("MESSAGE", "Ничья! Что-ж, за то никому не придется платить...");
                Context.EventManager.Trigger(Event.MESSAGE, e);
                Context.FinishGame();
            }
        }
        public void Next(Player player)
        {
            Timer?.Dispose();

            CheckWinner();
            if (Context.Status == GameStatus.FINISHED) return;

            if (player.Is(ModifierState.PLAYER_DEAD))
            {
                Current = player;
                Next();
                return;
            }

            var turnDuration = Context.Settings.MAX_TURN_DURATION;
            foreach (var modirier in player.Modifiers)
            {
                if (modirier.State.Contains(ModifierState.PLAYER_TURN_TIME_LIMITED))
                {
                    turnDuration /= modirier.Value;
                }
            }
            Context.EventManager.Trigger(Event.TURN_CHANGED, new Items.EventData()
            {
                target = player,
                special = new Dictionary<string, object>
                {
                    { "TIME", turnDuration }
                }
            });

            if (player.Is(ModifierState.PLAYER_STUNNED))
            {
                Context.EventManager.Trigger(Event.TURN_SKIPPED, new Items.EventData()
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
                    Context.EventManager.Trigger(Event.TURN_EXPIRED, new Items.EventData()
                    {
                        target = player
                    });
                    Next();
                }, turnDuration);
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
