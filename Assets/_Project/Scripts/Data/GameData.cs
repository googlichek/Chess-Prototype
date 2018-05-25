using System.Collections.Generic;

namespace ChessProto
{
	/// <summary>
	/// Container for data that is accessed throughout different places.
	/// </summary>
	public struct GameData
	{
		/// <summary>
		/// List of all pieces on board.
		/// </summary>
		public static List<BasePiece> Pieces;

		/// <summary>
		/// List of all cells on board.
		/// </summary>
		public static List<Cell> Cells;

		/// <summary>
		/// List of all currently highlighted cells.
		/// </summary>
		public static List<Cell> HighlightedCells;
	}
}
