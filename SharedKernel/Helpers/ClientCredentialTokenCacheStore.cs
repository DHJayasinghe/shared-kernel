using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace SharedKernel.Helpers;

public class ClientCredentialTokenCacheStore : IClientCredentialTokenCacheStore
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfidentialClientApplication _app;
    public const int TokenExpirationRenewalOffsetInSeconds = 300;

    public ClientCredentialTokenCacheStore(IConfidentialClientApplication app, IMemoryCache memoryCache)
    {
        _app = app ?? throw new ArgumentNullException(nameof(app));
        _memoryCache = memoryCache;
    }

    public async Task<AuthenticationResult> AcquireTokenForClient(string[] scopes)
    {
        string key = string.Join(",", scopes);
        if (!_memoryCache.TryGetValue(key, out AuthenticationResult cacheValue))
        {
            cacheValue = await AcquireTokenForClientAsync(scopes);
            var tokenExpiry = cacheValue.ExpiresOn.ToUniversalTime();
            var currentTime = DateTime.UtcNow;

            var tokenShouldEpireInSecs = tokenExpiry.Subtract(currentTime).TotalSeconds - TokenExpirationRenewalOffsetInSeconds;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(tokenShouldEpireInSecs))
                .SetSlidingExpiration(TimeSpan.FromSeconds(tokenShouldEpireInSecs));
            _memoryCache.Set(key, cacheValue, cacheEntryOptions);
        }

        return cacheValue;
    }

    public virtual async Task<AuthenticationResult> AcquireTokenForClientAsync(string[] scopes) =>
        await _app.AcquireTokenForClient(scopes).ExecuteAsync();
}

public interface IClientCredentialTokenCacheStore
{
    Task<AuthenticationResult> AcquireTokenForClient(string[] scopes);
}