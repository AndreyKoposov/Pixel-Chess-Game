using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GameLogic;

namespace GameUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    private readonly Image[,] pieceImages = new Image[8, 8];
    private readonly Rectangle[,] highlights = new Rectangle[8, 8];
    private readonly TextBlock[,] hps = new TextBlock[8, 8];

    private readonly Dictionary<Position, Move> moveCache = [];

    private GameState gameState;
    private Position selectedPos = null;
    public MainWindow () {
        InitializeComponent();
        InitializeBoard();
        //SetCursor();

        StartLevelMenu();
    }

    private void StartLevelMenu () {
        LevelMenu levelMenu = new();
        MenuContainer.Content = levelMenu;

        levelMenu.LevelSelected += level => {
            MenuContainer.Content = null;

            GameState.ReloadGame(level);
            gameState = GameState.GetInstance();

            DrawBoard(gameState.Board);
        };
    }

    private void SetCursor () {
        Stream stream = Application.GetResourceStream(new Uri("Assets/pixel chess_v1.2/cursor.cur", UriKind.Relative)).Stream;
        Cursor = new Cursor(stream, true);
    }

    private void InitializeBoard () {
        for (int r = 0; r < 8; r++) {
            for (int c = 0; c < 8; c++) {
                Image image = new();
                pieceImages[r, c] = image;
                PiecesGrid.Children.Add(image);

                Rectangle rect = new();
                highlights[r, c] = rect;
                HighlightGrid.Children.Add(rect);

                TextBlock text = new() {
                    FontSize = 14,
                    Margin = new Thickness(5, 0, 0, 0),
                    Foreground = Brushes.IndianRed
                };
                hps[r, c] = text;
                HPsGrid.Children.Add(text);
            }
        }
    }

    private void DrawBoard (Board board) {
        for (int r = 0; r < 8; r++) {
            for (int c = 0; c < 8; c++) {
                Piece piece = board[r, c];
                pieceImages[r, c].Source = Images.GetImage(piece);

                if (piece != null)
                    hps[r, c].Text = piece.Type == PieceType.GunKing ? ((GunKing) piece).Bullets.ToString()
                                                                       : piece.HP.ToString();
                else
                    hps[r, c].Text = "";
            }
        }
    }

    private void BoardGrid_MouseDown (object sender, MouseButtonEventArgs e) {
        if (IsMenuOnScreen()) return;
        if (gameState.CurrentPlayer != gameState.PlayerKing.Color) return;

        Point point = e.GetPosition(HighlightGrid);
        Position pos = ToSquarePosition(point);

        if (!Board.IsInside(pos)) return;

        if (selectedPos == null) {
            OnFromPositionSelected(pos);
        }
        else {
            OnToPositionSelected(pos);
        }
    }

    private Position ToSquarePosition (Point point) {
        double squareSize = HighlightGrid.ActualWidth / 8;

        int row = (int) (point.Y / squareSize);
        int col = (int) (point.X / squareSize);

        return new Position(row, col);
    }

    private void OnFromPositionSelected (Position pos) {
        IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

        if (moves.Any()) {
            selectedPos = pos;
            CacheMoves(moves);
            ShowHighlights();
        }
    }

    private void OnToPositionSelected (Position pos) {
        selectedPos = null;
        HideHighlights();

        if (moveCache.TryGetValue(pos, out Move move)) {
            HandleMove(move);
        }

        if (gameState.CurrentPlayer == gameState.PlayerKing.Color.Opponent()) {
            OpponentTurnTask();
        }
    }

    private void OpponentTurnTask () 
    {
        if (gameState.IsGameOver())
        {
            ShowGameOver();
            return;
        }

        while (gameState.OpponentMoves.Count > 0) {
            Move opponentMove = gameState.OpponentMoves.Pop();
            HandleMove(opponentMove);
        }
    }

    private void HandleMove (Move move) {
        gameState.Move(move);
        DrawBoard(gameState.Board);

        if (gameState.IsGameOver()) {
            ShowGameOver();
        }
    }

    private void CacheMoves (IEnumerable<Move> moves) {
        moveCache.Clear();

        foreach (Move move in moves) {
            moveCache[move.ToPos] = move;
        }
    }

    private void ShowHighlights () {
        Color moveColor = Color.FromArgb(150, 47, 255, 36);
        Color shotColor = Color.FromArgb(150, 255, 125, 125);

        foreach (var pair in moveCache) {
            if (pair.Value.Type == MoveType.Normal)
                highlights[pair.Key.Row, pair.Key.Column].Fill = new SolidColorBrush(moveColor);
            if (pair.Value.Type == MoveType.ShotMove)
                highlights[pair.Key.Row, pair.Key.Column].Fill = new SolidColorBrush(shotColor);
        }
    }

    private void HideHighlights () {
        foreach (Position pos in moveCache.Keys) {
            highlights[pos.Row, pos.Column].Fill = Brushes.Transparent;
        }
    }

    private bool IsMenuOnScreen () {
        return MenuContainer.Content != null;
    }

    private void ShowGameOver () {
        GameOverMenu gameOverMenu = new(gameState);
        MenuContainer.Content = gameOverMenu;

        gameOverMenu.OptionSelected += option => {
            if (option == Option.Restart) {
                MenuContainer.Content = null;
                RestartGame();
            }
            else {
                Application.Current.Shutdown();
            }
        };
    }

    private void RestartGame () {
        HideHighlights();
        moveCache.Clear();

        StartLevelMenu();
    }
}