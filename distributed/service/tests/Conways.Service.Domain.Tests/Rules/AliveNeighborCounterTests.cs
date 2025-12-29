using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Rules;
using FluentAssertions;

namespace Conways.Service.Domain.Tests.Rules;

public sealed class AliveNeighborCounterTests
{
    private readonly AliveNeighborCounter _aliveNeighborCounter = new();

    [Fact]
    public void Count_ShouldReturnZero_WhenCellHasNoAliveNeighbors()
    {
        // Arrange
        var grid = new Grid(new List<IReadOnlyList<CellState>>
        {
            new List<CellState> { CellState.Dead, CellState.Dead, CellState.Dead },
            new List<CellState> { CellState.Dead, CellState.Dead, CellState.Dead },
            new List<CellState> { CellState.Dead, CellState.Dead, CellState.Dead }
        });

        const int targetRow = 1;
        const int targetColumn = 1;

        // Act
        var aliveNeighborsCount = _aliveNeighborCounter.Count(grid, targetRow, targetColumn);

        // Assert
        aliveNeighborsCount.Should().Be(0);
    }

    [Fact]
    public void Count_ShouldReturnCorrectNumber_WhenSomeNeighborsAreAlive()
    {
        // Arrange
        var grid = new Grid(new List<IReadOnlyList<CellState>>
        {
            new List<CellState> { CellState.Alive, CellState.Dead,  CellState.Alive },
            new List<CellState> { CellState.Dead,  CellState.Dead,  CellState.Dead  },
            new List<CellState> { CellState.Alive, CellState.Dead,  CellState.Dead  }
        });

        const int targetRow = 1;
        const int targetColumn = 1;

        // Act
        var aliveNeighborsCount = _aliveNeighborCounter.Count(grid, targetRow, targetColumn);

        // Assert
        aliveNeighborsCount.Should().Be(3);
    }

    [Fact]
    public void Count_ShouldIgnoreCellsOutsideGridBounds()
    {
        // Arrange
        var grid = new Grid(new List<IReadOnlyList<CellState>>
        {
            new List<CellState> { CellState.Alive, CellState.Alive },
            new List<CellState> { CellState.Alive, CellState.Dead  }
        });

        const int targetRow = 0;
        const int targetColumn = 0;

        // Act
        var aliveNeighborsCount = _aliveNeighborCounter.Count(grid, targetRow, targetColumn);

        // Assert
        aliveNeighborsCount.Should().Be(2);
    }

    [Fact]
    public void Count_ShouldNotCountTargetCellItself()
    {
        // Arrange
        var grid = new Grid(new List<IReadOnlyList<CellState>>
        {
            new List<CellState> { CellState.Alive, CellState.Alive, CellState.Alive },
            new List<CellState> { CellState.Alive, CellState.Alive, CellState.Alive },
            new List<CellState> { CellState.Alive, CellState.Alive, CellState.Alive }
        });

        const int targetRow = 1;
        const int targetColumn = 1;

        // Act
        var aliveNeighborsCount = _aliveNeighborCounter.Count(grid, targetRow, targetColumn);

        // Assert
        aliveNeighborsCount.Should().Be(8);
    }
}