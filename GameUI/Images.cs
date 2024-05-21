using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameLogic;

namespace GameUI;

public static class Images
{
    private static readonly Dictionary<PieceType, ImageSource> whiteSoures = new()
    {
        { PieceType.Pawn, LoadImage("16x32 pieces/W_Pawn.png") },
        { PieceType.Bishop, LoadImage("16x32 pieces/W_Bishop.png") },
        { PieceType.Rook, LoadImage("16x32 pieces/W_Rook.png") },
        { PieceType.Knight, LoadImage("16x32 pieces/W_Knight.png") },
        { PieceType.King, LoadImage("16x32 pieces/W_King.png") },
        { PieceType.GunKing, LoadImage("16x32 pieces/W_King.png") },
        { PieceType.Queen, LoadImage("16x32 pieces/W_Queen.png") }
    };

    private static readonly Dictionary<PieceType, ImageSource> blackSoures = new()
    {
        { PieceType.Pawn, LoadImage("16x32 pieces/B_Pawn.png") },
        { PieceType.Bishop, LoadImage("16x32 pieces/B_Bishop.png") },
        { PieceType.Rook, LoadImage("16x32 pieces/B_Rook.png") },
        { PieceType.Knight, LoadImage("16x32 pieces/B_Knight.png") },
        { PieceType.King, LoadImage("16x32 pieces/B_King.png") },
        { PieceType.GunKing, LoadImage("16x32 pieces/B_King.png") },
        { PieceType.Queen, LoadImage("16x32 pieces/B_Queen.png") }
    };

    private static ImageSource LoadImage(string filePath)
    {
        return new BitmapImage(new Uri("Assets/pixel chess_v1.2/" + filePath, UriKind.Relative));
    }

    public static ImageSource GetImage(Player color, PieceType type)
    {
        return color switch
        {
            Player.White => whiteSoures[type],
            Player.Black => blackSoures[type],
            _ => null
        };
    }

    public static ImageSource GetImage(Piece piece)
    {
        if (piece == null)
            return null;
        else
            return GetImage(piece.Color, piece.Type);
    }
}
