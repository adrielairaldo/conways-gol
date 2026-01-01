using Conways.Service.Application.Boards.SimulateUntilConclusion;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.Simulation;
using Conways.Service.Domain.TestData;

using FluentAssertions;

using Moq;

namespace Conways.Service.Application.Tests.Boards;

public sealed class SimulateUntilConclusionHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepositoryMock;
    private readonly NextGenerationCalculator _nextGenerationCalculator;
    private readonly BoardSimulationService _boardSimulationService;

    private readonly SimulateUntilConclusionHandler _handler;

    public SimulateUntilConclusionHandlerTests()
    {
        _boardRepositoryMock = new Mock<IBoardRepository>();
        _nextGenerationCalculator = new NextGenerationCalculator(new AliveNeighborCounter());
        _boardSimulationService = new BoardSimulationService(_nextGenerationCalculator);

        _handler = new SimulateUntilConclusionHandler(_boardRepositoryMock.Object, _boardSimulationService);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = BoardId.New();

        _boardRepositoryMock
            .Setup(repository => repository.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Board?)null);

        var command = new SimulateUntilConclusionCommand
        (
            boardId,
            MaxIterations: 10
        );

        // Act
        var act = () => _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{boardId.Value}*");
    }

    [Fact]
    public async Task HandleAsync_ShouldDetectStableState()
    {
        // Arrange
        var stableGrid = BasicGridGenerator.StillLifeBlock();

        var board = new Board
        (
            BoardId.New(),
            new BoardState(stableGrid, generation: 0)
        );

        _boardRepositoryMock
            .Setup(repository => repository.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var command = new SimulateUntilConclusionCommand
        (
            board.Id,
            MaxIterations: 10
        );

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.SimulationResult.TerminationReason.Should().Be(SimulationTerminationReason.StableStateReached);
        result.SimulationResult.FinalState.Generation.Should().Be(0);
    }

    [Fact]
    public async Task HandleAsync_ShouldDetectOscillation()
    {
        // Arrange
        var oscillatingGrid = BasicGridGenerator.SimpleVerticalBlinker();

        var board = new Board
        (
            BoardId.New(),
            new BoardState(oscillatingGrid, generation: 0)
        );

        _boardRepositoryMock
            .Setup(repository => repository.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var command = new SimulateUntilConclusionCommand
        (
            board.Id,
            MaxIterations: 10
        );

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.SimulationResult.TerminationReason.Should().Be(SimulationTerminationReason.OscillationDetected);
        result.SimulationResult.FinalState.Generation.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenMaxIterationsReachedWithoutConclusion()
    {
        // Arrange
        var gliderGrid = BasicGridGenerator.Glider();

        var board = new Board
        (
            BoardId.New(),
            new BoardState(gliderGrid, generation: 0)
        );

        _boardRepositoryMock
            .Setup(repository => repository.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var command = new SimulateUntilConclusionCommand
        (
            board.Id,
            MaxIterations: 1 // We know a glider won't stabilize or oscillate in just one iteration
        );

        // Act
        var act = () => _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*did not reach a conclusion*");
    }
}