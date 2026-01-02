namespace Conways.Service.Domain.Boards;

/// <summary>
/// Represents the root entity for a Game of Life board.
/// </summary>
public sealed class Board
{
    public BoardId Id { get; }
    public BoardState CurrentState { get; private set; }

    public Board(BoardId id, BoardState initialState)
    {
        Id = id;
        CurrentState = initialState;
    }

    /// <summary>
    /// Updates the board to a new state/generation.
    /// </summary>
    public void AdvanceTo(BoardState nextState)
    {
        CurrentState = nextState;
    }
}