
namespace GameLogic;

public class GunKing : Piece {

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
    public override PieceType Type => PieceType.GunKing;
    public override Player Color { get; }
    public int Bullets { get; set; } = 6;
    public bool HasShot { get; set; } = false;

    public GunKing (Player color) {
        Color = color;
    }

    public override IPrototype Copy () {
        GunKing copy = new(Color) {
            HasMoved = HasMoved,
            HasShot = HasShot,
            Bullets = Bullets
        };

        return copy;
    }

    private IEnumerable<Position> MovePositions (Position from, Board board) {
        if (HasMoved) yield break;

        foreach (Direction dir in dirs) {
            Position to = from + dir;

            if (!Board.IsInside(to)) {
                continue;
            }

            if (board.IsEmpty(to)) {
                yield return to;
            }
        }
    }
    private IEnumerable<Position> ShotPositions (Position from, Board board) {
        if (Bullets == 0) return [];
        else return board.PiecePositionsFor(Color.Opponent()).Where(pos =>
        {
            return CheckPiecePositionForShot(from, pos, board);
        });
    }

    private bool CheckPiecePositionForShot(Position from, Position to, Board board)
    {
        Direction shotDir = new Direction(from.Row - to.Row, from.Column - to.Column);

        int RowDir, ColDir;
        if (shotDir.RowDelta > shotDir.ColumnDelta)
        {
            RowDir = Math.Sign(shotDir.RowDelta);
            ColDir = 0;
        }
        else if (shotDir.RowDelta > shotDir.ColumnDelta)
        {
            RowDir = 0;
            ColDir = Math.Sign(shotDir.ColumnDelta);
        }
        else
        {
            RowDir = Math.Sign(shotDir.RowDelta);
            ColDir = Math.Sign(shotDir.ColumnDelta);
        }

        Position front = new Position(to.Row + RowDir, to.Column + ColDir);

        return board.IsEmpty(front) || board[front].Type == PieceType.GunKing;
    }

    public override IEnumerable<Move> GetMoves (Position from, Board board) {
        foreach (Position to in MovePositions(from, board)) {
            NormalMove move = new(from, to);
            move.MoveEvent += ReloadGun;
            yield return move;
        }
        foreach (Position to in ShotPositions(from, board)) {
            yield return new ShotMove(from, to);
        }
    }

    public override bool CanCaptureOpponentKing (Position from, Board board) {
        return false;
    }

    public void ReloadGun () {
        if (Bullets < 6)
            Bullets++;
    }

    internal void Reset () {
        HasShot = false;
        HasMoved = false;
    }
}
