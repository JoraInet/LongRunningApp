
namespace LongRunningApp.Infrastructure.Services;
public interface ICacheService
{
    Task<string> ReadFromCacheAsync(string cacheKey);
    Task WriteToCacheAsync(string cacheKey, string value);
}