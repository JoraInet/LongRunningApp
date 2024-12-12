using LongRunningApp.Infrastructure.Models;
using LongRunningApp.Shared.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LongRunningApp.Infrastructure.Services;
public sealed class CacheService(
    ILogger<CacheService> logger,
    IOptions<InfrastructureLayerSettings> options,
    IDistributedCache cache) : ICacheService
{
    private readonly InfrastructureLayerSettings _infrastructureLayerSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    public async Task<string> ReadFromCacheAsync(string cacheKey)
    {
        if (logger.LogErrorIfNullOrWhiteSpace(cacheKey, nameof(cache)) || !_infrastructureLayerSettings.UseRedisCache)
        {
            return string.Empty;
        }

        return await cache.GetStringAsync(cacheKey) ?? string.Empty;
    }

    public async Task WriteToCacheAsync(string cacheKey, string value)
    {
        if (logger.LogErrorIfNullOrWhiteSpace(cacheKey, nameof(cacheKey)) 
            || logger.LogErrorIfNullOrWhiteSpace(value, nameof(value))
            || !_infrastructureLayerSettings.UseRedisCache)
        {
            return;
        }

        await cache.SetStringAsync(cacheKey, value);
    }
}
