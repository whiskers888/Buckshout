using Buckshout.Managers;
using BuckshoutApp.Context;
using BuckshoutApp.Manager;

namespace Buckshout.Models
{
    public class GameModel
    {
        public GameModel(GameContext context)
        {
            id = context.Id;
            players = context.PlayerManager.Players.Select(it => new PlayerModel(it)).ToArray();
        }

        public string id { get; set; }

        public PlayerModel[] players { get; set; } = [];

    }
}
