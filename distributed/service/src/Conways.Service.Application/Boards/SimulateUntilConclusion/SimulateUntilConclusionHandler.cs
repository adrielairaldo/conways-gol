using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Simulation;

namespace Conways.Service.Application.Boards.SimulateUntilConclusion;

/// <summary>
/// Triggers a simulation to find the final state or pattern of a board.
/// </summary>
public sealed class SimulateUntilConclusionHandler : ICommandHandler<SimulateUntilConclusionCommand, SimulateUntilConclusionResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly BoardSimulationService _simulationService;

    public SimulateUntilConclusionHandler(IBoardRepository boardRepository, BoardSimulationService simulationService)
    {
        _boardRepository = boardRepository;
        _simulationService = simulationService;
    }

    public async Task<SimulateUntilConclusionResult> HandleAsync(SimulateUntilConclusionCommand command, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        var simulationResult = _simulationService.SimulateUntilConclusion(board.CurrentState, command.MaxIterations);

        return new SimulateUntilConclusionResult(simulationResult);
    }
}