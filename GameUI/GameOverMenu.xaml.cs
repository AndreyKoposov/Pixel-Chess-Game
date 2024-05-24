using System.Windows;
using System.Windows.Controls;
using GameLogic;

namespace GameUI;

/// <summary>
/// Interaction logic for GameOverMenu.xaml
/// </summary>
public partial class GameOverMenu : UserControl, GameUIComponent {
    //public event Action<Option> OptionSelected;
    private GameDialog dialog;
    public GameOverMenu (GameState gameState) {
        InitializeComponent();

        Result result = gameState.Result;
        WinnerText.Text = GetWinnerText(result.Winner);
        ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
    }

    public void SetMediator(IMediator mediator)
    {
        this.dialog = (GameDialog)mediator;
    }

    private static string GetWinnerText (Player winner) {
        return winner switch {
            Player.White => "WHITE WINS",
            Player.Black => "BLACK WINS",
            _ => "DRAW"
        };
    }

    private static string PlayerString (Player player) {
        return player switch {
            Player.White => "WHITE",
            Player.Black => "BLACK",
            _ => ""
        };
    }

    private static string GetReasonText (EndReason reason, Player currentPlayer) {
        return reason switch {
            EndReason.StaleMate => $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
            EndReason.CheckMate => $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
            EndReason.FiftyMoveRule => $"FIFTY MOVE RULE",
            EndReason.InsufficientMaterial => $"INSUFFICIENT MATERIAL",
            EndReason.ThreefoldRepetition => $"THREEHOLD REPITITION",
            EndReason.KingDied => $"{PlayerString(currentPlayer)} KING IS DEFEATED",
            EndReason.OnlyKing => $"{PlayerString(currentPlayer)} KING'S ARMY IS DEFEATED",
            _ => ""
        };
    }

    private void Restart_Click (object sender, RoutedEventArgs e) {
        dialog.Notify(this, Option.Restart);
        //OptionSelected?.Invoke(Option.Restart);
    }

    private void Exit_Click (object sender, RoutedEventArgs e) {
        dialog.Notify(this, Option.Exit);
        //OptionSelected?.Invoke(Option.Exit);
    }
}
