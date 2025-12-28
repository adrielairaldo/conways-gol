using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Rules;

public sealed class AliveNeighborCounter
{
    public int Count(Grid grid, int targetRow, int targetColumn)
    {
        var totalRows = grid.TotalRows;
        var totalColumns = grid.TotalColumns;

        return NeighborDirections.All.Aggregate
        (
            seed: 0,
            func: (aliveCount, direction) =>
            {
                var neighborRow = targetRow + direction.RowOffset;
                var neighborColumn = targetColumn + direction.ColumnOffset;

                var isInsideGrid =
                    neighborRow >= 0 &&
                    neighborRow < totalRows &&
                    neighborColumn >= 0 &&
                    neighborColumn < totalColumns;

                return isInsideGrid && grid.GetCell(neighborRow, neighborColumn) == CellState.Alive
                    ? aliveCount + 1
                    : aliveCount;
            }
        );
    }
}