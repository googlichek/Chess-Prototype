using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessProto
{
	public class BasePiece : MonoBehaviour, IPointerClickHandler
	{
		public delegate void OnChessPieceClicked(BasePiece piece);
		public event OnChessPieceClicked ChessPieceClickedEvent;

		public Side Side { get { return _side; } }
		public int Row { get { return _row; } }
		public int Column { get { return _column; } }

		[SerializeField] private Sprite _whitePiece = null;
		[SerializeField] private Sprite _blackPiece = null;

		private Side _side = Side.None;
		private int _row = 0;
		private int _column = 0;

		public void SetSide(Side side)
		{
			_side = side;

			Image image = GetComponent<Image>();
			if (image == null) return;

			switch (_side)
			{
				case Side.Enemy:
					image.sprite = _blackPiece;
					break;
				case Side.Player:
					image.sprite = _whitePiece;
					break;
			}
		}

		public void SetPositionIndexes(int column, int row)
		{
			_row = row;
			_column = column;
		}

		public virtual void Move()
		{
		}

		public virtual void HighlightPositions()
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (ChessPieceClickedEvent != null) ChessPieceClickedEvent(this);
		}

		protected void FindPosition(int column, int row)
		{
			var cell =
				GameData
					.Cells
					.FirstOrDefault(
						x => x.Column == column && x.Row == row && x.Side != _side);

			if (cell == null) return;

			cell.Highlight();
			GameData.HighlightedCells.Add(cell);
		}

		protected void FindOpponentPosition(int column, int row, Side original)
		{
			var opponent = Side.None;

			switch (original)
			{
				case Side.Player:
					opponent = Side.Enemy;
					break;
				case Side.Enemy:
					opponent = Side.Player;
					break;
			}

			if (opponent == Side.None) return;

			var cell =
				GameData
					.Cells
					.FirstOrDefault(
						x => x.Column == column && x.Row == row && x.Side == opponent);

			if (cell == null) return;

			cell.Highlight();
			GameData.HighlightedCells.Add(cell);
		}
	}
}
