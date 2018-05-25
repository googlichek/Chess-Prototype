namespace ChessProto
{
	public class Knight : BasePiece
	{
		private const int DistanceUnit = GlobalVariables.DistanceUnit;

		public override void HighlightPositions()
		{
			FindPosition(Column + 2 * DistanceUnit, Row + DistanceUnit, Side);
			FindPosition(Column + 2 * DistanceUnit, Row - DistanceUnit, Side);

			FindPosition(Column - 2 * DistanceUnit, Row + DistanceUnit, Side);
			FindPosition(Column - 2 * DistanceUnit, Row - DistanceUnit, Side);

			FindPosition(Column + DistanceUnit, Row + 2 * DistanceUnit, Side);
			FindPosition(Column - DistanceUnit, Row + 2 * DistanceUnit, Side);

			FindPosition(Column + DistanceUnit, Row - 2 * DistanceUnit, Side);
			FindPosition(Column - DistanceUnit, Row - 2 * DistanceUnit, Side);
		}
	}
}
