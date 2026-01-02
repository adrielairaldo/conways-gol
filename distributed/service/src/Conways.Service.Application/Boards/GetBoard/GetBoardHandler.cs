using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Repositories;

namespace Conways.Service.Application.Boards.GetBoard;

/// <summary>
/// Retrieves the current state of an existing board.
/// </summary>
public sealed class GetBoardHandler : IQueryHandler<GetBoardQuery, GetBoardResult>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<GetBoardResult> HandleAsync(GetBoardQuery command, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        return new GetBoardResult(board.CurrentState);
    }
}