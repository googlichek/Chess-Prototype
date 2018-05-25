namespace ChessProto
{
	/// <summary>
	/// Describes Pawn piece behaviour.
	/// </summary>
	public class Pawn : BasePiece
	{
		private const int PlayerInitialColumn = 2;
		private const int EnemyInitialColumn = 7;

		public override void FindCellsToHighlight()
		{
			var initialColumn = 0;
			var multiplier = 0;

			switch (Side)
			{
				case Side.Player:
					initialColumn = PlayerInitialColumn;
					multiplier = DistanceUnit;
					break;
				case Side.Enemy:
					initialColumn = EnemyInitialColumn;
					multiplier = -DistanceUnit;
					break;
			}

			FindSelf();

			FindFreePosition(Column + multiplier, Row);

			FindPosition(Column + multiplier, Row + multiplier, Side);
			FindPosition(Column + multiplier, Row - multiplier, Side);

			FindOpponentPosition(Column - multiplier, Row + multiplier, Side);
			FindOpponentPosition(Column - multiplier, Row - multiplier, Side);

			if (Column == initialColumn)
				FindFreePosition(Column + 2 * multiplier, Row);
		}
	}
}
