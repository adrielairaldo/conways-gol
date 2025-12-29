using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Tests.TestData;

internal static class BasicGridGenerator
{
    public static Grid SimpleBlinker() => new(new List<List<CellState>>
    {
        new() { CellState.Dead, CellState.Alive, CellState.Dead },
        new() { CellState.Dead, CellState.Alive, CellState.Dead },
        new() { CellState.Dead, CellState.Alive, CellState.Dead }
    });

    public static Grid StillLifeBlock() => new(new List<List<CellState>>
    {
        new() { CellState.Alive, CellState.Alive },
        new() { CellState.Alive, CellState.Alive }
    });
}