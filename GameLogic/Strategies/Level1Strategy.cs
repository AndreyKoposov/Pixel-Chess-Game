using GameLogic.LevelBuilders;

namespace GameLogic;
public class Level1Strategy : ILevelGenerator {
    public void BuildBoard (Board board) =>
        LevelDirector.BuildBoard(new Level1Builder(board));
}
