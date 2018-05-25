namespace ChessProto
{
	/// <summary>
	/// Describes Rook piece behaviour.
	/// </summary>
	public class Rook : BasePiece
	{
		private const int SearchLength = GlobalVariables.BoardLength - 1;

		/// <summary>
		/// Rules for finding valid Rook movement cells.
		/// </summary>
		public override void FindCellsToHighlight()
		{
			FindSelf();

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(Column + index * DistanceUnit, Row, Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(Column - index * DistanceUnit, Row, Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(Column, Row + index * DistanceUnit, Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(Column, Row - index * DistanceUnit, Side);
				if (!isPositionClear) break;
			}
		}
	}
}
