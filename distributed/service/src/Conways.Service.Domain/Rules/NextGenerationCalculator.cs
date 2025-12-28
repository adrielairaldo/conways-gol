using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Rules;

public sealed class NextGenerationCalculator
{
    private readonly AliveNeighborCounter _aliveNeighborCounter;

    public NextGenerationCalculator(AliveNeighborCounter aliveNeighborCounter)
    {
        _aliveNeighborCounter = aliveNeighborCounter;
    }

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