namespace GameLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public GameState(Player nextPlayer, Board nextBoard)
        {
            CurrentPlayer = nextPlayer;
            Board = nextBoard;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if(Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            var moveCandidates = piece.GetMoves(pos, Board);

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void Move(Move move)
        {
            move.ExecuteOn(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}
