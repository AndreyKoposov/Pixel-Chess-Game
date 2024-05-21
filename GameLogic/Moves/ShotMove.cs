namespace GameLogic
{
    internal class ShotMove : Move
    {
        public override MoveType Type => MoveType.ShotMove;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public ShotMove(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
        }

        public override void ExecuteOn(Board board)
        {
            Piece piece = board[ToPos];

            int damage = 1;
            double distance = Math.Sqrt(Math.Pow((ToPos.Row - FromPos.Row),2) + Math.Pow((ToPos.Column - FromPos.Column),2));

            if      (distance < 2) damage = 4;
            else if (distance < 4 && distance >= 2) damage = 3;
            else if (distance < 6 && distance >= 4) damage = 2;
            else if (distance >= 6) damage = 1;

            piece.HP -= damage;

            if(piece.HP <= 0)
            {
                board[ToPos] = null;
            }

            board[FromPos].HasMoved = true;
        }
    }
}
