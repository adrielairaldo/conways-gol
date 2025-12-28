namespace Conways.Service.Domain.Rules;

public static class NeighborDirections
{
    public static readonly IReadOnlyList<(int RowOffset, int ColumnOffset)> All =
    [
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1)
    ];
}