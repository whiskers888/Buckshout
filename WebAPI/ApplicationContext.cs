using Buckshout.Managers;

namespace BuckshoutApp.Context
{
    public class ApplicationContext 
    {
        public ApplicationContext(IConfiguration config)
        {
            Version = "0.1";
            Title = "HealthAPI";
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
