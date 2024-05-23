namespace GameLogic; 
public enum Level {
    Level1,
    Level2,
    Level3
}

public static class LevelExtension {
    public static ILevelGenerator LevelBuilder (this Level level) =>
        level switch {
            Level.Level1 => new Level1Strategy(),
            Level.Level2 => new Level2Strategy(),
            Level.Level3 => new Level3Strategy(),
            _ => null
        };
}
