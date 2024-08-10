using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp.Objects.rifle
{
    public class Rifle
    {
        public Rifle()
        {
            patrons = new List<Patron>();
        }
        private List<Patron> patrons;

        public void Charge()
        {
            
        }

        public void Pull() { }
        public void Shoot(int UUID) { }
    }
}
