using System.Security.Cryptography;
using System.Text;
using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.Simulation;

namespace Conways.Service.Application.Boards.SimulateUntilConclusion;

public sealed class SimulateUntilConclusionHandler : ICommandHandler<SimulateUntilConclusionCommand, SimulateUntilConclusionResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly NextGenerationCalculator _nextGenerationCalculator;

    public SimulateUntilConclusionHandler
    (
        IBoardRepository boardRepository,
        NextGenerationCalculator nextGenerationCalculator
    )
    {
        _boardRepository = boardRepository;
        _nextGenerationCalculator = nextGenerationCalculator;
    }

    public async Task<SimulateUntilConclusionResult> HandleAsync
    (
        SimulateUntilConclusionCommand command,
        CancellationToken cancellationToken
    )
    {
        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        var visitedStatesHashes = new HashSet<string>();
        var currentState = board.CurrentState;

        for (var iteration = 0; iteration < command.MaxIterations; iteration++)
        {
            var currentStateHash = ComputeStateHash(currentState.Grid);

            if (!visitedStatesHashes.Add(currentStateHash))
            {
                return new SimulateUntilConclusionResult
                (
                    new SimulationResult
                    (
                        currentState,
                        SimulationTerminationReason.OscillationDetected
                    )
                );
            }

            var nextGrid = _nextGenerationCalculator.Calculate(currentState.Grid);

            if (AreGridsEqual(currentState.Grid, nextGrid))
            {
                return new SimulateUntilConclusionResult
                (
                    new SimulationResult
                    (
                        currentState,
                        SimulationTerminationReason.StableStateReached
                    )
                );
            }

            currentState = new BoardState
            (
                grid: nextGrid,
                generation: currentState.Generation + 1
            );
        }

        throw new InvalidOperationException($"Simulation did not reach a conclusion after {command.MaxIterations} iterations.");
    }

    private static bool AreGridsEqual(Grid first, Grid second)
        => first.Cells
            .SelectMany(row => row)
            .SequenceEqual(second.Cells.SelectMany(row => row));

    private static string ComputeStateHash(Grid grid)
    {
        var flattened = grid.Cells
            .SelectMany(row => row)
            .Select(cell => ((int)cell).ToString());

        var raw = string.Join(",", flattened);

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));

        return Convert.ToBase64String(bytes);
    }
}