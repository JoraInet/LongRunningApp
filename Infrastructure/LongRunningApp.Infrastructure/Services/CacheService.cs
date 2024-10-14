using LongRunningApp.Infrastructure.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace LongRunningApp.Infrastructure.Services;
public sealed class CacheService : ICacheService
{
    private readonly InfrastructureLayerSettings _infrastructureLayerSettings;
    private readonly IDistributedCache _cache;

    public CacheService(
        IOptions<InfrastructureLayerSettings> options,
        IDistributedCache cache)
    {
        _infrastructureLayerSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<string> ReadFromCacheAsync(string cacheKey)
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new ArgumentNullException(nameof(cacheKey));
        }

        if (!_infrastructureLayerSettings.UseRedisCache)
        {
            return string.Empty;
        }

        return await _cache.GetStringAsync(cacheKey) ?? string.Empty;
    }

    public async Task WriteToCacheAsync(string cacheKey, string value)
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new ArgumentException($"'{nameof(cacheKey)}' cannot be null or whitespace.", nameof(cacheKey));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
        }

        if (!_infrastructureLayerSettings.UseRedisCache)
        {
            return;
        }

        await _cache.SetStringAsync(cacheKey, value);
    }
}
