namespace Conways.Service.Domain.Simulation;

public enum SimulationTerminationReason
{
    StableStateReached,
    OscillationDetected,
    MaxIterationsExceeded
}