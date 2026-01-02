using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Simulation;
using Microsoft.Extensions.Logging;

namespace Conways.Service.Application.Boards.SimulateUntilConclusion;

/// <summary>
/// Triggers a simulation to find the final state or pattern of a board.
/// </summary>
public sealed class SimulateUntilConclusionHandler : ICommandHandler<SimulateUntilConclusionCommand, SimulateUntilConclusionResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly BoardSimulationService _simulationService;
    private readonly ILogger<SimulateUntilConclusionHandler> _logger;

    public SimulateUntilConclusionHandler(IBoardRepository boardRepository, BoardSimulationService simulationService, ILogger<SimulateUntilConclusionHandler> logger)
    {
        _boardRepository = boardRepository;
        _simulationService = simulationService;
        _logger = logger;
    }

    public async Task<SimulateUntilConclusionResult> HandleAsync(SimulateUntilConclusionCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Simulating board {BoardId} until conclusion (Max iterations: {Max}).",
                                command.BoardId.Value, command.MaxIterations);

        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            _logger.LogWarning("Simulation failed: Board {BoardId} not found.", command.BoardId.Value);
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        var simulationResult = _simulationService.SimulateUntilConclusion(board.CurrentState, command.MaxIterations);

        _logger.LogInformation("Simulation for {BoardId} completed. Reason: {Reason}", 
                                command.BoardId.Value, simulationResult.TerminationReason);

        return new SimulateUntilConclusionResult(simulationResult);
    }
}