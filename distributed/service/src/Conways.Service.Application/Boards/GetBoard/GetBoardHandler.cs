using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Cache;
using Conways.Service.Domain.Repositories;

using Microsoft.Extensions.Logging;


namespace Conways.Service.Application.Boards.GetBoard;

/// <summary>
/// Retrieves the current state of an existing board.
/// </summary>
public sealed class GetBoardHandler : IQueryHandler<GetBoardQuery, GetBoardResult>
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    private readonly IBoardRepository _boardRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetBoardHandler> _logger;

    public GetBoardHandler(IBoardRepository boardRepository, ICacheService cacheService, ILogger<GetBoardHandler> logger)
    {
        _boardRepository = boardRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<GetBoardResult> HandleAsync(GetBoardQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = query.BoardId.ToCacheKey();

        var cached = await _cacheService.GetAsync<GetBoardResult>(cacheKey, cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        var board = await _boardRepository.GetByIdAsync(query.BoardId, cancellationToken);

        if (board is null)
        {
            _logger.LogWarning("Query failed: Board {BoardId} does not exist.", query.BoardId.Value);
            throw new InvalidOperationException($"Board with id '{query.BoardId.Value}' was not found.");
        }

        var result = new GetBoardResult(board.CurrentState);

        await _cacheService.SetAsync(cacheKey, result, CacheTtl, cancellationToken);

        return result;
    }
}