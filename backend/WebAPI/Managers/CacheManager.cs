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
        internal async Task SetCache(string userIdentifier, UserConnection userConnection)
        {
            var userData = JsonSerializer.Serialize(userConnection);
            await _cache.SetStringAsync(userIdentifier, userData);
        }

        internal async Task<UserConnection> GetCache(string userIdentifier)
        {
            var userData = await _cache.GetStringAsync(userIdentifier);
            if (userData is null) return null;
            return JsonSerializer.Deserialize<UserConnection>(userData);
        }

        internal async Task UpdateCache(string userIdentifier, UserConnection userData)
        {
            var cache = await GetCache(userIdentifier);
            if (cache is not null)
            {
                await RemoveCache(userIdentifier);
                await SetCache(userIdentifier, userData);
            }

        }
        internal async Task RemoveCache(string userIdentifier)
        {
            await _cache.RemoveAsync(userIdentifier);
        }

        /*internal async Task RemoveCache(IClientProxy groups, string userIdentifier, Action remove)
        {
            await _cache.RemoveAsync(connectionId);
        }*/
    }
}
