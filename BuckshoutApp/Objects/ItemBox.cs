using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuckshoutApp.Items;

namespace BuckshoutApp.Objects
{
    public class ItemBox
    {

        private List<Item> Items { get; set; }
        public Item Get()
        {
            Item item = Items.Last();
            Items.Remove(item);
            return item;
        }
    }
}
