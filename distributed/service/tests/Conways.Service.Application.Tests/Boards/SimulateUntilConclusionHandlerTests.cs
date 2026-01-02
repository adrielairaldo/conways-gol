using Conways.Service.Application.Boards.SimulateUntilConclusion;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.Simulation;
using Conways.Service.Domain.TestData;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
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
        _boardSimulationService = new BoardSimulationService(_nextGenerationCalculator, NullLogger<BoardSimulationService>.Instance);

        _handler = new SimulateUntilConclusionHandler
        (
            _boardRepositoryMock.Object,
            _boardSimulationService,
            NullLogger<SimulateUntilConclusionHandler>.Instance
        );
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
    public async Task HandleAsync_ShouldReturnMaxIterationsExceeded_WhenSimulationDoesNotConclude()
    {
        // Arrange
        var boardId = BoardId.New();
        var glider = BasicGridGenerator.Glider3x3();

        var board = new Board
        (
            boardId,
            new BoardState(glider, generation: 0)
        );

        _boardRepositoryMock
            .Setup(r => r.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var command = new SimulateUntilConclusionCommand
        (
            boardId,
            /* A 3x3 glider stabilizes in a 2x2 square starting at iteration 4. At iteration 3,
             * there are still no conclusive results. */
            MaxIterations: 3
        );

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.SimulationResult.TerminationReason.Should().Be(SimulationTerminationReason.MaxIterationsExceeded);
    }
}