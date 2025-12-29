using Conways.Service.Domain.Boards;

namespace Conways.Service.Domain.Repositories;

public interface IBoardRepository
{
    Task SaveAsync(Board board, CancellationToken cancellationToken);
    Task<Board?> GetByIdAsync(BoardId boardId, CancellationToken cancellationToken);
}