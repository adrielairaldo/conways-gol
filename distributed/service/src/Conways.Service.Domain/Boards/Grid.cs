namespace Conways.Service.Domain.Boards;

/// <summary>
/// Represents the two-dimensional layout of cells.
/// </summary>
public sealed class Grid
{
    public IReadOnlyList<IReadOnlyList<CellState>> Cells { get; }
    public int TotalRows => Cells.Count;
    public int TotalColumns => Cells[0].Count;

    public Grid(IReadOnlyList<IReadOnlyList<CellState>> cells)
    {
        EnsureGridIsNotEmpty(cells);
        EnsureAllRowsHaveSameLength(cells);
        Cells = cells;
    }

    public CellState GetCell(int row, int column) => Cells[row][column];

    private static void EnsureGridIsNotEmpty(IReadOnlyList<IReadOnlyList<CellState>> cells)
    {
        if (cells is null || cells.Count == 0)
        {
            throw new ArgumentException("Grid must contain at least one row.", nameof(cells));
        }
    }

    private static void EnsureAllRowsHaveSameLength(IReadOnlyList<IReadOnlyList<CellState>> cellsMatrix)
    {
        var expectedColumnCount = cellsMatrix[0].Count;

        var hasInvalidRow = cellsMatrix.Any(row => row.Count != expectedColumnCount);

        if (hasInvalidRow)
        {
            throw new ArgumentException
            (
                "All rows in the grid must have the same number of columns.",
                nameof(cellsMatrix)
            );
        }
    }
}