namespace GameLogic.LevelBuilders;

internal abstract class ILevelBuilder {
    protected Board board;

    public ILevelBuilder () => board = new();
    public ILevelBuilder (Board board) => this.board = board;


    internal virtual void AddGunKing () =>
        board[board.StartGunKingPosition.Row, board.StartGunKingPosition.Column] = new GunKing(Player.White);
    internal virtual void AddPawns () { }
    internal virtual void AddBishops () { }
    internal virtual void AddKnights () { }
    internal virtual void AddRooks () { }
    internal virtual void AddQueens () { }
    internal virtual void AddKings () { }
    internal virtual Board RetrieveBoard () => board;
}
