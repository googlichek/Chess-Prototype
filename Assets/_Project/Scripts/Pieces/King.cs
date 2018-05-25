namespace ChessProto
{
	public class King : BasePiece
	{
		private const int DistanceUnit = GlobalVariables.DistanceUnit;

		public override void HighlightPositions()
		{
			FindPosition(Column + DistanceUnit, Row + DistanceUnit, Side);
			FindPosition(Column + DistanceUnit, Row - DistanceUnit, Side);

			FindPosition(Column - DistanceUnit, Row + DistanceUnit, Side);
			FindPosition(Column - DistanceUnit, Row - DistanceUnit, Side);

			FindPosition(Column, Row + DistanceUnit, Side);
			FindPosition(Column, Row - DistanceUnit, Side);

			FindPosition(Column + DistanceUnit, Row, Side);
			FindPosition(Column - DistanceUnit, Row, Side);
		}
	}
}
