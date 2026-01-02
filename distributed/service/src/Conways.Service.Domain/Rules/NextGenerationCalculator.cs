using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Rules;

/// <summary>
/// Service that applies the Game of Life rules to produce the next grid state.
/// </summary>
public sealed class NextGenerationCalculator
{
    private readonly AliveNeighborCounter _aliveNeighborCounter;

    public NextGenerationCalculator(AliveNeighborCounter aliveNeighborCounter)
    {
        _aliveNeighborCounter = aliveNeighborCounter;
    }

    /// <summary>
    /// Evaluates the current grid and returns a new Grid based on survival and birth rules.
    /// </summary>
    public Grid Calculate(Grid currentGrid)
    {
        var nextCells = currentGrid.Cells
            .Select((row, rowIndex) =>
                row.Select((cell, columnIndex) =>
                {
                    var aliveNeighborsCount = _aliveNeighborCounter.Count(currentGrid, rowIndex, columnIndex);

                    var isAlive = cell == CellState.Alive;
                    var survives = isAlive && (aliveNeighborsCount == 2 || aliveNeighborsCount == 3);
                    var becomesAlive = !isAlive && aliveNeighborsCount == 3;

                    return survives || becomesAlive
                        ? CellState.Alive
                        : CellState.Dead;
                }).ToList()
            ).ToList();

        return new Grid(nextCells);
    }
}