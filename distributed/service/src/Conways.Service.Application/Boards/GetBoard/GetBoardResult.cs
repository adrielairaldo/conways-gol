using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Boards.GetBoard;

public sealed record GetBoardResult(BoardState CurrentState);