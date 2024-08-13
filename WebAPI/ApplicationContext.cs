using Buckshout.Managers;

namespace BuckshoutApp.Context
{
    public class ApplicationContext
    {
        internal static readonly Dictionary<string, string> Sessions = new Dictionary<string, string>();
        public ApplicationContext(IConfiguration config)
        {
            Version = "0.9999-beta2";
            Title = "Buckshout";
            Configuration = config;
            Initialize();
        }
        public void Initialize()
        {
            RoomManager = new RoomManager();
        }

        public RoomManager RoomManager { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
