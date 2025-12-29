using Conways.Service.Domain.Boards;

namespace Conways.Service.HttpApi.Contracts
;
public sealed record GetBoardResponse(IReadOnlyList<IReadOnlyList<CellState>> Grid, int Generation)
{
    public static GetBoardResponse From(BoardState boardState) => new
    (
        boardState.Grid.Cells,
        boardState.Generation
    );
}