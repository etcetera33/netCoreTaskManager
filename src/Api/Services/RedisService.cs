using Core.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class RedisService : IRedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly IOptions<RedisConfig> _options;

        public RedisService(IOptions<RedisConfig> options)
        {
            _options = options;
        }

        public async Task SetItemAsync<T>(string key, T value, int expirySeconds)
        {
            var serialized = JsonConvert.SerializeObject(value);

            await _redis.GetDatabase().StringSetAsync(key, serialized, TimeSpan.FromSeconds(expirySeconds));
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var serializedData = await _redis.GetDatabase().StringGetAsync(key);

            if (serializedData == default)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(serializedData);
        }

        public void Connect()
        {
            try
            {
                _redis = ConnectionMultiplexer.Connect(_options.Value.ConnectionString);
            }
            catch (RedisConnectionException err)
            {
                throw err;
            }
        }
    }
}
