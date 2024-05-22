namespace GameLogic.LevelBuilders;

internal class Level2Builder : ILevelBuilder {
    public Level2Builder (Board board) : base(board) {}

    internal override void AddBishops () {
        board[0, 2] = new Bishop(Player.Black);
        board[0, 5] = new Bishop(Player.Black);
    }

    internal override void AddKings () {
        board[0, 4] = new King(Player.Black);
    }

    internal override void AddKnights () {
        board[0, 1] = new Knight(Player.Black);
        board[0, 6] = new Knight(Player.Black);
    }

    internal override void AddPawns () {
        for (int i = 0; i < 8; i++) {
            board[1, i] = new Pawn(Player.Black);
        }
    }

    internal override void AddQueens () {
        board[0, 3] = new Queen(Player.Black);
    }

    internal override void AddRooks () {
        board[0, 0] = new Rook(Player.Black);
        board[0, 7] = new Rook(Player.Black);
    }
}
