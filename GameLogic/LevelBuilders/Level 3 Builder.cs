namespace GameLogic.LevelBuilders;

internal class Level3Builder : ILevelBuilder {
    public Level3Builder (Board board) : base(board) {}

    internal override void AddKings () {
        board[0, 4] = new King(Player.Black);
    }

    internal override void AddKnights () {
        board[2, 3] = new Knight(Player.Black);
        board[2, 4] = new Knight(Player.Black);
        board[1, 2] = new Knight(Player.Black);
        board[1, 5] = new Knight(Player.Black);
        board[0, 1] = new Knight(Player.Black);
        board[0, 6] = new Knight(Player.Black);
    }
}
