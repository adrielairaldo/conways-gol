using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Tests.TestData;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Boards;

public sealed class BoardTests
{
    [Fact]
    public void Constructor_ShouldAssignIdAndInitialState()
    {
        // Arrange
        var boardId = new BoardId(Guid.NewGuid());
        var initialState = new BoardState
        (
            BasicGridGenerator.SimpleVerticalBlinker(),
            generation: 0
        );

        // Act
        var board = new Board(boardId, initialState);

        // Assert
        board.Id.Should().Be(boardId);
        board.CurrentState.Should().Be(initialState);
    }

    [Fact]
    public void AdvanceTo_ShouldReplaceCurrentState()
    {
        // Arrange
        var board = new Board
        (
            new BoardId(Guid.NewGuid()),
            new BoardState(BasicGridGenerator.SimpleVerticalBlinker(), generation: 0)
        );

        var nextState = new BoardState
        (
            BasicGridGenerator.StillLifeBlock(),
            generation: 1
        );

        // Act
        board.AdvanceTo(nextState);

        // Assert
        board.CurrentState.Should().Be(nextState);
    }

    [Fact]
    public void AdvanceTo_ShouldNotChangeBoardId()
    {
        // Arrange
        var boardId = new BoardId(Guid.NewGuid());
        var board = new Board
        (
            boardId,
            new BoardState(BasicGridGenerator.SimpleVerticalBlinker(), generation: 0)
        );

        var nextState = new BoardState
        (
            BasicGridGenerator.StillLifeBlock(),
            generation: 1
        );

        // Act
        board.AdvanceTo(nextState);

        // Assert
        board.Id.Should().Be(boardId);
    }
}