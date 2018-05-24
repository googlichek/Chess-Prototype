using System.Collections.Generic;
using UnityEngine;

namespace ChessProto
{
	public class GameHandler : MonoBehaviour
	{
		void Awake()
		{
			GameData.EnemyPieces = new List<BasePiece>();
			GameData.PlayerPieces = new List<BasePiece>();
			GameData.Cells = new List<Cell>();
		}

		public void Reset()
		{
			GameData.EnemyPieces.Clear();
			GameData.PlayerPieces.Clear();
			GameData.Cells.Clear();
		}
	}
}
