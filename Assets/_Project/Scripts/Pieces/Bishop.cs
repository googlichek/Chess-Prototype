namespace ChessProto
{
	public class Bishop : BasePiece
	{
		private const int SearchLength = GlobalVariables.BoardLength - 1;

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
