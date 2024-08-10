﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public Patron[] CreatePatron()
        {
            int countPatrons = Context.Random.Next(2, 6);
            List<Patron> createdPatrons = new List<Patron>();
            for (int i = 0; i < countPatrons; i++)
            {
                createdPatrons.Add(new Patron());
            }
            patrons.AddRange(createdPatrons);
            Shuffle();
            return createdPatrons.ToArray();
        }
        private void Shuffle()
        {
            for (int i = patrons.Count - 1; i > 0; i--)
            {
                int j = Context.Random.Next(0, i + 1);

                Patron temp = patrons[i];
                patrons[i] = patrons[j];
                patrons[j] = temp;
            }
        }

        public bool NextPatron()
        {
            Patron patron = patrons[patrons.Capacity - 1];
            bool shoot = patron.IsCharged;
            patrons.Remove(patron);
            return shoot;

        }

    }
}
