using Conways.Service.Application.Boards.AdvanceBoard;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.TestData;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Conways.Service.Application.Tests.Boards;

public sealed class AdvanceBoardHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepositoryMock;
    private readonly AdvanceBoardHandler _handler;

    public AdvanceBoardHandlerTests()
    {
        _boardRepositoryMock = new Mock<IBoardRepository>();

        var aliveNeighborCounter = new AliveNeighborCounter();
        var nextGenerationCalculator = new NextGenerationCalculator(aliveNeighborCounter);

        _handler = new AdvanceBoardHandler
        (
            _boardRepositoryMock.Object,
            nextGenerationCalculator,
            NullLogger<AdvanceBoardHandler>.Instance
        );
    }

    [Fact]
    public async Task HandleAsync_ShouldAdvanceBoardByGivenNumberOfSteps()
    {
        // Arrange
        var boardId = BoardId.New();

        var initialGrid = BasicGridGenerator.SingleAliveCellGrid();
        var initialState = new BoardState(initialGrid, generation: 0);
        var board = new Board(boardId, initialState);

        _boardRepositoryMock
            .Setup(repo => repo.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        _boardRepositoryMock
            .Setup(repo => repo.SaveAsync(board, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new AdvanceBoardCommand(boardId, Steps: 1);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.CurrentState.Generation.Should().Be(1);
        result.CurrentState.Grid.Should().NotBeSameAs(initialGrid);

        _boardRepositoryMock.Verify
        (
            // Check that SaveAsync was invoked exactly once:
            repo => repo.SaveAsync(board, It.IsAny<CancellationToken>()), Times.Once
        );
    }

[Fact]
    public async Task HandleAsync_ShouldAdvanceBoardMultipleSteps_WhenStepsIsGreaterThanOne()
    {
        // Arrange
        var initialGrid = BasicGridGenerator.SimpleVerticalBlinker();

        var initialState = new BoardState(initialGrid, generation: 0);
        var board = new Board(BoardId.New(), initialState);

        _boardRepositoryMock
            .Setup(repo => repo.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var command = new AdvanceBoardCommand(board.Id, Steps: 3);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.CurrentState.Generation.Should().Be(3);

        _boardRepositoryMock.Verify(
            repo => repo.SaveAsync(board, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = BoardId.New();

        _boardRepositoryMock
            .Setup(repo => repo.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Board?)null);

        var command = new AdvanceBoardCommand(boardId, Steps: 1);

        // Act
        var act = async () =>
            await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{boardId.Value}*");
    }
}