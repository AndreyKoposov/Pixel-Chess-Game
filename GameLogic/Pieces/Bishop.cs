
namespace GameLogic;

public class Bishop : Piece {
    private static readonly Direction[] dirs =
    {
        Direction.NorthEast,
        Direction.NorthWest,
        Direction.SouthEast,
        Direction.SouthWest
    };

    public override PieceType Type => PieceType.Bishop;
    public override Player Color { get; }
    public override int HP { get; set; } = 6;

    public Bishop (Player color) {
        Color = color;
    }

    public override IPrototype Copy () {
        Bishop copy = new(Color) {
            HasMoved = HasMoved,
            HP = HP
        };

        return copy;
    }

    public override IEnumerable<Move> GetMoves (Position from, Board board) {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
