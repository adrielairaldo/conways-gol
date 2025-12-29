using Conways.Service.Domain.Boards;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Boards;

public sealed class GridTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenGridIsNull()
    {
        // Arrange
        IReadOnlyList<IReadOnlyList<CellState>>? nullGrid = null;

        // Act
        var act = () => new Grid(nullGrid!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*at least one row*");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenGridHasNoRows()
    {
        // Arrange
        var emptyGrid = new List<IReadOnlyList<CellState>>();

        // Act
        var act = () => new Grid(emptyGrid);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*at least one row*");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenRowsHaveDifferentLengths()
    {
        // Arrange
        var invalidGrid = new[]
        {
            new[] { CellState.Dead, CellState.Alive },
            new[] { CellState.Alive }
        };

        // Act
        var act = () => new Grid(invalidGrid);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*same number of columns*");
    }

    [Fact]
    public void Constructor_ShouldCreateGrid_WhenInputIsValid()
    {
        // Arrange
        var cells = new[]
        {
            new[] { CellState.Dead, CellState.Alive },
            new[] { CellState.Alive, CellState.Dead }
        };


        // Act
        var grid = new Grid(cells);

        // Assert
        grid.TotalRows.Should().Be(2);
        grid.TotalColumns.Should().Be(2);
        grid.Cells.Should().BeEquivalentTo(cells);
    }

    [Fact]
    public void GetCell_ShouldReturnCorrectCellState_ForGivenCoordinates()
    {
        // Arrange
        var grid = new Grid
        (
            [
                [CellState.Dead,  CellState.Alive],
                [CellState.Alive, CellState.Dead ]
            ]
        );

        const int targetRow = 1;
        const int targetColumn = 0;

        // Act
        var cellState = grid.GetCell(targetRow, targetColumn);

        // Assert
        cellState.Should().Be(CellState.Alive);
    }
}