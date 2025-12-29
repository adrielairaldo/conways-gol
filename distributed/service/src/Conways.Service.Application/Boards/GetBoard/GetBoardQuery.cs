using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Boards.GetBoard;

public sealed record GetBoardQuery(BoardId BoardId) : IQuery<GetBoardResult>;