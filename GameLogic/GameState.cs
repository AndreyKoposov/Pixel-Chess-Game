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


            var cleanupTask = Task.Run(async () => {
                string a = await ChessBot.BotMove(Board);
                Move mv = new NormalMove(new Position(ConvertChessNotation(a)[0], ConvertChessNotation(a)[1]), new Position(ConvertChessNotation(a)[2], ConvertChessNotation(a)[3]));
                OpponentMoves.Push(mv);
            });
            cleanupTask.Wait();


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

    //===================================================================

    public static int[] ConvertChessNotation (string notation) {
        if (notation.Length != 4) {
            if (notation.Length != 5)
            {
                throw new ArgumentException("Notation must be exactly 4 characters long");
            }
            else notation= notation.Remove(notation.Length - 1);
        }

        int[] result = new int[4];

        result[0] = 8 - CharToInt(notation[1]);
        result[1] = CharToDigit(notation[0]) - 1;
        result[2] = 8 - CharToInt(notation[3]);
        result[3] = CharToDigit(notation[2]) - 1;

        return result;
    }

    private static int CharToDigit (char c) {
        // Convert columns a-h to 1-8
        if (c < 'a' || c > 'h') {
            throw new ArgumentException("Column letter must be between 'a' and 'h'");
        }

        return c - 'a' + 1;
    }

    private static int CharToInt (char c) {
        // Convert rows 1-8 to integers 1-8
        if (c < '1' || c > '8') {
            throw new ArgumentException("Row number must be between '1' and '8'");
        }

        return c - '0';
    }

}
