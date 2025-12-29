using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Boards.CreateBoard;

public sealed record CreateBoardCommand(Grid InitialGrid) : ICommand<CreateBoardResult>;