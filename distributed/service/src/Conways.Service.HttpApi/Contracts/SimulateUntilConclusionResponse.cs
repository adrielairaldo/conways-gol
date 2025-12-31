using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Simulation;

namespace Conways.Service.HttpApi.Contracts;

public sealed record SimulateUntilConclusionResponse
(
    IReadOnlyList<IReadOnlyList<CellState>> Grid,
    int Generation,
    SimulationTerminationReason TerminationReason
)
{
    public static SimulateUntilConclusionResponse From(SimulationResult simulationResult) => new
    (
        simulationResult.FinalState.Grid.Cells,
        simulationResult.FinalState.Generation,
        simulationResult.TerminationReason
    );
}