using GameLogic.LevelBuilders;
using System;
using System.Collections.Generic;
namespace GameLogic
{
    public class Level2Strategy : ILevelGenerator
    {
        public void BuildBoard (Board board) =>
            LevelDirector.BuildBoard(new Level2Builder(board));
    }
}
