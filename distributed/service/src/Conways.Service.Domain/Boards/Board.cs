namespace Conways.Service.Domain.Boards;

public sealed class Board
{
    public BoardId Id { get; }
    public BoardState CurrentState { get; private set; }

    public Board(BoardId id, BoardState initialState)
    {
        Id = id;
        CurrentState = initialState;
    }

    public void AdvanceTo(BoardState nextState)
    {
        CurrentState = nextState;
    }
}