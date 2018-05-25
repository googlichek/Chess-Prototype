using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessProto
{
	public class BasePiece : MonoBehaviour, IPointerClickHandler
	{
		public delegate void OnChessPieceClicked(BasePiece piece);
		public event OnChessPieceClicked ChessPieceClickedEvent;

		public delegate void OnChessPieceMove();
		public event OnChessPieceMove ChessPieceMoveStartEvent;
		public event OnChessPieceMove ChessPieceMoveEndEvent;
		public event OnChessPieceMove ChessPieceMoveResetEvent;

		public Side Side { get { return _side; } }
		public int Row { get { return _row; } }
		public int Column { get { return _column; } }

		[Header("Animation Variables")]
		public Ease MovementEase = Ease.OutQuint;
		public Ease DefeatEase = Ease.InQuint;
		public Ease HighlightEase = Ease.InOutSine;
		[Range(0, 1)] public float AnimationDuration = 1;
		[Range(0, 1)] public float DefeatAnimationDuration = 0.5f;
		[Range(0, 2)] public float ScaleAmount = 1.2f;

		[Header("Piece Sprites")]
		[SerializeField] private Sprite _whitePiece = null;
		[SerializeField] private Sprite _blackPiece = null;

		private Image _image = null;

		private Side _side = Side.None;
		private int _row = 0;
		private int _column = 0;

		public void SetSide(Side side)
		{
			_side = side;

			_image = GetComponent<Image>();
			if (_image == null) return;

			switch (_side)
			{
				case Side.Enemy:
					_image.sprite = _blackPiece;
					break;
				case Side.Player:
					_image.sprite = _whitePiece;
					break;
			}
		}

		public void SetPositionIndexes(int column, int row)
		{
			_row = row;
			_column = column;
		}

		public virtual void Move(int column, int row)
		{
			var endCell =
				GameData.HighlightedCells.FirstOrDefault(x => x.Column == column && x.Row == row);
			var startCell =
				GameData.Cells.FirstOrDefault(x => x.Column == _column && x.Row == _row);

			if (endCell == null || startCell == null)
			{
				if (ChessPieceMoveResetEvent != null) ChessPieceMoveResetEvent();
				return;
			}

			if (ChessPieceMoveStartEvent != null) ChessPieceMoveStartEvent();

			startCell.Side = Side.Free;
			endCell.Side = Side;

			if (ChessPieceMoveResetEvent != null) ChessPieceMoveResetEvent();

			_column = endCell.Column;
			_row = endCell.Row;

			transform
				.DOMove(endCell.transform.position, AnimationDuration)
				.SetEase(MovementEase)
				.OnComplete(() =>
				{
					if (ChessPieceMoveEndEvent != null) ChessPieceMoveEndEvent();
				});
		}

		public virtual void HighlightPositions()
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (ChessPieceClickedEvent != null) ChessPieceClickedEvent(this);
		}

		public void HighlightPiece()
		{
			_image.DOKill();
			_image.transform.DOScale(1, 0);
			_image.transform
				.DOPunchScale(ScaleAmount * Vector2.one, AnimationDuration, 4)
				.SetEase(HighlightEase);
		}

		public void SelfDestruct()
		{
			_image.DOFade(0, DefeatAnimationDuration).SetEase(DefeatEase);
			_image.transform
				.DOScale(0, DefeatAnimationDuration)
				.SetEase(DefeatEase)
				.OnComplete(() => Destroy(gameObject));
		}

		protected void FindFreePosition(int column, int row)
		{
			var cell =
				GameData
					.Cells
					.FirstOrDefault(
						x => x.Column == column && x.Row == row && x.Side == Side.Free);
			if (cell == null) return;

			GameData.HighlightedCells.Add(cell);
		}

		protected void FindPosition(int column, int row, Side original)
		{
			var opponent = FindOpposingSide(original);

			var cell =
				GameData
					.Cells
					.FirstOrDefault(
						x =>
							x.Column == column &&
							x.Row == row &&
							(x.Side == opponent || x.Side == Side.Free));
			if (cell == null) return;

			GameData.HighlightedCells.Add(cell);
		}

		protected void FindOpponentPosition(int column, int row, Side original)
		{
			var opponent = FindOpposingSide(original);

			var cell =
				GameData
					.Cells
					.FirstOrDefault(
						x =>
							x.Column == column &&
							x.Row == row &&
							x.Side == opponent);
			if (cell == null) return;

			GameData.HighlightedCells.Add(cell);
		}

		private static Side FindOpposingSide(Side original)
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

			if (opponent == Side.None) return opponent;
			return opponent;
		}
	}
}
