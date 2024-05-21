
namespace GameLogic;

public class Rook : Piece
{
    private static readonly Direction[] dirs =
    {
        Direction.West, 
        Direction.South, 
        Direction.East, 
        Direction.North
    };
    public override PieceType Type => PieceType.Rook;
    public override Player Color { get; }
    public override int HP { get; set; } = 10;

    public Rook(Player color)
    {
        Color = color;
    }

    public override Piece Copy()
    {
        Rook copy = new Rook(Color);
        copy.HasMoved = HasMoved;
        copy.HP = HP;

        return copy;
    }

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
