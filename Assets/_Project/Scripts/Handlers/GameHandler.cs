using System.Collections.Generic;
using UnityEngine;

namespace ChessProto
{
	public class GameHandler : MonoBehaviour
	{
		private BoardHandler _boardHandler = null;

		void Awake()
		{
			GameData.EnemyPieces = new List<BasePiece>();
			GameData.PlayerPieces = new List<BasePiece>();
			GameData.Cells = new List<Cell>();
		}

		void Start()
		{
			_boardHandler = FindObjectOfType<BoardHandler>();
			_boardHandler.EnablePieceMovementEvent += SubscribeToEvents;
		}

		public void Reset()
		{
			GameData.EnemyPieces.Clear();
			GameData.PlayerPieces.Clear();
			GameData.Cells.Clear();
		}

		private void SubscribeToEvents()
		{
			foreach (var cell in GameData.Cells)
			{
				cell.CellClickedEvent += CellClickedEventRecieved;
			}

			foreach (var enemy in GameData.EnemyPieces)
			{
				enemy.ChessPieceClickedEvent += EnemyChessPieceClickedEventRecieved;
			}

			foreach (var enemy in GameData.PlayerPieces)
			{
				enemy.ChessPieceClickedEvent += PlayerChessPieceClickedEventRecieved;
			}
		}

		private void CellClickedEventRecieved(Cell cell)
		{
		}

		private void EnemyChessPieceClickedEventRecieved(BasePiece piece)
		{
		}

		private void PlayerChessPieceClickedEventRecieved(BasePiece piece)
		{
		}
	}
}
