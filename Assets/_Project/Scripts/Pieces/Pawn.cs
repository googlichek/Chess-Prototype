namespace ChessProto
{
	public class Pawn : BasePiece
	{
		public override void HighlightPositions()
		{
			var initialColumn = 0;
			var multiplier = 0;

			switch (Side)
			{
				case Side.Player:
					initialColumn = 2;
					multiplier = 1;
					break;
				case Side.Enemy:
					initialColumn = 7;
					multiplier = -1;
					break;
			}

			FindPosition(Column, Row);

			FindPosition(Column + multiplier, Row);
			FindPosition(Column + multiplier, Row + multiplier);
			FindPosition(Column + multiplier, Row - multiplier);

			FindOpponentPosition(Column - multiplier, Row + multiplier, Side);
			FindOpponentPosition(Column - multiplier, Row - multiplier, Side);

			if (Column == initialColumn)
				FindPosition(Column + 2 * multiplier, Row);
		}
	}
}
