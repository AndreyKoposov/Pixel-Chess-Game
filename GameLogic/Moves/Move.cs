namespace GameLogic;

public abstract class Move {
    public abstract MoveType Type { get; }
    public abstract Position FromPos { get; }
    public abstract Position ToPos { get; }
    public abstract void ExecuteOn (Board board);
    public abstract void ExecuteOnTest (Board board);
    public virtual bool IsLegal (Board board) {
        Player player = board[FromPos].Color;
        Board boardCopy = (Board) board.Copy();
        ExecuteOnTest(boardCopy);

        return !boardCopy.IsInCheck(player);
    }
}
