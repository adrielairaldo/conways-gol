namespace Conways.Service.Domain.Boards;

/// <summary>
/// Represents a snapshot of the grid at a specific generation.
/// </summary>
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