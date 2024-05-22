﻿namespace GameLogic;

public class Board {
    public readonly Position StartGunKingPosition = new Position(7, 3);
    private readonly Piece[,] pieces = new Piece[8, 8];
    public ILevelGenerator LevelGenerator { get; }


    public Board(ILevelGenerator generator) 
    {
        LevelGenerator = generator;
    }
    public Piece this[int row, int column] {
        get { return pieces[row, column]; }
        set { pieces[row, column] = value; }
    }

    public Piece this[Position pos] {
        get { return pieces[pos.Row, pos.Column]; }
        set { pieces[pos.Row, pos.Column] = value; }
    }

    public static Board Initial (ILevelGenerator generator) {
        Board board = new Board(generator);
        board.AddStartPieces();

        return board;
    }

    public GunKing GetGunKing () {
        return (GunKing) this[StartGunKingPosition.Row, StartGunKingPosition.Column];
    }

    private void AddStartPieces () {
        LevelGenerator.BuildBoard(this);
        /*this[0, 0] = new Rook(Player.Black);
        this[0, 1] = new Knight(Player.Black);
        this[0, 2] = new Bishop(Player.Black);
        this[0, 3] = new Queen(Player.Black);
        this[0, 4] = new King(Player.Black);
        this[0, 5] = new Bishop(Player.Black);
        this[0, 6] = new Knight(Player.Black);
        this[0, 7] = new Rook(Player.Black);

        //this[7, 0] = new Rook(Player.White);
        //this[7, 1] = new Knight(Player.White);
        //this[7, 2] = new Bishop(Player.White);
        this[StartGunKingPosition.Row, StartGunKingPosition.Column] = new GunKing(Player.White);
        //this[7, 4] = new Queen(Player.White);
        //this[7, 5] = new Bishop(Player.White);
        //this[7, 6] = new Knight(Player.White);
        //this[7, 7] = new Rook(Player.White);

        for (int i = 0; i < 3; i++) {
            this[1, i] = new Pawn(Player.Black);
            //this[6, i] = new Pawn(Player.White);
        }*/
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
                Position pos = new Position(r, c);

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

    public Board Copy () {
        Board boardCopy = new Board(LevelGenerator);

        foreach (Position pos in PiecePositions()) {
            boardCopy[pos] = (Piece)this[pos].Copy();
        }

        return boardCopy;
    }
}
