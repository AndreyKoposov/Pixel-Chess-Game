using GameLogic.LevelBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Level1Strategy : ILevelGenerator
    {
        public void BuildBoard (Board board) =>
            LevelDirector.BuildBoard(new Level1Builder(board));
    }
}
