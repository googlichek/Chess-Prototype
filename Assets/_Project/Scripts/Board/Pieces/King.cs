namespace ChessProto
{
	/// <summary>
	/// Describes King piece behaviour.
	/// </summary>
	public class King : BasePiece
	{
		public override void FindCellsToHighlight()
		{
			FindSelf();

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
