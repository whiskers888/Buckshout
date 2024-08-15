using BuckshoutApp.Context;
using BuckshoutApp.Manager;
using System.Dynamic;

namespace Buckshout.Models
{
    public class GameModel
    {
        public GameModel(GameContext context)
        {
            var props = typeof(Settings).GetProperties();
            foreach (var prop in props)
            {
                object value = prop.GetValue(context.Settings, null) ?? "";
                Settings.Add(prop.Name, value);
            }
            Players = context.PlayerManager.Players.Select(it => new PlayerModel(it)).ToArray();
            Status = context.Status;
            Id = context.Id;
        }
        public string Id { get; set; }

        public PlayerModel[] Players { get; set; }

        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

        public GameStatus Status { get; set; }

    }
}
