namespace GameLogic;

public class Board : IPrototype {
    public readonly Position StartGunKingPosition = new(7, 3);
    private readonly Piece[,] pieces = new Piece[8, 8];
    public ILevelGenerator LevelGenerator { get; }


    public Board (ILevelGenerator generator) {
        LevelGenerator = generator;
    }

    public Board () { }

    public Piece this[int row, int column] {
        get { return pieces[row, column]; }
        set { pieces[row, column] = value; }
    }

    public Piece this[Position pos] {
        get { return pieces[pos.Row, pos.Column]; }
        set { pieces[pos.Row, pos.Column] = value; }
    }

    public static Board Initial (ILevelGenerator generator) {
        Board board = new(generator);
        board.AddStartPieces();

        return board;
    }

    public GunKing GetGunKing () {
        return (GunKing) this[StartGunKingPosition.Row, StartGunKingPosition.Column];
    }

    private void AddStartPieces () {
        LevelGenerator.BuildBoard(this);
    }

    public static bool IsInside (Position pos) {
        return pos.Row >= 0 && pos.Column >= 0 && pos.Column < 8 && pos.Row < 8;
    }

    public bool IsEmpty (Position pos) {
        return this[pos] == null;
    }

    public IEnumerable<Position> PiecePositions () {
        for (ushort r = 0; r < 8; r++) {
            for (ushort c = 0; c < 8; c++) {
                Position pos = new(r, c);

                if (!IsEmpty(pos)) {
                    yield return pos;
                }
            }
        }
    }

    public IEnumerable<Position> PiecePositionsFor (Player player) {
        return PiecePositions().Where(pos => this[pos].Color == player);
    }

    public bool IsInCheck (Player player) {
        return PiecePositionsFor(player.Opponent()).Any(pos => {
            Piece piece = this[pos];
            return piece.CanCaptureOpponentKing(pos, this);
        });
    }

    public IPrototype Copy () {
        Board boardCopy = new(LevelGenerator);

        foreach (Position pos in PiecePositions()) {
            boardCopy[pos] = (Piece) this[pos].Copy();
        }

        return boardCopy;
    }
}
