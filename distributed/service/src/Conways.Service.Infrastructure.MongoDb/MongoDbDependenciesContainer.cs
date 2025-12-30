using Conways.Service.Domain.Repositories;
using Conways.Service.Infrastructure.MongoDb.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Conways.Service.Infrastructure.MongoDb;

public static class MongoDbDependenciesContainer
{
    /// <summary>
    /// Configures MongoDB-related services and health checks for an ASP.NET Core application.
    /// </summary>
    /// <param name="services">The IServiceCollection to configure.</param>
    /// <returns>The IServiceCollection with MongoDB-related services configured.</returns>
    public static IServiceCollection AddInfrastructureMongoDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Get MongoDB options from configuration
        var options = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;

        // Add MongoDB health check
        services.AddHealthChecks().AddMongoDb(options.ConnectionString);

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        //BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        // Add MongoDB client
        services.AddSingleton<IMongoClient>(_ => new MongoClient(options.ConnectionString));

        // Add MongoDB database
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        // Register MongoBoardRepository as the implementation for IBoardRepository
        services.AddScoped<IBoardRepository, MongoBoardRepository>();

        return services;
    }
}