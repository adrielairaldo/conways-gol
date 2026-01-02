namespace Conways.Service.Infrastructure.MongoDb.Documents;

/// <summary>
/// Data contract for storing a board's state, including its generation number.
/// </summary>
internal sealed class BoardStateDocument
{
    public GridDocument Grid { get; init; } = null!;
    public int Generation { get; init; }
}