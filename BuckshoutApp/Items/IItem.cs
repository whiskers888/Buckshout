using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp.Items
{
    public interface IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.Action Action { get; set; }

        public void Use() { }

    }
}
