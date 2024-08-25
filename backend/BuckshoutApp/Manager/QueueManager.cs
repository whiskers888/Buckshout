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
                if (e.Target == Current)
                    Next();
            });
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public IDisposable? Timer { get; set; } = null;

        public void CheckWinner()
        {
            if (Context.PlayerManager.AlivePlayers.Count == 0)
            {
                var e = new EventData();
                e.Special.Add("MESSAGE", "Ничья! Что-ж, за то никому не придется платить...");
                Context.EventManager.Trigger(Event.MESSAGE, e);
                Context.FinishGame();
            }
            else
            {
                var team = Context.PlayerManager.AlivePlayers[0].Team;
                if (Context.PlayerManager.AlivePlayers.Where(it => it.Team == team).Count() == Context.PlayerManager.AlivePlayers.Count)
                {
                    Context.EventManager.Trigger(Event.PLAYER_WON, new EventData()
                    {
                        Target = Context.PlayerManager.Players.FirstOrDefault(it => !it.Is(ModifierState.PLAYER_DEAD))
                    });
                }
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
            foreach (var modifier in player.Modifiers)
            {
                if (modifier.State.Contains(ModifierState.PLAYER_TURN_TIME_LIMITED))
                {
                    turnDuration /= modifier.Value;
                }
            }
            Context.EventManager.Trigger(Event.TURN_CHANGED, new Items.EventData()
            {
                Target = player,
                Special = new Dictionary<string, object>
                {
                    { "TIME", turnDuration }
                }
            });

            if (player.Is(ModifierState.PLAYER_STUNNED))
            {
                Context.EventManager.Trigger(Event.TURN_SKIPPED, new Items.EventData()
                {
                    Target = player,
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
                        Target = player
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
