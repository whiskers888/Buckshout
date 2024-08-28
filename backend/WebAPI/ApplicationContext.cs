using Buckshout.Managers;
using Microsoft.Extensions.Caching.Distributed;

namespace BuckshoutApp.Context
{
    public class ApplicationContext
    {
        public ApplicationContext(IConfiguration config, IDistributedCache cache)
        {
            Version = "1.0";
            Title = "Buckshout";
            Configuration = config;

            RoomManager = new RoomManager(this);
            CacheManager = new CacheManager(cache, this);
        }


        public CacheManager CacheManager { get; set; }
        public RoomManager RoomManager { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
