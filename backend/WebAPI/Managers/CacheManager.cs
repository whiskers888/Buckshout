using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Buckshout.Managers
{
    public class CacheManager
    {
        private readonly IDistributedCache _cache;

        /*private readonly ApplicationContext ApplicationContext;*/
        public CacheManager(IDistributedCache cache, ApplicationContext applicationContext)
        {
            _cache = cache;
        }
        internal async Task SetCache(string connectionId, UserConnection userConnection)
        {
            var userData = JsonSerializer.Serialize(userConnection);
            await _cache.SetStringAsync(connectionId, userData);
        }

        internal async Task<UserConnection> GetCache(string connectionId)
        {
            var userData = await _cache.GetStringAsync(connectionId);
            if (userData is null) return null;
            return JsonSerializer.Deserialize<UserConnection>(userData);
        }

        internal async Task UpdateCache(string connectionId, UserConnection userData)
        {
            var cache = await GetCache(connectionId);
            if (cache is not null)
            {
                await RemoveCache(connectionId);
                await SetCache(connectionId, userData);
            }

        }
        internal async Task RemoveCache(string connectionId)
        {
            await _cache.RemoveAsync(connectionId);
        }

        /*internal async Task RemoveCache(IClientProxy groups, string userIdentifier, Action remove)
        {
            await _cache.RemoveAsync(connectionId);
        }*/
    }
}
