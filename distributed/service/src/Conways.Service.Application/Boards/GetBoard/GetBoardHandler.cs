using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Conways.Service.Application.Boards.GetBoard;

/// <summary>
/// Retrieves the current state of an existing board.
/// </summary>
public sealed class GetBoardHandler : IQueryHandler<GetBoardQuery, GetBoardResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly ILogger<GetBoardHandler> _logger;

    public GetBoardHandler(IBoardRepository boardRepository, ILogger<GetBoardHandler> logger)
    {
        _boardRepository = boardRepository;
        _logger = logger;
    }

    public async Task<GetBoardResult> HandleAsync(GetBoardQuery command, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            _logger.LogWarning("Query failed: Board {BoardId} does not exist.", command.BoardId.Value);
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        return new GetBoardResult(board.CurrentState);
    }
}