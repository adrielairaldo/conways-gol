using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Boards.CreateBoard;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conways.Service.Application;

public static class ApplicationDependenciesContainer
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Boards
        services.AddTransient<ICommandHandler<CreateBoardCommand, CreateBoardResult>, CreateBoardHandler>();

        return services;
    }
}