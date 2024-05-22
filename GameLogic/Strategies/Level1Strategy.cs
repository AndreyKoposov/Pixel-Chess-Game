using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Level1Strategy : ILevelGenerator
    {
        public void BuildBoard(Board board)
        {
            board[board.StartGunKingPosition.Row, board.StartGunKingPosition.Column] = new GunKing(Player.White);
            board[0, 4] = new King(Player.Black);
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(Player.Black);
            }
        }
    }
}
