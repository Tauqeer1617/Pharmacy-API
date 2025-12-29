using StackExchange.Redis;

namespace Pharmacy.Infrastructure.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }
    }
}
