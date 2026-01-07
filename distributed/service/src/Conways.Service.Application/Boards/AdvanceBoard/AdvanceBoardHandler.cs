using Conways.Service.Application.Abstractions;
using Conways.Service.Application.Cache;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;

using Microsoft.Extensions.Logging;

namespace Conways.Service.Application.Boards.AdvanceBoard;

/// <summary>
/// Handles the logic for moving a board forward by a specific number of generations.
/// </summary>
public sealed class AdvanceBoardHandler : ICommandHandler<AdvanceBoardCommand, AdvanceBoardResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly ICacheService _cacheService;

    private readonly NextGenerationCalculator _nextGenerationCalculator;
    private readonly ILogger<AdvanceBoardHandler> _logger;

    public AdvanceBoardHandler(IBoardRepository boardRepository, ICacheService cacheService, NextGenerationCalculator nextGenerationCalculator, ILogger<AdvanceBoardHandler> logger)
    {
        _boardRepository = boardRepository;
        _nextGenerationCalculator = nextGenerationCalculator;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<AdvanceBoardResult> HandleAsync(AdvanceBoardCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Advancing board {BoardId} by {Steps} steps.", command.BoardId.Value, command.Steps);

        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
            _logger.LogWarning("Board {BoardId} not found for advancement.", command.BoardId.Value);
            throw new InvalidOperationException($"Board with id '{command.BoardId.Value}' was not found.");
        }

        var currentState = board.CurrentState;

        for (var step = 0; step < command.Steps; step++)
        {
            var nextGrid = _nextGenerationCalculator.Calculate(currentState.Grid);

            currentState = new BoardState
            (
                grid: nextGrid,
                generation: currentState.Generation + 1
            );
        }

        board.AdvanceTo(currentState);

        await _boardRepository.SaveAsync(board, cancellationToken);

        _logger.LogInformation("Board {BoardId} successfully advanced to generation {Generation}.",
                                command.BoardId.Value, currentState.Generation);

        await _cacheService.RemoveAsync(command.BoardId.ToCacheKey(), cancellationToken);

        return new AdvanceBoardResult(currentState);
    }
}