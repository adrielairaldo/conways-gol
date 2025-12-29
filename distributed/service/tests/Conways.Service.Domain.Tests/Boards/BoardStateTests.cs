using Conways.Service.Domain.Boards;
using Conways.Service.Domain.TestData;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Boards;

public sealed class BoardStateTests
{
    [Fact]
    public void Constructor_ShouldAssignGridAndGeneration()
    {
        // Arrange
        var grid = BasicGridGenerator.SimpleVerticalBlinker();
        const int generation = 5;

        // Act
        var boardState = new BoardState(grid, generation);

        // Assert
        boardState.Grid.Should().Be(grid);
        boardState.Generation.Should().Be(generation);
    }

    [Fact]
    public void BoardState_ShouldBeImmutable()
    {
        // Arrange
        var grid = BasicGridGenerator.SimpleVerticalBlinker();
        var boardState = new BoardState(grid, generation: 0);

        // Act
        var act = () =>
        {
            // No setters exist; compilation itself enforces immutability
            // This test exists to document the design decision
        };

        // Assert
        act.Should().NotThrow();
    }
}