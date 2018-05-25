namespace ChessProto
{
	/// <summary>
	/// Describes Queen piece behaviour.
	/// </summary>
	public class Queen : BasePiece
	{
		private const int SearchLength = GlobalVariables.BoardLength - 1;

		/// <summary>
		/// Rules for finding valid Queen movement cells.
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
