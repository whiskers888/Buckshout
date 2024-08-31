using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Buckshout.Managers
{
    public class CacheManager
    {
        private readonly IDistributedCache _cache;
        public CacheManager(IDistributedCache cache, ApplicationContext applicationContext)
        {
            _cache = cache;
        }
        internal async Task SetCache(string id, UserConnection userConnection)
        {
            var userData = JsonSerializer.Serialize(userConnection);
            await _cache.SetStringAsync(id, userData);
        }

        internal async Task<UserConnection> GetCache(string id)
        {
            var userData = await _cache.GetStringAsync(id);
            if (userData is null) return null;
            return JsonSerializer.Deserialize<UserConnection>(userData);
        }

        internal async Task UpdateCache(string id, UserConnection userData)
        {
            var cache = await GetCache(id);
            if (cache is not null)
            {
                await RemoveCache(id);
                await SetCache(id, userData);
            }

        }
        internal async Task RemoveCache(string id)
        {
            await _cache.RemoveAsync(id);
        }

        /*internal async Task RemoveCache(IClientProxy groups, string userIdentifier, Action remove)
        {
            await _cache.RemoveAsync(connectionId);
        }*/
    }
}
