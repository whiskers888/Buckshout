﻿using BuckshoutApp.Context;

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
            SkipPlayers = [];
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public List<Player> SkipPlayers { get; set; }
        public IDisposable? timer { get; set; } = null;
        public void Next(Player player)
        {
            timer?.Dispose();
            if (SkipPlayers.Contains(player))
            {
                Context.EventManager.Trigger(Events.Event.TURN_SKIPPED, new Items.EventData()
                {
                    target = player,
                });
                SkipPlayers.Remove(player);
                int currentPlayerIndex = Queue.IndexOf(Current);
                Player? nextPlayer = Queue.Skip(currentPlayerIndex).First(it => it != Current && it != player);
                if (nextPlayer != null)
                    Next(nextPlayer);
                else
                    Next(Queue[0]);
            }
            else
                Current = player;
            Context.EventManager.Trigger(Events.Event.TURN_CHANGED, new Items.EventData()
            {
                target = Context.QueueManager.Current,
                special = new Dictionary<string, object>
                {
                    { "TIME", Context.Settings.MAX_TURN_DURATION }
                }
            });
            timer = TimerExtension.SetTimeout(() =>
            {
                if (Context.Status == GameStatus.FINISHED) return;
                Context.EventManager.Trigger(Events.Event.TURN_EXPIRED, new Items.EventData());
                Next();
            }, Context.Settings.MAX_TURN_DURATION);

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
        public void SkipPlayer(Player player)
        {
            if (!SkipPlayers.Contains(player))
                SkipPlayers.Add(player);
        }
    }
}
