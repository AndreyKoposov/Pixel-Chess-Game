using System.Windows;
using System.Windows.Controls;
using GameLogic;

namespace GameUI; 
/// <summary>
/// Interaction logic for LevelMenu.xaml
/// </summary>
public partial class LevelMenu : UserControl, GameUIComponent {
    //public event Action<Level> LevelSelected;
    private GameDialog dialog;
    public LevelMenu () {
        InitializeComponent();
    }

    public void SetMediator(IMediator mediator)
    {
        dialog = (GameDialog)mediator;
    }

    private void Level1_Click (object sender, RoutedEventArgs e) {
        dialog.Notify(this, Level.Level1);
        //LevelSelected?.Invoke(Level.Level1);
    }
    private void Level2_Click (object sender, RoutedEventArgs e) {
        dialog.Notify(this, Level.Level2);
        //LevelSelected?.Invoke(Level.Level2);
    }

    private void Level3_Click (object sender, RoutedEventArgs e) {
        dialog.Notify(this, Level.Level3);
        //LevelSelected?.Invoke(Level.Level3);
    }
}
