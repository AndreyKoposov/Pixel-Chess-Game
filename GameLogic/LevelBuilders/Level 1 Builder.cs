namespace GameLogic.LevelBuilders;

internal class Level1Builder : ILevelBuilder {
    public Level1Builder (Board board) : base(board) { }

    internal override void AddPawns () {
        for (int i = 0; i < 8; i++) {
            board[1, i] = new Pawn(Player.Black);
        }
    }
}
