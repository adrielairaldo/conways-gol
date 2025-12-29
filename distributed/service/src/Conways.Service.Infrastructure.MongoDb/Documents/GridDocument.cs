namespace Conways.Service.Infrastructure.MongoDb.Documents;

internal sealed class GridDocument
{
    public IReadOnlyList<IReadOnlyList<int>> Cells { get; init; } = [];
}