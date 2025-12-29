using MongoDB.Bson.Serialization.Attributes;

namespace Conways.Service.Infrastructure.MongoDb.Documents;

internal sealed class BoardDocument
{
    [BsonId]
    public Guid Id { get; init; }

    public BoardStateDocument CurrentState { get; init; } = null!;
}