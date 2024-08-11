using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuckshoutApp.Context;
using BuckshoutApp.Objects.rifle;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuckshoutApp.Manager
{
    public class Patron
    {
        public bool IsCharged { get; set; }
    }

    public class RifleManager
    {
        private GameContext Context;
        public RifleManager(GameContext context)
        {
            Context = context;
            patrons = new List<Patron>();
        }
        private List<Patron> patrons { get; set; }
        public Patron[] Patrons => patrons.ToArray();
        public int Damage = 1;

        public Patron[] CreatePatrons()
        {
            int countPatrons = Context.Random.Next(2, 6);
            List<Patron> createdPatrons = new List<Patron>();
            for (int i = 0; i < countPatrons; i++)
            {
                createdPatrons.Add(new Patron() { 
                    IsCharged = Context.Random.Next(2) == 1 
                });
            }
            patrons.AddRange(createdPatrons);
            patrons.Shuffle();
            return createdPatrons.ToArray();
        }

        public bool NextPatron()
        {
            Patron patron = patrons[patrons.Count - 1];
            bool shoot = patron.IsCharged;
            patrons.Remove(patron);
            return shoot;

        }

        public bool Shoot(Player target)
        {
            Player currentPlayer = Context.QueueManager.Current;
            bool IsCharged = NextPatron();
            if (IsCharged && target == currentPlayer)
            {
                Context.PlayerManager.SetHealth(DirectionHealth.Down, Damage, currentPlayer);
                Context.QueueManager.Next();
            }
            else if (IsCharged && target != currentPlayer)
            {
                Context.PlayerManager.SetHealth(DirectionHealth.Down, Damage, target);
                Context.QueueManager.Next(target);
            }
            else if (!IsCharged && target == currentPlayer)
                Context.QueueManager.Next(currentPlayer);
            else if (!IsCharged && target != currentPlayer)
                Context.QueueManager.Next(target);
            return IsCharged;

        }
    }
}
