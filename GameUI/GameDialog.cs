using GameLogic;

namespace GameUI
{
    public class GameDialog : IMediator
    {
        private LevelMenu levelMenu;
        private GameOverMenu gameOverMenu;

        public event Action<Level> LevelOptionSelected;
        public event Action<Option> GameOverOptionSelected;

        public LevelMenu LevelMenu
        {
            set
            {
                levelMenu = value;
            }
        }

        public GameOverMenu GameOverMenu
        {
            set
            {
                gameOverMenu = value;
            }
        }

        public GameDialog() { }

        public GameDialog(LevelMenu levelMenu, GameOverMenu gameOverMenu, MainWindow mainWindow)
        {
            this.levelMenu = levelMenu;
            this.gameOverMenu = gameOverMenu;
        }
        public void Notify(GameUIComponent component, Enum option)
        {
            if(component == gameOverMenu)
            {
                GameOverOptionSelected?.Invoke((Option)option);
            }
            if(component == levelMenu)
            {
                LevelOptionSelected?.Invoke((Level)option);
            }
        }
    }
}
