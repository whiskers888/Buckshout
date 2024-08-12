using BuckshoutApp.Context;

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

        public bool Shoot(Player targetPlayer)
        {
            Player currentPlayer = Context.QueueManager.Current;
            bool IsCharged = NextPatron();
            if (IsCharged && targetPlayer == currentPlayer)
            {
                Context.PlayerManager.SetHealth(DirectionHealth.Down, Damage, currentPlayer);
                Context.QueueManager.Next();
            }
            else if (IsCharged && targetPlayer != currentPlayer)
            {
                Context.PlayerManager.SetHealth(DirectionHealth.Down, Damage, targetPlayer);
                Context.QueueManager.Next(targetPlayer);
            }
            else if (!IsCharged && targetPlayer == currentPlayer)
                Context.QueueManager.Next(currentPlayer);
            else if (!IsCharged && targetPlayer != currentPlayer)
                Context.QueueManager.Next(targetPlayer);

            Context.EventManager.Trigger(Events.Event.RIFLE_SHOT, new Items.EventData() { 
                                                        initiator = currentPlayer, 
                                                        target = targetPlayer, 
                                                        special = new Dictionary<string, object>() { { "IS_CHARGED", IsCharged } } });
            return IsCharged;


        }
    }
}
