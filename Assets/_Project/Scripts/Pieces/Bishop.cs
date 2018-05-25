namespace ChessProto
{
	/// <summary>
	/// Describes Bishop piece behaviour.
	/// </summary>
	public class Bishop : BasePiece
	{
		private const int SearchLength = GlobalVariables.BoardLength - 1;

		/// <summary>
		/// Rules for finding valid Bishop movement cells.
		/// </summary>
		public override void FindCellsToHighlight()
		{
			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(
						Column + index * DistanceUnit,
						Row + index * DistanceUnit,
						Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(
						Column + index * DistanceUnit,
						Row - index * DistanceUnit,
						Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(
						Column - index * DistanceUnit,
						Row + index * DistanceUnit,
						Side);
				if (!isPositionClear) break;
			}

			for (int index = 1; index <= SearchLength; index++)
			{
				var isPositionClear =
					FindPositionWithValidation(
						Column - index * DistanceUnit,
						Row - index * DistanceUnit,
						Side);
				if (!isPositionClear) break;
			}
		}
	}
}
