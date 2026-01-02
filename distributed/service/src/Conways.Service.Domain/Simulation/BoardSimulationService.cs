using System.Security.Cryptography;
using System.Text;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Rules;

namespace Conways.Service.Domain.Simulation;

/// <summary>
/// Orchestrates the simulation process, running generations until a conclusion is reached.
/// </summary>
public sealed class BoardSimulationService
{
    private readonly NextGenerationCalculator _nextGenerationCalculator;

    public BoardSimulationService(NextGenerationCalculator nextGenerationCalculator)
    {
        _nextGenerationCalculator = nextGenerationCalculator;
    }

    /// <summary>
    /// Runs the simulation from an initial state until it stabilizes, oscillates, or hits the iteration limit.
    /// </summary>
    public SimulationResult SimulateUntilConclusion(BoardState initialState, int maxIterations)
    {
        var visitedStateHashes = new HashSet<string>();
        var currentState = initialState;

        for (var iteration = 0; iteration < maxIterations; iteration++)
        {
            var currentStateHash = ComputeStateHash(currentState.Grid);

            // Check for patterns that repeat (Oscillation)
            if (!visitedStateHashes.Add(currentStateHash))
            {
                return new SimulationResult
                (
                    currentState,
                    SimulationTerminationReason.OscillationDetected
                );
            }

            var nextGrid = _nextGenerationCalculator.Calculate(currentState.Grid);

            // Check if the grid has stopped changing (Stable State)
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

        return new SimulationResult
        (
            currentState,
            SimulationTerminationReason.MaxIterationsExceeded
        );
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