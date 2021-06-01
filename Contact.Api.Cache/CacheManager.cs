using Contact.Api.Common.Conracts;
using System;

namespace Contact.Api.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IRedisClient redisClient;

        public CacheManager(IRedisClient iRedisClient)
        {
            redisClient = iRedisClient;
        }

        public T Get<T>(string key)
        {
           return redisClient.Get<T>(key);
        }

        public T Get<T>(string key, TimeSpan duration, Func<T> cacheFeed)
        {
            return redisClient.Get<T>(key,duration,cacheFeed);
        }

        public void Remove(string key)
        {
            redisClient.Remove(key);
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return redisClient.Set<T>(key, value, expiresAt);
        }
    }
}
