
namespace GameLogic;

public class Queen : Piece {
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
    public override PieceType Type => PieceType.Queen;
    public override Player Color { get; }
    public override int HP { get; set; } = 15;

    public Queen (Player color) {
        Color = color;
    }

    public override Piece Copy () {
        Queen copy = new Queen(Color);
        copy.HasMoved = HasMoved;
        copy.HP = HP;

        return copy;
    }

    public override IEnumerable<Move> GetMoves (Position from, Board board) {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
