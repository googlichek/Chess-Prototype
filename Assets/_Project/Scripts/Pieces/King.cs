namespace ChessProto
{
	public class King : BasePiece
	{
		public override void HighlightPositions()
		{
			var multiplier = 0;

			switch (Side)
			{
				case Side.Player:
					multiplier = 1;
					break;
				case Side.Enemy:
					multiplier = -1;
					break;
			}

			FindPosition(Column + multiplier, Row + multiplier, Side);
			FindPosition(Column + multiplier, Row - multiplier, Side);

			FindPosition(Column - multiplier, Row + multiplier, Side);
			FindPosition(Column - multiplier, Row - multiplier, Side);

			FindPosition(Column, Row + multiplier, Side);
			FindPosition(Column, Row - multiplier, Side);

			FindPosition(Column + multiplier, Row, Side);
			FindPosition(Column - multiplier, Row, Side);
		}
	}
}
