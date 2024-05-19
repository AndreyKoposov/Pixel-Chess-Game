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
    }
}
