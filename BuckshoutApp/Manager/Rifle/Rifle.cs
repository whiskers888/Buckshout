using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Manager.Rifle
{
    public class Patron(bool IsCharged)
    {
        public bool IsCharged { get; set; } = IsCharged;
    }

    public class Rifle
    {
        private readonly GameContext Context;
        public Rifle(GameContext context)
        {
            Context = context;
            Patrons = [];
            Modifiers = [];

            Context.EventManager.Subcribe(Event.RIFLE_EMPTIED, (_) =>
            {
                Context.StartRound();
            });
            Context.EventManager.Subcribe(Event.RIFLE_PULLED, (_) =>
            {
                if (Patrons.Count == 0)
                {
                    Context.EventManager.Trigger(Event.RIFLE_EMPTIED);
                }
            });
        }
        public List<Patron> Patrons { get; private set; }
        public List<RifleModifier> Modifiers { get; set; }
        public int Damage => Modifiers.Contains(RifleModifier.DOUBLE_DAMAGE) ? 2 : 1;
        public void LoadRifle()
        {
            int countPatrons = Context.Random.Next(2, Context.Settings.MAX_PATRONS_IN_RIFLE);
            List<Patron> createdPatrons = [new Patron(true), new Patron(false)];
            for (int i = 0; i < countPatrons - 2; i++)
            {
                createdPatrons.Add(new Patron(Context.Random.Next(2) == 1));
            }
            Patrons.AddRange(createdPatrons);
            Patrons.Shuffle();
            Context.EventManager.Trigger(Event.RIFLE_LOADED, new Items.EventData()
            {
                special = {
                    {"CHARGED", Patrons.Where(it => it.IsCharged == true).Count() },
                    {"COUNT", Patrons.Count }
                }
            });
        }

        public bool NextPatron()
        {
            Patron patron = Patrons[^1]/*Patrons[Patrons.Count - 1]*/;
            Patrons.Remove(patron);
            return patron.IsCharged;
        }

        public void Shoot(Player targetPlayer)
        {
            Player currentPlayer = Context.QueueManager.Current;

            EventData e = new()
            {
                initiator = currentPlayer,
                target = targetPlayer,
            };

            bool IsCharged = NextPatron();
            if (IsCharged && targetPlayer == currentPlayer)
            {
                targetPlayer.ChangeHealth(ChangeHealthType.Damage, Damage, currentPlayer);
                Context.QueueManager.Next();
            }
            else if (IsCharged && targetPlayer != currentPlayer)
            {
                targetPlayer.ChangeHealth(ChangeHealthType.Damage, Damage, targetPlayer);
                Context.QueueManager.Next(targetPlayer);
            }
            else if (!IsCharged && targetPlayer == currentPlayer)
                Context.QueueManager.Next(currentPlayer);
            else if (!IsCharged && targetPlayer != currentPlayer)
                Context.QueueManager.Next(targetPlayer);

            e.special.Add("IS_CHARGED", IsCharged);

            Context.EventManager.Trigger(Event.RIFLE_SHOT, e);

            Context.EventManager.Trigger(Event.RIFLE_PULLED, e);
        }
    }
}
