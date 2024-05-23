using System.Windows;
using System.Windows.Controls;
using GameLogic;

namespace GameUI; 
/// <summary>
/// Interaction logic for LevelMenu.xaml
/// </summary>
public partial class LevelMenu : UserControl {
    public event Action<Level> LevelSelected;
    public LevelMenu () {
        InitializeComponent();
    }
    private void Level1_Click (object sender, RoutedEventArgs e) {
        LevelSelected?.Invoke(Level.Level1);
    }
    private void Level2_Click (object sender, RoutedEventArgs e) {
        LevelSelected?.Invoke(Level.Level2);
    }

    private void Level3_Click (object sender, RoutedEventArgs e) {
        LevelSelected?.Invoke(Level.Level3);
    }
}
