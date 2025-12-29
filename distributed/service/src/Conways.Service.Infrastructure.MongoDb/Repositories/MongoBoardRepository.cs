using Conways.Service.Domain.Boards;
using Conways.Service.Domain.Repositories;
using Conways.Service.Infrastructure.MongoDb.Documents;
using Conways.Service.Infrastructure.MongoDb.Mappings;

using MongoDB.Driver;

namespace Conways.Service.Infrastructure.MongoDb.Repositories;

public sealed class MongoBoardRepository : IBoardRepository
{
    private readonly IMongoCollection<BoardDocument> _boardsCollection;

    public MongoBoardRepository(IMongoDatabase database)
    {
        _boardsCollection = database.GetCollection<BoardDocument>("boards");
    }

    public async Task SaveAsync(Board board, CancellationToken cancellationToken)
    {
        var boardDocument = board.ToDocument();

        await _boardsCollection.ReplaceOneAsync
        (
            filter: x => x.Id == boardDocument.Id,
            replacement: boardDocument,
            options: new ReplaceOptions { IsUpsert = true },
            cancellationToken: cancellationToken
        );
    }

    public async Task<Board?> GetByIdAsync(BoardId boardId, CancellationToken cancellationToken)
    {
        var boardDocument = await _boardsCollection
            .Find(x => x.Id == boardId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        return boardDocument is null
            ? null
            : boardDocument.ToDomain();
    }
}