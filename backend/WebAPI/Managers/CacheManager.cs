using Buckshout.Models;
using BuckshoutApp.Context;
using Microsoft.AspNetCore.SignalR;
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
        internal async Task SetCache(string connectionId, string roomName = "")
        {
            var stringConnection = JsonSerializer.Serialize(new UserConnection(connectionId, roomName));
            await _cache.SetStringAsync(connectionId, stringConnection);
        }

        internal async Task<UserConnection> GetCache(string connectionId)
        {
            var stringConnection = await _cache.GetAsync(connectionId);
            if (stringConnection is null) return null;
            return JsonSerializer.Deserialize<UserConnection>(stringConnection);
            /*else
                throw new Exception("Невозможно достать из кэша подключение пользователя");*/
        }

        internal async Task UpdateCache(UserConnection conn)
        {
            var cache = await GetCache(conn.connectionId);
            if (cache is not null)
            {
                await RemoveCache(conn.connectionId);
                await SetCache(conn.connectionId, conn.roomName);
            }

        }

        private async Task RemoveCache(string connectionId)
        {
            await _cache.RemoveAsync(connectionId);
        }

        internal async Task RemoveCache(IClientProxy groups, string connectionId, Action remove)
        {
            await _cache.RemoveAsync(connectionId);
        }
    }
}
