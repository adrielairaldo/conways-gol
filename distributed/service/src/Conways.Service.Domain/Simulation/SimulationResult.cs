using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Simulation;

public sealed class SimulationResult
{
    public BoardState FinalState { get; }
    public SimulationTerminationReason TerminationReason { get; }

    public SimulationResult(BoardState finalState, SimulationTerminationReason terminationReason)
    {
        FinalState = finalState;
        TerminationReason = terminationReason;
    }
}