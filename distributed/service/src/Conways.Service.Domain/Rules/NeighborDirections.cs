namespace Conways.Service.Domain.Rules;

/// <summary>
/// Provides the relative coordinates for all 8 neighbors of a cell.
/// </summary>
public static class NeighborDirections
{
    public static readonly IReadOnlyList<(int RowOffset, int ColumnOffset)> All =
    [
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1)
    ];
}