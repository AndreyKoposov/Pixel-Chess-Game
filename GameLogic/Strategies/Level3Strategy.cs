namespace GameLogic
{
    public class Level3Strategy : ILevelGenerator
    {
        public void BuildBoard(Board board)
        {
            board[board.StartGunKingPosition.Row, board.StartGunKingPosition.Column] = new GunKing(Player.White);

            board[2, 3] = new Knight(Player.Black);
            board[2, 4] = new Knight(Player.Black);
            board[1, 2] = new Knight(Player.Black);
            board[1, 5] = new Knight(Player.Black);
            board[0, 1] = new Knight(Player.Black);
            board[0, 6] = new Knight(Player.Black);
        }
    }
}
