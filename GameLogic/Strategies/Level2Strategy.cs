using System;
using System.Collections.Generic;
namespace GameLogic
{
    public class Level2Strategy : ILevelGenerator
    {
        public void BuildBoard(Board board)
        {
            board[board.StartGunKingPosition.Row, board.StartGunKingPosition.Column] = new GunKing(Player.White);

            board[0, 0] = new Rook(Player.Black);
            board[0, 1] = new Knight(Player.Black);
            board[0, 2] = new Bishop(Player.Black);
            board[0, 3] = new Queen(Player.Black);
            board[0, 4] = new King(Player.Black);
            board[0, 5] = new Bishop(Player.Black);
            board[0, 6] = new Knight(Player.Black);
            board[0, 7] = new Rook(Player.Black);

            for (int i = 0; i < 8; i++) {
                board[1, i] = new Pawn(Player.Black);
            }
        }
    }
}
