using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Dynamic;

namespace Buckshout.Hubs
{

    public interface IClient
    {
        public Task ReceiveMessage (string userName, object data);
        public Task ExceptionMessage (string data);

        public Task RoomCreated (object data);
        public Task GameStarted(object data);

        public Task Damage(object data);
        public Task Health(object data);

        public Task Shoot(object data);
        public Task MovePass(object data);
    }
    public class BaseHub :Hub <IClient>
    {
        internal dynamic GetCommon() => new ExpandoObject();
        internal async Task Send(string roomName, object? data = null, string taskName = "WALL-E")
        {
            await Clients.Groups(roomName)
                .ReceiveMessage(taskName, new JsonResult(new
                {
                    data,
                    datetime = DateTime.Now.ToString()
                })
            );
        }
    }
}
