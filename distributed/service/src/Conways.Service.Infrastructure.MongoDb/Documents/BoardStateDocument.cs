namespace Conways.Service.Infrastructure.MongoDb.Documents;

internal sealed class BoardStateDocument
{
    public GridDocument Grid { get; init; } = null!;
    public int Generation { get; init; }
}