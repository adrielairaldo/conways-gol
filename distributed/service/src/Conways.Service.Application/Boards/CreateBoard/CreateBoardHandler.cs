using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Conways.Service.Application.Boards.CreateBoard;

/// <summary>
/// Handles the creation of a new Game of Life board.
/// </summary>
public sealed class CreateBoardHandler : ICommandHandler<CreateBoardCommand, CreateBoardResult>
{
    private readonly IBoardRepository _boardRepository;
    private readonly ILogger<CreateBoardHandler> _logger;

    public CreateBoardHandler(IBoardRepository boardRepository, ILogger<CreateBoardHandler> logger)
    {
        _boardRepository = boardRepository;
        _logger = logger;
    }

    public async Task<CreateBoardResult> HandleAsync(CreateBoardCommand command, CancellationToken cancellationToken)
    {
        var boardId = BoardId.New();

        var initialBoardState = new BoardState
        (
            grid: command.InitialGrid,
            generation: 0
        );

        var board = new Board(boardId, initialBoardState);

        await _boardRepository.SaveAsync(board, cancellationToken);

        _logger.LogInformation("New board created with ID: {BoardId}", boardId.Value);

        return new CreateBoardResult(boardId);
    }
}