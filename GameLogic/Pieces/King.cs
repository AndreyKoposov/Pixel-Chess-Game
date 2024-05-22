namespace GameLogic;

public class King : Piece {
    private static readonly Direction[] dirs =
    {
        Direction.West,
        Direction.South,
        Direction.East,
        Direction.North,
        Direction.NorthEast,
        Direction.NorthWest,
        Direction.SouthEast,
        Direction.SouthWest
    };
    public override PieceType Type => PieceType.King;
    public override Player Color { get; }
    public override int HP { get; set; } = 4;

    public King (Player color) {
        Color = color;
    }

    public override Piece Copy () {
        King copy = new(Color) {
            HasMoved = HasMoved,
            HP = HP
        };

        return copy;
    }

    private IEnumerable<Position> MovePositions (Position from, Board board) {
        foreach (Direction dir in dirs) {
            Position to = from + dir;

            if (!Board.IsInside(to)) {
                continue;
            }

            if (board.IsEmpty(to) || board[to].Color != Color) {
                yield return to;
            }
        }
    }

    public override IEnumerable<Move> GetMoves (Position from, Board board) {
        foreach (Position to in MovePositions(from, board)) {
            yield return new NormalMove(from, to);
        }
    }

    public override bool CanCaptureOpponentKing (Position from, Board board) {
        return MovePositions(from, board).Any(to => {
            Piece toPiece = board[to];
            return toPiece != null && toPiece.Type == PieceType.GunKing;
        });
    }
}
