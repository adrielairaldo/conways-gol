namespace Conways.Service.Domain.Boards;

public sealed class Grid
{
    public IReadOnlyList<IReadOnlyList<CellState>> Cells { get; }

    public int TotalRows => Cells.Count;
    public int TotalColumns => Cells[0].Count;

    public Grid(IReadOnlyList<IReadOnlyList<CellState>> cells)
    {
        Cells = cells;
    }

    public CellState GetCell(int row, int column)
        => Cells[row][column];
}