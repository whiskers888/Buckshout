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

            var sec = 1000;
            var timeChecker = 0;
            timeChecker = TimerExtension.SetInterval(() =>
            {
                TimeLeft -= sec;
                if (context.Status == GameStatus.FINISHED)
                    TimerExtension.ClearInterval(timeChecker);
            }, sec);
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public int TimeLeft { get; set; } = 0;
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

        public Player ForcePlayer { get; set; }
        public void ForceNext(Player player)
        {
            ForcePlayer = player;
        }
        public void Next(Player player)
        {
            Timer?.Dispose();
            if (ForcePlayer != null)
            {
                player = ForcePlayer;
                ForcePlayer = null;
            }

            Console.WriteLine($"Ход передается от {Current.Name} к {player.Name}");
            CheckWinner();
            if (Context.Status == GameStatus.FINISHED) return;

            if (player.Is(ModifierState.PLAYER_DEAD))
            {
                Current = player;
                Next();
                return;
            }

            TimeLeft = Context.Settings.MAX_TURN_DURATION;
            foreach (var modifier in player.Modifiers)
            {
                if (modifier.State.Contains(ModifierState.PLAYER_TURN_TIME_LIMITED))
                {
                    TimeLeft /= modifier.Value;
                }
            }
            Context.EventManager.Trigger(Event.TURN_CHANGED, new EventData()
            {
                Target = player,
                Special = new Dictionary<string, object>
                {
                    { "TIME", TimeLeft }
                }
            });

            if (player.Is(ModifierState.PLAYER_STUNNED))
            {
                Console.WriteLine($"{player.Name}, был скован.");
                Context.EventManager.Trigger(Event.TURN_SKIPPED, new EventData()
                {
                    Target = player,
                });
                int currentPlayerIndex = Queue.IndexOf(Current);
                var aliveQueue = Queue.Where(p => !p.Is(ModifierState.PLAYER_DEAD));

                Player nextPlayer;
                if (aliveQueue.Count() > 2)
                    nextPlayer = aliveQueue.Skip(currentPlayerIndex).FirstOrDefault(it => it != Current && it != player) ?? aliveQueue.Where(it => it != Current && it != player).FirstOrDefault() ?? aliveQueue.First();
                else
                    nextPlayer = Current;

                if (Current != player)
                {
                    Current = player;
                    Next(nextPlayer);
                }
                else
                {
                    Next();
                }
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
                }, TimeLeft);
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
