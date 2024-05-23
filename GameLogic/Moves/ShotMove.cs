namespace GameLogic;

internal class ShotMove : Move {
    public override MoveType Type => MoveType.ShotMove;
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    public ShotMove (Position fromPos, Position toPos) {
        FromPos = fromPos;
        ToPos = toPos;
    }

    public override void ExecuteOn (Board board) {
        MakeShot(board);
    }

    public override void ExecuteOnTest (Board board) {
        MakeShot(board);
    }

    public void MakeShot (Board board) {
        GunKing gunKing = (GunKing) board[FromPos];

        double distance = GetDistance(FromPos, ToPos);
        int damage = GetDamageByDistance(distance);

        List<Position> area = GetShotArea(distance, GetShotDirection(FromPos, ToPos));

        foreach (Position pos in area) {
            if (!Board.IsInside(pos) || board.IsEmpty(pos)) continue;

            Piece piece = board[pos];
            piece.HP -= damage;
            if (piece.HP <= 0) {
                board[pos] = null;
            }
        }

        gunKing.Bullets--;
        gunKing.HasShot = true;
    }

    private double GetDistance (Position FromPos, Position ToPos) {
        double delta_row = ToPos.Row - FromPos.Row;
        double delta_col = ToPos.Column - FromPos.Column;

        return Math.Sqrt(Math.Pow(delta_row, 2) + Math.Pow(delta_col, 2));
    }

    private int GetDamageByDistance (double distance) {
        int damage = 0;

        if (distance < 2) damage = 4;
        else if (distance < 4 && distance >= 2) damage = 3;
        else if (distance < 6 && distance >= 4) damage = 2;
        else if (distance >= 6) damage = 1;

        return damage;
    }

    private List<Position> GetShotArea (double distance, Direction dir) {
        Position[] area = new Position[GetAreaSizeByDistance(distance)];

        int delta = -(area.Length / 2);
        for (int i = 0; i < area.Length; i++) {
            Position nextPos = new(ToPos.Row + delta * dir.RowDelta, ToPos.Column + delta * dir.ColumnDelta);
            area[i] = nextPos;
            delta++;
        }

        return new List<Position>(area);
    }

    private int GetAreaSizeByDistance (double distance) {
        int size = 0;

        if (distance < 2) size = 1;
        else if (distance < 4 && distance >= 2) size = 1;
        else if (distance < 6 && distance >= 4) size = 3;
        else if (distance >= 6) size = 5;

        return size;
    }

    private Direction GetShotDirection (Position FromPos, Position ToPos) {
        Direction dir;

        double delta_row = ToPos.Row - FromPos.Row;
        double delta_col = ToPos.Column - FromPos.Column;

        if (delta_row < delta_col) dir = new Direction(0, Math.Sign(delta_row));
        else dir = new Direction(Math.Sign(delta_col), 0);

        return dir;
    }
}
