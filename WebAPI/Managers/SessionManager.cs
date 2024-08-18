namespace Buckshout.Managers
{
    public class SessionManager
    {
        public Dictionary<string, string> Sessions { get; set; } = [];

        public string AddSession(string connectionId)
        {
            var id = Guid.NewGuid().ToString();
            Sessions.Add(id, connectionId);

            return id;
        }

        public void UpdateSession(string connectionId, string sessionId) => Sessions[sessionId] = connectionId;
        public void RemoveSession(string connectionId) => Sessions.Remove(connectionId);
    }
}
