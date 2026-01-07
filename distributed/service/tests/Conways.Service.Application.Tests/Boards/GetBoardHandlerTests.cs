using Conways.Service.Application.Boards.GetBoard;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.TestData;
using Conways.Service.Application.Cache;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Conways.Service.Application.Tests.Boards;

public sealed class GetBoardHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetBoardHandler _handler;

    public GetBoardHandlerTests()
    {
        _boardRepositoryMock = new Mock<IBoardRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetBoardHandler(_boardRepositoryMock.Object, _cacheServiceMock.Object, NullLogger<GetBoardHandler>.Instance);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnBoardState_WhenBoardExists()
    {
        // Arrange
        var boardId = BoardId.New();

        var boardState = new BoardState
        (
            grid: BasicGridGenerator.SimpleVerticalBlinker(),
            generation: 3
        );

        var board = new Board(boardId, boardState);

        var query = new GetBoardQuery(boardId);

        _cacheServiceMock
            .Setup(cache =>
                cache.GetAsync<GetBoardResult>(
                    boardId.ToCacheKey(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetBoardResult?)null);

        _boardRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.CurrentState.Should().Be(boardState);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = BoardId.New();
        var query = new GetBoardQuery(boardId);

        _cacheServiceMock
            .Setup(cache =>
                cache.GetAsync<GetBoardResult>(
                    boardId.ToCacheKey(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetBoardResult?)null);

        _boardRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Board?)null);

        // Act
        var act = async () =>
            await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{boardId.Value}*");
    }
}