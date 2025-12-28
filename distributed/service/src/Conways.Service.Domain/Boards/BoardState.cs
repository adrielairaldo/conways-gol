namespace Conways.Service.Domain.Boards;

public sealed class BoardState
{
    public Grid Grid { get; }
    public int Generation { get; }

    public BoardState(Grid grid, int generation)
    {
        Grid = grid;
        Generation = generation;
    }
}