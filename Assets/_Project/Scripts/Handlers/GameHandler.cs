using System.Collections.Generic;
using UnityEngine;

namespace ChessProto
{
	/// <summary>
	/// Handles game flow.
	/// Listens to all events that pieces and cells send.
	/// </summary>
	public class GameHandler : MonoBehaviour
	{
		private BoardHandler _boardHandler = null;
		private BasePiece _activePiece = null;

		private bool _pieceMovementEnabled = true;

		void Awake()
		{
			GameData.Pieces = new List<BasePiece>();
			GameData.Cells = new List<Cell>();
			GameData.HighlightedCells = new List<Cell>();
		}

		void Start()
		{
			_boardHandler = FindObjectOfType<BoardHandler>();
			_boardHandler.EnablePieceMovementEvent += SubscribeToEvents;
		}

		private void SubscribeToEvents()
		{
			foreach (var cell in GameData.Cells)
				cell.CellClickedEvent += CellClickedEventRecieved;

			foreach (var piece in GameData.Pieces)
				SubscribeToPieceEvents(piece);

			// Clearing list to assure safe destruction of pieces.
			GameData.Pieces.Clear();
		}

		private void SubscribeToPieceEvents(BasePiece piece)
		{
			piece.ChessPieceClickedEvent += ChessPieceClickedEventRecieved;
			piece.ChessPieceMoveStartEvent += ChessPieceMoveStartEventRecieved;
			piece.ChessPieceMoveEndEvent += ChessPieceMoveEndEventRecieved;
			piece.ChessPieceMoveResetEvent += ResetBoardActivities;
		}

		private void CellClickedEventRecieved(Cell cell)
		{
			if (!_pieceMovementEnabled) return;
			if (_activePiece == null) return;

			if (GameData.HighlightedCells.Contains(cell)) _activePiece.Move(cell.Column, cell.Row);
			else ResetBoardActivities();
		}

		private void ChessPieceClickedEventRecieved(BasePiece piece)
		{
			if (!_pieceMovementEnabled) return;

			if (_activePiece == null)
			{
				BeginBoardActivities(piece);
			}
			else
			{
				if (piece.Side != _activePiece.Side)
				{
					TryToPickOnOpponent(piece);
				}
				else
				{
					ResetBoardActivities();
					BeginBoardActivities(piece);
				}
			}
		}

		private void TryToPickOnOpponent(BasePiece piece)
		{
			var isTargetValid =
				GameData.HighlightedCells.Exists(
					x =>
						x.Column == piece.Column &&
						x.Row == piece.Row &&
						x.Side == piece.Side);

			if (!isTargetValid)
			{
				ResetBoardActivities();
				return;
			}

			_activePiece.Move(piece.Column, piece.Row);
			piece.SelfDestruct();
		}

		private void ChessPieceMoveStartEventRecieved()
		{
			_pieceMovementEnabled = false;
		}

		private void ChessPieceMoveEndEventRecieved()
		{
			_pieceMovementEnabled = true;
		}

		private void BeginBoardActivities(BasePiece piece)
		{
			_activePiece = piece;
			piece.HighlightPiece();

			piece.FindCellsToHighlight();
			foreach (var highlightedCell in GameData.HighlightedCells)
				highlightedCell.Highlight();
		}

		private void ResetBoardActivities()
		{
			foreach (var highlightedCell in GameData.HighlightedCells)
				highlightedCell.StopHighlighting();
			GameData.HighlightedCells.Clear();

			_activePiece = null;
		}
	}
}
