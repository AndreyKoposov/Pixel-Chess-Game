using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;

namespace GameUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = [];

        private GameState gameState;
        private Position selectedPos = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            //SetCursor();

            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);
        }

        private void SetCursor()
        {
            Stream stream = Application.GetResourceStream(new Uri("Assets/pixel chess_v1.2/cursor.cur", UriKind.Relative)).Stream;
            Cursor = new Cursor(stream, true);
        }

        private void InitializeBoard()
        {
            for(int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new Image();
                    pieceImages[r, c] = image;
                    PiecesGrid.Children.Add(image);

                    Rectangle rect = new Rectangle();
                    highlights[r, c] = rect;
                    HighlightGrid.Children.Add(rect);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(HighlightGrid);
            Position pos = ToSquarePosition(point);

            if(selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = HighlightGrid.ActualWidth / 8;

            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);

            return new Position(row, col);
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if(moveCache.TryGetValue(pos, out Move move))
            {
                HandleMove(move);
            }
        }

        private void HandleMove(Move move)
        {
            gameState.Move(move);
            DrawBoard(gameState.Board);
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach(Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 255, 125, 125);

            foreach(Position pos in moveCache.Keys)
            {
                highlights[pos.Row, pos.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position pos in moveCache.Keys)
            {
                highlights[pos.Row, pos.Column].Fill = Brushes.Transparent;
            }
        }
    }
}