namespace SharedKernel.Interfaces;

public interface IDatabaseMemoryCache
{
    void SetSmallCacheWithLongLifeSpan<T>(string key, T cacheEntry);

    void SetSmallCacheWithShortLifeSpan<T>(string key, T cacheEntry);

    void SetMediumCacheWithLongLifeSpan<T>(string key, T cacheEntry);

    void SetMediumCacheWithShortLifeSpan<T>(string key, T cacheEntry);

    void SetMediumCacheWithVeryShortLifeSpan<T>(string key, T cacheEntry);

    void SetLargeCacheWithShortLifeSpan<T>(string key, T cacheEntry);

    /// <summary>
    /// Determine whether specified cache entry available.
    /// Returns False if expired or not available.
    /// </summary>
    bool HasCache<T>(string key, out T cacheEntry);

    /// <summary>
    /// Remove specific cache
    /// </summary>
    void RemoveCache(string key);
}