using Conways.Service.Domain.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conways.Service.Infrastructure.Redis;

public static class RedisDependenciesContainer
{
    /// <summary>
    /// Configures Redis-related services and health checks for an ASP.NET Core application.
    /// </summary>
    /// <param name="services">The IServiceCollection to configure.</param>
    /// <returns>The IServiceCollection with MongoDB-related services configured.</returns>
    public static IServiceCollection AddInfrastructureRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Get Redis options from configuration
        var options = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()!;

        // Add Redis caching service
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = options.ConnectionString;
        });

        // Add Redis health check
        services.AddHealthChecks().AddRedis(options.ConnectionString);

        // Register RedisCacheService as the implementation for ICacheService
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}