namespace ChessProto
{
	public class King : BasePiece
	{
		public override void FindCellsToHighlight()
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
