﻿namespace GameLogic {
    public class GameState {
        private static GameState singleton;

        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;
        public GunKing PlayerKing { get; private set; }
        public Stack<Move> OpponentMoves { get; private set; } = new Stack<Move>();

        public static GameState GetInstance() 
        {
            return singleton; 
        }
        public static void ReloadGame(Level level)
        {
            singleton = new GameState(Player.White, Board.Initial(LevelExtension.LevelBuilder(level)));
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

            if (PlayerKing.HasShoot && CurrentPlayer == PlayerKing.Color) {
                CurrentPlayer = CurrentPlayer.Opponent();
                //OpponentMoves.Push(new NormalMove(new Position(1, 0), new Position(2, 0)));
                //OpponentMoves.Push(new NormalMove(new Position(1, 1), new Position(2, 1)));
                //OpponentMoves.Push(new NormalMove(new Position(1, 2), new Position(2, 2)));
                PlayerKing.Reset();
            }
            if (OpponentMoves.Count == 0 && CurrentPlayer == PlayerKing.Color.Opponent()) {
                CurrentPlayer = CurrentPlayer.Opponent();
            }


            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesForPlayer (Player player) {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos => {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver () {
            if (!AllLegalMovesForPlayer(CurrentPlayer).Any() && CurrentPlayer == PlayerKing.Color) {

                if (Board.IsInCheck(CurrentPlayer))
                    Result = Result.Win(CurrentPlayer.Opponent());
                else
                    Result = Result.Draw(EndReason.StaleMate);
            }
            else
            {
                //Board.PiecePositionsFor(P)
            }
        }

        public bool IsGameOver () {
            return Result != null;
        }

    }
}
