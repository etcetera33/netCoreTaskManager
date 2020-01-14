using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class RedisService : IRedisService
    {
        private readonly string _redisHost;
        private readonly int _redisPort;
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration config)
        {
            _redisHost = config["Redis:Host"];
            _redisPort = Convert.ToInt32(config["Redis:Port"]);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            var serialized = JsonConvert.SerializeObject(value);

            await _redis.GetDatabase().StringSetAsync(key, serialized);
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
                var configString = $"{_redisHost}:{_redisPort},connectRetry=5";
                _redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (RedisConnectionException err)
            {
                throw err;
            }
        }
    }
}
