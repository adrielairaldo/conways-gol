namespace Conways.Service.Domain.Boards;

public readonly record struct BoardId(Guid Value)
{
    public static BoardId New() => new(Guid.NewGuid());
}