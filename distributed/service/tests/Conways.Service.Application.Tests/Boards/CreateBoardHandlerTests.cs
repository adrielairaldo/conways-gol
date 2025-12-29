using Conways.Service.Application.Boards.CreateBoard;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;

using FluentAssertions;

using Moq;

namespace Conways.Service.Application.Tests.Boards;

public sealed class CreateBoardHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepositoryMock;
    private readonly CreateBoardHandler _handler;

    public CreateBoardHandlerTests()
    {
        _boardRepositoryMock = new Mock<IBoardRepository>();
        _handler = new CreateBoardHandler(_boardRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateBoard_WithGenerationZero_AndPersistIt()
    {
        // Arrange
        var initialGrid = new Grid
        (
            [
                [CellState.Dead,  CellState.Alive],
                [CellState.Alive, CellState.Dead ]
            ]
        );

        var command = new CreateBoardCommand(initialGrid);

        Board? persistedBoard = null;

        _boardRepositoryMock
            .Setup(repository => repository.SaveAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()))
            .Callback<Board, CancellationToken>((board, _) =>
            {
                // Capture the arguments passed internally to the method:
                persistedBoard = board;
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.BoardId.Value.Should().NotBe(Guid.Empty);

        persistedBoard.Should().NotBeNull();
        persistedBoard!.Id.Should().Be(result.BoardId);
        persistedBoard.CurrentState.Generation.Should().Be(0);
        persistedBoard.CurrentState.Grid.Should().Be(initialGrid);

        _boardRepositoryMock.Verify
        (
            // Check that SaveAsync was invoked exactly once:
            repository => repository.SaveAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()), Times.Once
        );
    }
}