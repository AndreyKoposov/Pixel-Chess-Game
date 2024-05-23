namespace GameLogic;

public abstract class Piece : IPrototype {
    public abstract PieceType Type { get; }
    public abstract Player Color { get; }
    public bool HasMoved { get; set; } = false;
    public virtual int HP { get; set; } = 0;

    public abstract IPrototype Copy ();
    public abstract IEnumerable<Move> GetMoves (Position from, Board board);

    protected IEnumerable<Position> MovePositionInDir (Position from, Board board, Direction dir) {
        for (Position pos = from + dir; Board.IsInside(pos); pos += dir) {
            if (board.IsEmpty(pos)) {
                yield return pos;
                continue;
            }

            Piece piece = board[pos];

            if (piece.Color != Color) {
                yield return pos;
            }

            yield break;
        }
    }

    protected IEnumerable<Position> MovePositionInDirs (Position from, Board board, Direction[] dirs) {
        return dirs.SelectMany(dir => MovePositionInDir(from, board, dir));
    }

    public virtual bool CanCaptureOpponentKing (Position from, Board board) {
        return GetMoves(from, board).Any(move => {
            return board[move.ToPos]?.Type == PieceType.GunKing;
        });
    }
}
