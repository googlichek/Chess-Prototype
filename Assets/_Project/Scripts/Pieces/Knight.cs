namespace ChessProto
{
	/// <summary>
	/// Describes Knight piece behaviour.
	/// </summary>
	public class Knight : BasePiece
	{
		public override void FindCellsToHighlight()
		{
			FindSelf();

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
