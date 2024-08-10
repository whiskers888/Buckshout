using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp.Manager
{
    public class QueueManager
    {
        private GameContext Context;
        public QueueManager(GameContext context)
        {
            Context = context;
            Queue = Context.PlayerManager.Players;
            Current = Queue.First();
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
                int index = Queue.IndexOf(Current);
                Player? nextPlayer = Queue.FirstOrDefault(it => it != Current && it != player);
                if(nextPlayer != null)
                    Next(nextPlayer);
            }
            else
                Current = player;
        }
        public void SkipPlayer(Player player)
        {
            if (!SkipPlayers.Contains(player))
                SkipPlayers.Add(player);
        }
    }
}
