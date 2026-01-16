using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using MyNet.Application.Common.Cache;
using MyNet.Application.Common.Options;

namespace MyNet.Infrastructure.Services
{
    public class RedisCacheService : ICache
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly CacheOptions _option;
        public RedisCacheService(CacheOptions option, ILogger<RedisCacheService> logger)
        {
            _option = option;
            var configOptions = new ConfigurationOptions
            {
                EndPoints = { _option.Host },
                Password = _option.Password,
                Ssl = _option.Ssl,
                AbortOnConnectFail = _option.AbortOnConnectFail,
                DefaultDatabase = _option.Database
            };

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configOptions));
            _logger = logger;
        }

        ConnectionMultiplexer Connection => _lazyConnection.Value;

        public Task<bool> DeleteAsync(string key, int? database = null)
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis delete from db {databaseNumber}: {key}");
                return redisDatabase.KeyDeleteAsync(key);
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
            return Task.FromResult(false);
        }

        public async Task<T> GetAsync<T>(string key, int? database = null) where T : class
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis get from db {databaseNumber}: {key}");
                var value = await redisDatabase.StringGetAsync(key);
                if (!value.HasValue)
                {
                    return default!;
                }
                var (objResult, success) = FromByteArray<T>(value!);
                if (success)
                {
                    return objResult!;
                }
                
                return JsonSerializer.Deserialize<T>(value.ToString())!;
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
            return default!;
        }
        public async Task<string> GetStringAsync(string key, int? database = null)
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis get string from db {databaseNumber}: {key}");
                return (await redisDatabase.StringGetAsync(key))!;
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
            return null!;
        }

        public async Task StoreAsync<T>(string key, T value, int? database = null) where T : class
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis store to db {databaseNumber}: {key}");
                await redisDatabase.StringSetAsync(key, ToByteArray<T>(value), TimeSpan.FromSeconds(_option.CacheDuration));
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
        }
        public Task StoreAsync(string key, string value, int? database = null)
        {
            return StoreStringAsync(key, value, TimeSpan.FromSeconds(_option.CacheDuration), database);

        }

        public async Task StoreAsync<T>(string key, T value, TimeSpan duration, int? database = null) where T : class
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis store to db {databaseNumber}: {key}");
                await redisDatabase.StringSetAsync(key, ToByteArray<T>(value), duration);
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
        }

        public async Task<bool> DeleteAllKeysAsync(string parternKey, int? database = null)
        {
            try
            {
                foreach (var ep in Connection.GetEndPoints())
                {
                    var server = Connection.GetServer(ep);
                    var targetDatabase = database ?? _option.Database;
                    var redisDatabase = Connection.GetDatabase(targetDatabase);
                    var keys = server.Keys(targetDatabase, pattern: parternKey, int.MaxValue).ToArray();
                    if (keys.Any())
                    {
                        _logger.LogTrace($"Redis delete all keys from db {targetDatabase} ({string.Join(",", (IEnumerable<RedisKey>)keys)})");
                        await redisDatabase.KeyDeleteAsync(keys);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting redis cache key");
                return false;
            }
        }

        public static byte[] ToByteArray<T>(T obj) where T : class
        {
            if (obj == null)
                return Array.Empty<byte>();
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static (T?, bool) FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return (default(T), false);
            return (JsonSerializer.Deserialize<T>(data), true);
        }

        public async Task StoreStringAsync(string key, string value, TimeSpan duration, int? database = null)
        {
            try
            {
                var databaseNumber = database ?? _option.Database;
                var redisDatabase = Connection.GetDatabase(databaseNumber);
                _logger.LogTrace($"Redis string store to db {databaseNumber}: {key}");
                await redisDatabase.StringSetAsync(key, value, duration);
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc, exc.Message);
            }
        }
    }
}
