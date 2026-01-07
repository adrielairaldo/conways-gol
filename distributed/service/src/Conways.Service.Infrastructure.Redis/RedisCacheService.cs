using System.Text.Json;

using Conways.Service.Domain.Repositories;

using Microsoft.Extensions.Caching.Distributed;


namespace Conways.Service.Infrastructure.Redis;

public sealed class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var cachedValue = await _cache.GetStringAsync(key, cancellationToken);

        return cachedValue is null
            ? default
            : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken)
    {
        var serialized = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        await _cache.SetStringAsync(key, serialized, options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken) => _cache.RemoveAsync(key, cancellationToken);
}
