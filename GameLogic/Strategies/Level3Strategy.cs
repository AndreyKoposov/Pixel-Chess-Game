using GameLogic.LevelBuilders;

namespace GameLogic
{
    public class Level3Strategy : ILevelGenerator
    {
        public void BuildBoard(Board board) =>
            LevelDirector.BuildBoard(new Level3Builder(board));
    }
}
