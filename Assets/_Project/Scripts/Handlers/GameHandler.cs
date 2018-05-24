using System.Collections.Generic;
using UnityEngine;

namespace ChessProto
{
	public class GameHandler : MonoBehaviour
	{
		private BoardHandler _boardHandler = null;

		private BasePiece _activePiece = null;

		void Awake()
		{
			GameData.EnemyPieces = new List<BasePiece>();
			GameData.PlayerPieces = new List<BasePiece>();
			GameData.Cells = new List<Cell>();
			GameData.HighlightedCells = new List<Cell>();
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
			GameData.HighlightedCells.Clear();
		}

		private void SubscribeToEvents()
		{
			foreach (var cell in GameData.Cells)
			{
				cell.CellClickedEvent += CellClickedEventRecieved;
			}

			foreach (var enemy in GameData.EnemyPieces)
			{
				enemy.ChessPieceClickedEvent += ChessPieceClickedEventRecieved;
			}

			foreach (var enemy in GameData.PlayerPieces)
			{
				enemy.ChessPieceClickedEvent += ChessPieceClickedEventRecieved;
			}
		}

		private void CellClickedEventRecieved(Cell cell)
		{
			if (_activePiece == null) return;

			if (GameData.HighlightedCells.Contains(cell))
			{
				_activePiece.Move();
			}
			else
			{
				foreach (var highlightedCell in GameData.HighlightedCells)
					highlightedCell.StopHighlighting();
				GameData.HighlightedCells.Clear();

				_activePiece = null;
			}
		}

		private void ChessPieceClickedEventRecieved(BasePiece piece)
		{
			_activePiece = piece;
			piece.HighlightPositions();
		}
	}
}
