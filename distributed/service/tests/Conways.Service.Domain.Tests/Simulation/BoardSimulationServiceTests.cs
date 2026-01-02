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
        // Arrange
        var glider = BasicGridGenerator.Glider3x3();

        var initialState = new BoardState(glider, generation: 0);

        // Act
        var result = _simulationService.SimulateUntilConclusion
        (
            initialState,
            /* A 3x3 glider stabilizes in a 2x2 square starting at iteration 4. At iteration 3,
             * there are still no conclusive results. */
            maxIterations: 3);

        // Assert
        result.TerminationReason.Should().Be(SimulationTerminationReason.MaxIterationsExceeded);
    }
}