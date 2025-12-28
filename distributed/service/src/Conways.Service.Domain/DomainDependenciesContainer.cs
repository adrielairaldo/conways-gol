using Conways.Service.Domain.Rules;

using Microsoft.Extensions.DependencyInjection;

namespace Conways.Service.Domain;

public static class DomainDependenciesContainer
{
    public static IServiceCollection AddDomainLayerServices(this IServiceCollection services)
    {
        // Rules:
        services.AddSingleton<AliveNeighborCounter>();
        services.AddSingleton<NextGenerationCalculator>();

        return services;
    }
}