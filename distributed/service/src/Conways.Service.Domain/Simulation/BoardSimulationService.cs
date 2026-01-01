using System.Security.Cryptography;
using System.Text;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Rules;

namespace Conways.Service.Domain.Simulation;

public sealed class BoardSimulationService
{
    private readonly NextGenerationCalculator _nextGenerationCalculator;

    public BoardSimulationService(NextGenerationCalculator nextGenerationCalculator)
    {
        _nextGenerationCalculator = nextGenerationCalculator;
    }

    public SimulationResult SimulateUntilConclusion(BoardState initialState, int maxIterations)
    {
        var visitedStateHashes = new HashSet<string>();
        var currentState = initialState;

        for (var iteration = 0; iteration < maxIterations; iteration++)
        {
            var currentStateHash = ComputeStateHash(currentState.Grid);

            if (!visitedStateHashes.Add(currentStateHash))
            {
                return new SimulationResult
                (
                    currentState,
                    SimulationTerminationReason.OscillationDetected
                );
            }

            var nextGrid = _nextGenerationCalculator.Calculate(currentState.Grid);

            if (AreGridsEqual(currentState.Grid, nextGrid))
            {
                return new SimulationResult
                (
                    currentState,
                    SimulationTerminationReason.StableStateReached
                );
            }

            currentState = new BoardState
            (
                grid: nextGrid,
                generation: currentState.Generation + 1
            );
        }

        throw new InvalidOperationException($"Simulation did not reach a conclusion after {maxIterations} iterations.");
    }

    private static bool AreGridsEqual(Grid first, Grid second)
        => first.Cells
            .SelectMany(row => row)
            .SequenceEqual(second.Cells.SelectMany(row => row));

    private static string ComputeStateHash(Grid grid)
    {
        var flattenedCells = grid.Cells
            .SelectMany(row => row)
            .Select(cell => ((int)cell).ToString());

        var rawRepresentation = string.Join(",", flattenedCells);

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(
            Encoding.UTF8.GetBytes(rawRepresentation));

        return Convert.ToBase64String(hashBytes);
    }
}