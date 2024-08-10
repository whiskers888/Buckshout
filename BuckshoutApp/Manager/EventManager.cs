using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshoutApp.Manager
{
    public class EventManager
    {
        public GameContext Context;
        public EventManager (GameContext context)
        {
            Context = context;
        }
    }
}
