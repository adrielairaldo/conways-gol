using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Boards.AdvanceBoard;

public sealed record AdvanceBoardCommand(BoardId BoardId, int Steps) : ICommand<AdvanceBoardResult>;