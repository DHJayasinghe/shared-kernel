using Microsoft.Extensions.Caching.Memory;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.Helpers;

public sealed class DatabaseMemoryCache : IDatabaseMemoryCache
{
    private readonly MemoryCacheConfig _memoryConfig;
    private MemoryCache Cache { get; set; }

    public DatabaseMemoryCache(MemoryCacheConfig memoryConfig)
    {
        _memoryConfig = memoryConfig ?? throw new ArgumentNullException(nameof(memoryConfig));
        Cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = memoryConfig.SizeLimit,
        });
    }

    public void SetSmallCacheWithLongLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Set cache entry size
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.SmallCacheWithLongLifeSpan).AbsoluteExpiryInSecs))
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.SmallCacheWithLongLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.High);
        Cache.Set(key, cacheEntry, cacheEntryOptions); // Save data in cache.
    }

    public void SetSmallCacheWithShortLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.SmallCacheWithShortLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.Low);
        Cache.Set(key, cacheEntry, cacheEntryOptions); // Save data in cache.
    }

    public void SetMediumCacheWithLongLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(2)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.MediumCacheWithLongLifeSpan).AbsoluteExpiryInSecs))
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.MediumCacheWithLongLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.Normal);
        Cache.Set(key, cacheEntry, cacheEntryOptions);
    }

    public void SetMediumCacheWithShortLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(2)
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.MediumCacheWithShortLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.Normal);
        Cache.Set(key, cacheEntry, cacheEntryOptions);
    }

    public void SetMediumCacheWithVeryShortLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(2)
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.MediumCacheWithVeryShortLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.Normal);
        Cache.Set(key, cacheEntry, cacheEntryOptions);
    }

    public void SetLargeCacheWithShortLifeSpan<T>(string key, T cacheEntry)
    {
        if (!_memoryConfig.Enabled) return;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(5)
                .SetSlidingExpiration(TimeSpan.FromSeconds(_memoryConfig.Get(CacheSizeEnum.LargeCacheWithShortLifeSpan).SlidingExpiryInSecs))
                .SetPriority(CacheItemPriority.Low);
        Cache.Set(key, cacheEntry, cacheEntryOptions);
    }

    public bool HasCache<T>(string key, out T cacheEntry) => Cache.TryGetValue(key, out cacheEntry);

    public void RemoveCache(string key) => Cache.Remove(key);
}

public sealed class MemoryCacheConfig
{
    public bool Enabled { get; set; }
    public long SizeLimit { get; set; }
    public Dictionary<CacheSizeEnum, CacheLifeTime> ExpiryConfigs { get; set; }
    public CacheLifeTime Get(CacheSizeEnum size) => ExpiryConfigs[size];
}

public enum CacheSizeEnum
{
    SmallCacheWithLongLifeSpan = 1,
    SmallCacheWithShortLifeSpan = 2,
    MediumCacheWithLongLifeSpan = 3,
    MediumCacheWithShortLifeSpan = 4,
    MediumCacheWithVeryShortLifeSpan = 5,
    LargeCacheWithShortLifeSpan = 6,
}

public sealed class CacheLifeTime
{
    public double SlidingExpiryInSecs { get; set; }
    public double AbsoluteExpiryInSecs { get; set; }
}