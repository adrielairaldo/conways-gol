using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.TestData;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Rules;

public sealed class NextGenerationCalculatorTests
{
    private readonly NextGenerationCalculator _calculator;

    public NextGenerationCalculatorTests()
    {
        var aliveNeighborCounter = new AliveNeighborCounter();
        _calculator = new NextGenerationCalculator(aliveNeighborCounter);
    }

    [Fact]
    public void Calculate_ShouldKillAliveCell_WhenItHasFewerThanTwoAliveNeighbors()
    {
        // Arrange
        var grid = new Grid
        (
            [
                [CellState.Dead,  CellState.Dead,  CellState.Dead],
                [CellState.Dead,  CellState.Alive, CellState.Dead],
                [CellState.Dead,  CellState.Dead,  CellState.Dead]
            ]
        );

        // Act
        var nextGrid = _calculator.Calculate(grid);

        // Assert
        nextGrid.GetCell(1, 1).Should().Be(CellState.Dead);
    }

    [Fact]
    public void Calculate_ShouldKeepAliveCellAlive_WhenItHasTwoAliveNeighbors()
    {
        // Arrange
        var grid = new Grid
        (
            [
                [CellState.Dead,  CellState.Alive, CellState.Dead],
                [CellState.Alive, CellState.Alive, CellState.Dead],
                [CellState.Dead,  CellState.Dead,  CellState.Dead]
            ]
        );

        // Act
        var nextGrid = _calculator.Calculate(grid);

        // Assert
        nextGrid.GetCell(1, 1).Should().Be(CellState.Alive);
    }

    [Fact]
    public void Calculate_ShouldKillAliveCell_WhenItHasMoreThanThreeAliveNeighbors()
    {
        // Arrange
        var grid = new Grid
        (
            [
                [CellState.Alive, CellState.Alive, CellState.Alive],
                [CellState.Alive, CellState.Alive, CellState.Dead ],
                [CellState.Dead,  CellState.Dead,  CellState.Dead ]
            ]
        );

        // Act
        var nextGrid = _calculator.Calculate(grid);

        // Assert
        nextGrid.GetCell(1, 1).Should().Be(CellState.Dead);
    }

    [Fact]
    public void Calculate_ShouldReviveDeadCell_WhenItHasExactlyThreeAliveNeighbors()
    {
        // Arrange
        var grid = new Grid
        (
            [
                [CellState.Alive, CellState.Alive, CellState.Dead],
                [CellState.Alive, CellState.Dead,  CellState.Dead],
                [CellState.Dead,  CellState.Dead,  CellState.Dead]
            ]
        );

        // Act
        var nextGrid = _calculator.Calculate(grid);

        // Assert
        nextGrid.GetCell(1, 1).Should().Be(CellState.Alive);
    }

    [Fact]
    public void Calculate_ShouldTransformBlinkerPatternCorrectly()
    {
        // Arrange
        var blinkerHorizontal = BasicGridGenerator.SimpleHorizontalBlinker();

        // Act
        var nextGrid = _calculator.Calculate(blinkerHorizontal);

        // Assert
        nextGrid.Should().BeEquivalentTo(BasicGridGenerator.SimpleVerticalBlinker());
    }
}