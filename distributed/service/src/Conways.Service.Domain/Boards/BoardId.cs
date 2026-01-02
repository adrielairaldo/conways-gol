namespace Conways.Service.Domain.Boards;

/// <summary>
/// A unique identifier for a Board.
/// </summary>
public readonly record struct BoardId(Guid Value)
{
    public static BoardId New() => new(Guid.NewGuid());
}