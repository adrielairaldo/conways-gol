using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Boards.SimulateUntilConclusion;

public sealed record SimulateUntilConclusionCommand(BoardId BoardId, int MaxIterations) : ICommand<SimulateUntilConclusionResult>;