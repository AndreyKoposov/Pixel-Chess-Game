using GameLogic.LevelBuilders;

namespace GameLogic;
internal class LevelDirector {

    internal static Board BuildBoard (ILevelBuilder levelBuilder) {
        levelBuilder.AddGunKing();
        levelBuilder.AddBishops();
        levelBuilder.AddKings();
        levelBuilder.AddKnights();
        levelBuilder.AddQueens();
        levelBuilder.AddRooks();
        levelBuilder.AddPawns();
        return levelBuilder.RetrieveBoard();
    }
}
