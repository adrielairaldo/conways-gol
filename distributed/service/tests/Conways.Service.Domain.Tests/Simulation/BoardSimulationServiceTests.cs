using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Rules;
using Conways.Service.Domain.Simulation;
using Conways.Service.Domain.TestData;

using FluentAssertions;

namespace Conways.Service.Domain.Tests.Simulation;

public sealed class BoardSimulationServiceTests
{
    private readonly BoardSimulationService _simulationService;

    public BoardSimulationServiceTests()
    {
        var aliveNeighborCounter = new AliveNeighborCounter();
        var nextGenerationCalculator = new NextGenerationCalculator(aliveNeighborCounter);

        _simulationService = new BoardSimulationService(nextGenerationCalculator);
    }

    [Fact]
    public void SimulateUntilConclusion_ShouldDetectStableState()
    {
        // Arrange
        var grid = BasicGridGenerator.StillLifeBlock();

        var initialState = new BoardState(grid, generation: 0);

        // Act
        var result = _simulationService.SimulateUntilConclusion(initialState, maxIterations: 10);

        // Assert
        result.TerminationReason.Should().Be(SimulationTerminationReason.StableStateReached);

        result.FinalState.Generation.Should().Be(0);
    }

    [Fact]
    public void SimulateUntilConclusion_ShouldDetectOscillation()
    {
        // Arrange
        var blinkerVertical = BasicGridGenerator.SimpleVerticalBlinker();

        var initialState = new BoardState(blinkerVertical, generation: 0);

        // Act
        var result = _simulationService.SimulateUntilConclusion(initialState, maxIterations: 10);

        // Assert
        result.TerminationReason.Should().Be(SimulationTerminationReason.OscillationDetected);
    }

    [Fact]
    public void SimulateUntilConclusion_ShouldThrow_WhenMaxIterationsReached()
    {
        // Arrange (glider keeps evolving)
        var grid = new Grid
        (
            [
                [CellState.Dead,  CellState.Alive, CellState.Dead ],
                [CellState.Dead,  CellState.Dead,  CellState.Alive],
                [CellState.Alive, CellState.Alive, CellState.Alive]
            ]
        );

        var initialState = new BoardState(grid, generation: 0);

        // Act
        var act = () => _simulationService.SimulateUntilConclusion(initialState, maxIterations: 2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*did not reach a conclusion*");
    }
}
