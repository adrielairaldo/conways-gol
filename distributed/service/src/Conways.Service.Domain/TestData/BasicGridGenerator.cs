using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.TestData;

public static class BasicGridGenerator
{
    /* This class contains a set of grids used to avoid repetition and increase clarity
     * in test cases. Since it is used in different projects, to avoid repetition we
     * leave the class in the Domain project, making a pragmatic decision. */

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

    public static Grid Glider3x3() => new
    (
        [
            [CellState.Dead,  CellState.Alive, CellState.Dead ],
            [CellState.Dead,  CellState.Dead,  CellState.Alive],
            [CellState.Alive, CellState.Alive, CellState.Alive]
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