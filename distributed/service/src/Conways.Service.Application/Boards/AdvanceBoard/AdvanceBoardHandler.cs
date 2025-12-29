using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Domain.Rules;

namespace Conways.Service.Application.Boards.AdvanceBoard;

public sealed class AdvanceBoardHandler : ICommandHandler<AdvanceBoardCommand, AdvanceBoardResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly NextGenerationCalculator _nextGenerationCalculator;

    public AdvanceBoardHandler(IBoardRepository boardRepository, NextGenerationCalculator nextGenerationCalculator)
    {
        _boardRepository = boardRepository;
        _nextGenerationCalculator = nextGenerationCalculator;
    }

    public async Task<AdvanceBoardResult> HandleAsync(AdvanceBoardCommand command, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

        if (board is null)
        {
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

        return new AdvanceBoardResult(currentState);
    }
}