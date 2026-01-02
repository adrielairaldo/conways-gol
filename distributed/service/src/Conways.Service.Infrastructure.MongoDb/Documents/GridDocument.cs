namespace Conways.Service.Infrastructure.MongoDb.Documents;

/// <summary>
/// Stores the grid as a nested list of integers for database compatibility.
/// </summary>
internal sealed class GridDocument
{
    public IReadOnlyList<IReadOnlyList<int>> Cells { get; init; } = [];
}