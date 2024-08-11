
using BuckshoutApp.Context;

namespace BuckshoutApp.Manager
{
    public class QueueManager
    {
        private GameContext Context;
        public QueueManager(GameContext context)
        {
            Context = context;
            Queue = Context.PlayerManager.Players;
            Queue.Shuffle();
            Current = Context.PlayerManager.Players.First();
            SkipPlayers = new List<Player>();
        }
        public List<Player> Queue { get; set; }
        public Player Current { get; set; }
        public List<Player> SkipPlayers { get; set; }

        public void Next(Player player)
        {
            if (SkipPlayers.Contains(player))
            {
                SkipPlayers.Remove(player);
                int currentPlayerIndex = Queue.IndexOf(Current);
                Player? nextPlayer = Queue.Skip(currentPlayerIndex).FirstOrDefault(it => it != Current && it != player);
                if (nextPlayer != null)
                    Next(nextPlayer);
                else
                    Next(Queue[0]);
            }
            else
                Current = player;
        }
        public void Next()
        {
            int nextPlayerIndex = Queue.IndexOf(Current)+1;
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
