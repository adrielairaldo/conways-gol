using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Tests.TestData;

internal static class BasicGridGenerator
{
    public static Grid SimpleVerticalBlinker() => new
    (
        [
            [CellState.Dead, CellState.Alive, CellState.Dead],
            [CellState.Dead, CellState.Alive, CellState.Dead],
            [CellState.Dead, CellState.Alive, CellState.Dead]
        ]
    );

    public static Grid SimpleHorizontalBlinker() => new
    (
        [
            [CellState.Dead, CellState.Dead, CellState.Dead],
            [CellState.Alive, CellState.Alive, CellState.Alive],
            [CellState.Dead, CellState.Dead, CellState.Dead]
        ]
    );

    public static Grid StillLifeBlock() => new
    (
        [
            [CellState.Alive, CellState.Alive],
            [CellState.Alive, CellState.Alive]
        ]
    );

    public static Grid SingleAliveCellGrid() => new
    (
        [
            [CellState.Alive]
        ]
    );

    public static Grid SingleDeadCellGrid() => new
    (
        [
            [CellState.Dead]
        ]
    );
}