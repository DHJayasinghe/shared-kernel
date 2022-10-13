using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedKernel.Helpers;

namespace SharedKernel.UnitTest;

[TestClass]
public class ClientCredentialTokenCacheStoreUnitTest
{
    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly Mock<IConfidentialClientApplication> _confidentialClientApp = new();
    private Mock<ClientCredentialTokenCacheStore> _clientCacheStore;
    private ClientCredentialTokenCacheStore? _sut;

    private const int OffsetToTestCacheExpiryInSeconds = 5;
    private const int DefaultTokenExpiryInSeconds = ClientCredentialTokenCacheStore.TokenExpirationRenewalOffsetInSeconds + OffsetToTestCacheExpiryInSeconds;

    [TestInitialize]
    public void Initialize()
    {
        _clientCacheStore = new Mock<ClientCredentialTokenCacheStore>(_confidentialClientApp.Object, _memoryCache);
        _clientCacheStore
            .Setup(store => store.AcquireTokenForClientAsync(It.IsAny<string[]>()))
            .Returns(() => Task.FromResult(new AuthenticationResult(Guid.NewGuid().ToString(), false, Guid.NewGuid().ToString(),
                new DateTimeOffset(DateTime.UtcNow.AddSeconds(DefaultTokenExpiryInSeconds), TimeSpan.Zero),
                new DateTimeOffset(DateTime.UtcNow.AddSeconds(DefaultTokenExpiryInSeconds), TimeSpan.Zero),
                null, null, null, null, Guid.NewGuid())));
        _sut = _clientCacheStore.Object;
    }

    async Task<AuthenticationResult> GetStateAfterInitialTokenRequestAsync(string key)
    {
        await _sut.AcquireTokenForClient(new[] { key });
        _memoryCache.TryGetValue(key, out AuthenticationResult result);
        return result;
    }
    async Task<AuthenticationResult> GetStateAfterSecondTokenRequestAsync(string key)
    {
        await _sut.AcquireTokenForClient(new[] { key });
        _memoryCache.TryGetValue(key, out AuthenticationResult result);
        return result;
    }

    // Should_ExpectedBehavior_When_StateUnderTest
    [TestMethod]
    public async Task Should_TokenWithSameScopeGetCached_During_InitialRequest()
    {
        // Arrange
        string key = "sample-scope";
        AuthenticationResult GetStateBeforeInitialTokenRequest(string key)
        {
            _memoryCache.TryGetValue(key, out AuthenticationResult result);
            return result;
        }

        // Act
        var beforeInitialTokenRequestState = GetStateBeforeInitialTokenRequest(key);
        var initialTokenRequestState = await GetStateAfterInitialTokenRequestAsync(key);

        // Assert
        beforeInitialTokenRequestState.Should().BeNull();
        initialTokenRequestState.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Should_RetrieveSameTokenForSameScope_After_FirstTokenRequest()
    {
        string scope = "sample-scope";
        async Task<AuthenticationResult> GetStateAfterSecondTokenRequestAsync(string key)
        {
            await _sut.AcquireTokenForClient(new[] { key });
            _memoryCache.TryGetValue(key, out AuthenticationResult result);
            return result;
        }

        var initialTokenRequestState = await GetStateAfterInitialTokenRequestAsync(scope);
        var secondTokenRequestState = await GetStateAfterSecondTokenRequestAsync(scope);

        initialTokenRequestState.AccessToken.Should().BeEquivalentTo(secondTokenRequestState.AccessToken);
    }

    [TestMethod]
    public async Task Should_ReceiveNewTokenForSameScope_If_CachedTokenIsExpiredIn10mins()
    {
        string scope = "sample-scope";
        async Task<AuthenticationResult> GetStateAfterInitialTokenCacheExpiredAsync(string key)
        {
            await WaitUntilTheTokenCacheExpires();
            _memoryCache.TryGetValue(key, out AuthenticationResult result);
            return result;

            static async Task WaitUntilTheTokenCacheExpires() => await Task.Delay((int)TimeSpan.FromSeconds(OffsetToTestCacheExpiryInSeconds).TotalMilliseconds);
        }

        var initialTokenRequestState = await GetStateAfterInitialTokenRequestAsync(scope);
        var afterInitialTokenRequestState = await GetStateAfterInitialTokenCacheExpiredAsync(scope);
        var secondTokenRequestState = await GetStateAfterSecondTokenRequestAsync(scope);

        initialTokenRequestState.Should().NotBeNull();
        afterInitialTokenRequestState.Should().BeNull();
        secondTokenRequestState.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Should_ReceiveDifferentTokens_When_RequestUsingDifferentScopes()
    {
        string scope1 = "sample-scope";
        string scope2 = "sample-scope2";

        var initialTokenRequestState1 = await GetStateAfterInitialTokenRequestAsync(scope1);
        var initialTokenRequestState2 = await GetStateAfterInitialTokenRequestAsync(scope2);

        initialTokenRequestState1.AccessToken.Should().NotBeEquivalentTo(initialTokenRequestState2.AccessToken);
    }
}