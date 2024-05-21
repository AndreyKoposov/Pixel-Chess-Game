using System.Runtime.CompilerServices;

namespace GameLogic;

public class Knight : Piece
{
    public override PieceType Type => PieceType.Knight;
    public override Player Color { get; }
    public override int HP { get; set; } = 8;

    public Knight(Player color)
    {
        Color = color;
    }

    public override Piece Copy()
    {
        Knight copy = new Knight(Color);
        copy.HasMoved = HasMoved;
        copy.HP = HP;

        return copy;
    }

    private static readonly Direction[] vDirs = [Direction.North, Direction.South];
    private static readonly Direction[] hDirs = [Direction.East, Direction.West];
    private static IEnumerable<Position> PotentialToPositions(Position from)
    {
        foreach (Direction vDir in vDirs) {
            foreach (Direction hDir in hDirs) {
                yield return from + 2*vDir + hDir;
                yield return from + 2*hDir + vDir;
            }
        }
    }

    private IEnumerable<Position> MovePositions(Position from, Board board)
    {
        return PotentialToPositions(from).Where(pos => IsValidPosition(pos, board));
    }

    private bool IsValidPosition(Position pos, Board board)
    {
        return Board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color);
    }

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositions(from, board).Select(to => new NormalMove(from, to));
    }
}
