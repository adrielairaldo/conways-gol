using Conways.Service.Domain.Boards;

namespace Conways.Service.HttpApi.Contracts;

public sealed record CreateBoardRequest(Grid InitialGrid);