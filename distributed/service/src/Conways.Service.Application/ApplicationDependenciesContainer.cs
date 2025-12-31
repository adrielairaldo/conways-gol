using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Boards.AdvanceBoard;
using Conways.Service.Application.Boards.CreateBoard;
using Conways.Service.Application.Boards.GetBoard;
using Conways.Service.Application.Boards.SimulateUntilConclusion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conways.Service.Application;

public static class ApplicationDependenciesContainer
{
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Boards
        services.AddTransient<ICommandHandler<AdvanceBoardCommand, AdvanceBoardResult>, AdvanceBoardHandler>();
        services.AddTransient<ICommandHandler<CreateBoardCommand, CreateBoardResult>, CreateBoardHandler>();
        services.AddTransient<IQueryHandler<GetBoardQuery, GetBoardResult>, GetBoardHandler>();
        services.AddTransient<ICommandHandler<SimulateUntilConclusionCommand, SimulateUntilConclusionResult>, SimulateUntilConclusionHandler>();

        return services;
    }
}