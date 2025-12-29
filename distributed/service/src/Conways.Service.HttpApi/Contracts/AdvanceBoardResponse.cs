using Conways.Service.Domain.Boards;

namespace Conways.Service.HttpApi.Contracts
;
public sealed record AdvanceBoardResponse(IReadOnlyList<IReadOnlyList<CellState>> Grid, int Generation)
{
    public static AdvanceBoardResponse From(BoardState boardState) => new
    (
        boardState.Grid.Cells,
        boardState.Generation
    );
}