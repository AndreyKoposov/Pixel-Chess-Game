namespace GameLogic; 
public class GameState {
    private static GameState singleton;

    public Board Board { get; }
    public Player CurrentPlayer { get; private set; }
    public Result Result { get; private set; } = null;
    public GunKing PlayerKing { get; private set; }
    public Stack<Move> OpponentMoves { get; private set; } = new Stack<Move>();

    public static GameState GetInstance () {
        return singleton;
    }
    public static void ReloadGame (Level level) {
        singleton = new GameState(Player.White, Board.Initial(level.LevelBuilder()));
    }

    private GameState (Player nextPlayer, Board nextBoard) {
        CurrentPlayer = nextPlayer;
        Board = nextBoard;
        PlayerKing = Board.GetGunKing();
    }

    public IEnumerable<Move> LegalMovesForPiece (Position pos) {
        if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            return [];

        Piece piece = Board[pos];

        return piece.GetMoves(pos, Board).Where(move => move.IsLegal(Board));
    }

    public void Move (Move move) {
        move.ExecuteOn(Board);


            if (PlayerKing.HasShot && CurrentPlayer == PlayerKing.Color) {
                if (!IsNextTurn())
                {
                    return;
                }
            ChessBot cb = new StockFishToChessBot();

            Move mv = new NormalMove(new Position(cb.BotMove(Board)[0], cb.BotMove(Board)[1]), new Position(cb.BotMove(Board)[2], cb.BotMove(Board)[3]));
            OpponentMoves.Push(mv);


            PlayerKing.Reset();
            }
            if (OpponentMoves.Count == 0 && CurrentPlayer == PlayerKing.Color.Opponent()) {
                if (!IsNextTurn())
                {
                    return;
                }
            }
            if (OpponentMoves.Count > 0 && CurrentPlayer == PlayerKing.Color.Opponent())
            {
                CheckForGameOver();
                if (Result != null)
                {
                    return;
                }
            }
        }

        private bool IsNextTurn()
        {
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();

            return Result == null;
        }

        private void NextTurn()
        {
            CurrentPlayer = CurrentPlayer.Opponent();
        }

    public IEnumerable<Move> AllLegalMovesForPlayer (Player player) {
        IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos => {
            Piece piece = Board[pos];
            return piece.GetMoves(pos, Board);
        });

        return moveCandidates.Where(move => move.IsLegal(Board));
    }

        private void CheckForGameOver () {
            if (CurrentPlayer == PlayerKing.Color.Opponent())
            {
                if (!Board.HasKingOf(CurrentPlayer))
                    Result = Result.KingDefeated(CurrentPlayer.Opponent());
                else if (Board.OnlyKingFor(CurrentPlayer))
                    Result = Result.KingsArmyDefeated(CurrentPlayer.Opponent());
            }
            else if (!AllLegalMovesForPlayer(CurrentPlayer).Any())
            {
                Result = Result.Win(CurrentPlayer.Opponent());
            }
        }

    public bool IsGameOver () {
        return Result != null;
    }


}
