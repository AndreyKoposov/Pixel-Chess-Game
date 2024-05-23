using System.Security.Cryptography.X509Certificates;

namespace GameLogic;

public class NormalMove : Move {
    public override MoveType Type => MoveType.Normal;
    public override Position FromPos { get; }
    public override Position ToPos { get; }
    public event Action MoveEvent;

    public NormalMove (Position fromPos, Position toPos) {
        FromPos = fromPos;
        ToPos = toPos;
    }

    public override void ExecuteOn (Board board) {
        MakeMove(board);

        MoveEvent?.Invoke();
    }

    public override void ExecuteOnTest (Board board) {
        MakeMove(board);
    }

    private void MakeMove (Board board) {
        Piece piece = board[FromPos];

        board[ToPos] = piece;
        board[FromPos] = null;

        piece.HasMoved = true;

        if(IsPawnOnLastLine(piece))
        {
            board[ToPos] = PawnToQueen(piece);
        }
    }

    private bool IsPawnOnLastLine(Piece piece)
    {
        if (piece.Type != PieceType.Pawn) 
            return false;

        if (piece.Color == Player.Black)
        {
            return ToPos.Row == 7;
        }
        if (piece.Color == Player.White)
        {
            return ToPos.Row == 0;
        }

        return false;
    }

    private Piece PawnToQueen(Piece pawn)
    {
        return new Queen(pawn.Color);
    }
}
