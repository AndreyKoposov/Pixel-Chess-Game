namespace GameLogic;

public class GameState
{
    private static GameState singletonInstance = new GameState(Player.White, Board.Initial());
    
    public Board Board { get; }
    public Player CurrentPlayer { get; private set; }
    public Result Result { get; private set; } = null;
    public GunKing PlayerKing { get; private set; }

    public static GameState GetInstance () => singletonInstance;
    
    private GameState(Player nextPlayer, Board nextBoard)
    {
        CurrentPlayer = nextPlayer;
        Board = nextBoard;
        PlayerKing = Board.GetGunKing();
    }

    public IEnumerable<Move> LegalMovesForPiece(Position pos)
    {
        if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            return [];

        Piece piece = Board[pos];

        return piece.GetMoves(pos, Board).Where(move => move.IsLegal(Board));
    }

    public void Move(Move move)
    {
        move.ExecuteOn(Board);

        if (PlayerKing.HasShoot || CurrentPlayer != PlayerKing.Color)
        {
            CurrentPlayer = CurrentPlayer.Opponent();
            PlayerKing.Reset();
        }

        CheckForGameOver();
    }

    public IEnumerable<Move> AllLegalMovesForPlayer(Player player)
    {
        IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
        {
            Piece piece = Board[pos];
            return piece.GetMoves(pos, Board);
        });

        return moveCandidates.Where(move => move.IsLegal(Board));
    }

    private void CheckForGameOver()
    {
        if (!AllLegalMovesForPlayer(CurrentPlayer).Any()) {

            if (Board.IsInCheck(CurrentPlayer))
                Result = Result.Win(CurrentPlayer.Opponent());
            else
                Result = Result.Draw(EndReason.StaleMate);
        }
    }

    public bool IsGameOver()
    {
        return Result != null;
    }
}
