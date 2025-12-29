using Conways.Service.Application.Abstractions;
using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;

namespace Conways.Service.Application.Boards.CreateBoard;

public sealed class CreateBoardHandler : ICommandHandler<CreateBoardCommand, CreateBoardResult>
{
    private readonly IBoardRepository _boardRepository;

    public CreateBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
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

        return new CreateBoardResult(boardId);
    }
}