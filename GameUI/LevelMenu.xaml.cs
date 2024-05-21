using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for LevelMenu.xaml
    /// </summary>
    public partial class LevelMenu : UserControl
    {
        public event Action<Level> LevelSelected;
        public LevelMenu()
        {
            InitializeComponent();
        }
        private void Level1_Click(object sender, RoutedEventArgs e)
        {
            LevelSelected?.Invoke(Level.Level1);
        }
        private void Level2_Click(object sender, RoutedEventArgs e)
        {
            LevelSelected?.Invoke(Level.Level2);
        }

        private void Level3_Click(object sender, RoutedEventArgs e)
        {
            LevelSelected?.Invoke(Level.Level3);
        }
    }
}
