
namespace GameLogic;

public class GunKing : Piece
{

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
    public bool HasShoot { get; set; } = false;

    public GunKing(Player color)
    {
        Color = color;
    }

    public override Piece Copy()
    {
        GunKing copy = new GunKing(Color);
        copy.HasMoved = HasMoved;
        copy.HasShoot = HasShoot;
        copy.Bullets = Bullets;

        return copy;
    }

    private IEnumerable<Position> MovePositions(Position from, Board board)
    {
        if (HasMoved) yield break;

        foreach (Direction dir in dirs)
        {
            Position to = from + dir;

            if (!Board.IsInside(to))
            {
                continue;
            }

            if (board.IsEmpty(to))
            {
                yield return to;
            }
        }
    }
    private IEnumerable<Position> ShotPositions(Position from, Board board)
    {
        if (Bullets == 0) return [];
        else              return board.PiecePositionsFor(Color.Opponent());
    }

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        foreach (Position to in MovePositions(from, board))
        {
            NormalMove move = new NormalMove(from, to);
            move.MoveEvent += ReloadGun;
            yield return move;
        }
        foreach (Position to in ShotPositions(from, board))
        {
            yield return new ShotMove(from, to);
        }
    }

    public override bool CanCaptureOpponentKing(Position from, Board board)
    {
        /*return MovePositions(from, board).Any(to =>
        {
            Piece toPiece = board[to];
            return toPiece != null && toPiece.Type == PieceType.King;
        });*/
        return false;
    }

    public void ReloadGun()
    {
        if(Bullets < 6)
            Bullets++;
    }

    internal void Reset()
    {
        HasShoot = false;
        HasMoved = false;
    }
}
