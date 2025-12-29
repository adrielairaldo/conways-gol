using Conways.Service.Domain.Boards;
using Conways.Service.Infrastructure.MongoDb.Documents;

namespace Conways.Service.Infrastructure.MongoDb.Mappings;

internal static class BoardMapper
{
    public static BoardDocument ToDocument(this Board board) => new()
    {
        Id = board.Id.Value,
        CurrentState = new BoardStateDocument
        {
            Generation = board.CurrentState.Generation,
            Grid = new GridDocument
            {
                Cells = board.CurrentState.Grid.Cells
                    .Select(row => row.Select(cell => (int)cell).ToList())
                    .ToList()
            }
        }
    };

    public static Board ToDomain(this BoardDocument document)
    {
        var gridCells = document.CurrentState.Grid.Cells
            .Select(row => row.Select(value => (CellState)value).ToList())
            .ToList();

        var grid = new Grid(gridCells);

        var boardState = new BoardState(grid, document.CurrentState.Generation);

        return new Board(new BoardId(document.Id), boardState);
    }
}