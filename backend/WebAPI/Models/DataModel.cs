﻿using BuckshoutApp.Items;
using BuckshoutApp.Modifiers;

namespace Buckshout.Models
{
    public class DataModel
    {
        public DataModel(EventData? e)
        {
            Special = [];
            if (e == null) return;

            if (e.Target is not null)
                Target = new PlayerModel(e.Target);
            if (e.Initiator is not null)
                Initiator = new PlayerModel(e.Initiator);
            foreach (var kv in e.Special)
            {
                if (kv.Value is Item item)
                    Special.Add(kv.Key, new ItemModel(item));
                else if (kv.Value is Modifier modifier)
                    Special.Add(kv.Key, new ModifierModel(modifier));
                else
                    Special.Add(kv.Key, kv.Value);
            };
        }
        public PlayerModel? Target { get; set; }
        public PlayerModel? Initiator { get; set; }
        public Dictionary<string, object> Special { get; set; }
    }
}