using BuckshoutApp.Context;
using BuckshoutApp.Items;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

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

            Context.EventManager.Subscribe(Event.RIFLE_EMPTIED, (_) =>
            {
                Context.StartRound();
            });
            Context.EventManager.Subscribe(Event.RIFLE_PULLED, (_) =>
            {
                if (Patrons.Count == 0)
                {
                    Context.EventManager.Trigger(Event.RIFLE_EMPTIED);
                }
            });
        }
        public List<Patron> Patrons { get; private set; }

        public List<Modifier> Modifiers { get; set; }
        public int Damage => 1 + (GetModifier(ModifierState.RIFLE_BONUS_DAMAGE)?.Value ?? 0);
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
            Context.EventManager.Trigger(Event.RIFLE_LOADED, new EventData()
            {
                Special = {
                    {"CHARGED", Patrons.Where(it => it.IsCharged == true).Count() },
                    {"COUNT", Patrons.Count }
                }
            });
        }
        public Modifier GetModifier(ModifierState state) => Modifiers.FirstOrDefault(it => it.State.Contains(state));

        public bool NextPatron()
        {
            Patron patron = Patrons[^1];
            Patrons.Remove(patron);
            return patron.IsCharged;
        }

        public void Shoot(Player targetPlayer)
        {
            Player currentPlayer = Context.QueueManager.Current;

            EventData e = new()
            {
                Initiator = currentPlayer,
                Target = targetPlayer,
            };

            if (Patrons.Count == 0)
            {
                Context.EventManager.Trigger(Event.RIFLE_EMPTIED);
                return;
            }

            bool IsCharged = NextPatron();
            bool evasion = IsMissing(targetPlayer);
            if (IsCharged && targetPlayer == currentPlayer)
            {
                if (!evasion)
                {
                    targetPlayer.ChangeHealth(ChangeHealthType.Damage, Damage, currentPlayer, "RIFLE");
                    Context.QueueManager.Next();
                }
                else
                    Context.QueueManager.Next(currentPlayer);
            }
            else if (IsCharged && targetPlayer != currentPlayer)
            {
                if (!evasion)
                    targetPlayer.ChangeHealth(ChangeHealthType.Damage, Damage, targetPlayer, "RIFLE");
                Context.QueueManager.Next(targetPlayer);
            }
            else if (!IsCharged && targetPlayer == currentPlayer)
                Context.QueueManager.Next(currentPlayer);
            else if (!IsCharged && targetPlayer != currentPlayer)
                Context.QueueManager.Next(targetPlayer);

            e.Special.Add("IS_CHARGED", IsCharged);
            e.Special.Add("IS_MISSING", IsCharged && evasion);

            Context.EventManager.Trigger(Event.RIFLE_SHOT, e);
            Context.EventManager.Trigger(Event.RIFLE_PULLED, e);
        }

        public bool IsMissing(Player target)
        {
            if (target.Is(ModifierState.PLAYER_EVASION))
            {
                int chance = Context.Random.Next(0, 100);
                if (target.GetModifier(ModifierState.PLAYER_EVASION).Value >= chance) return true;
            }
            return false;
        }
        public void AddModifier(Modifier modifier, Player initiator)
        {
            Modifiers.Add(modifier);
            Context.EventManager.Trigger(Event.MODIFIER_APPLIED, new EventData()
            {
                Target = initiator,
                Special = { { "MODIFIER", modifier } }
            });
        }
        public void RemoveModifier(Modifier modifier)
        {
            if (Modifiers.Remove(modifier))
                Context.EventManager.Trigger(Event.MODIFIER_REMOVED, new EventData()
                {
                    Special = { { "MODIFIER", modifier } }
                });
        }
        public bool Is(ModifierState state)
        {
            foreach (var modifier in Modifiers)
            {
                if (modifier.State.Contains(state)) return true;
            }
            return false;
        }
    }
}
