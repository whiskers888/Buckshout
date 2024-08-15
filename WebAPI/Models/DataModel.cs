using BuckshoutApp.Items;

namespace Buckshout.Models
{
    public class DataModel
    {
        public DataModel(EventData? e)
        {
            special = [];
            if (e == null) return;

            if (e.target is not null)
                target = new PlayerModel(e.target);
            if (e.initiator is not null)
                initiator = new PlayerModel(e.initiator);
            foreach (var kv in e.special)
            {
                if (kv.Value is Item item)
                    special.Add(kv.Key, new ItemModel(item));
                else
                    special.Add(kv.Key, kv.Value);
            };
        }
        public PlayerModel? target { get; set; }
        public PlayerModel? initiator { get; set; }
        public Dictionary<string, object> special { get; set; }
    }
}
