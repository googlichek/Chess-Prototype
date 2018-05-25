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
		protected const int DistanceUnit = GlobalVariables.DistanceUnit;

		/// <summary>
		/// Indicates to which side this piece belongs.
		/// </summary>
		public Side Side { get { return _side; } }

		/// <summary>
		/// Row index of the piece.
		/// </summary>
		public int Row { get { return _row; } }

		/// <summary>
		/// Column index of the piece.
		/// </summary>
		public int Column { get { return _column; } }

		[Header("Animation Variables")]
		public Ease MovementEase = Ease.OutQuint;
		public Ease DefeatEase = Ease.InQuint;
		public Ease HighlightEase = Ease.InOutSine;
		[Range(0, 1)] public float AnimationDuration = 1;
		[Range(0, 1)] public float DefeatAnimationDuration = 0.5f;
		[Range(0, 1)] public float HighlightAnimationDuration = 0.5f;
		[Range(0, 2)] public float ScaleAmount = 1.2f;

		[Header("Piece Sprites")]
		[SerializeField] private Sprite _whitePiece = null;
		[SerializeField] private Sprite _blackPiece = null;

		private Image _image = null;

		private Side _side = Side.None;
		private int _row = 0;
		private int _column = 0;

		/// <summary>
		/// Sets side of the piece with valid image.
		/// </summary>
		/// <param name="side">Side to which piece should belong.</param>
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

		/// <summary>
		/// Sets board position indexes of this piece.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row index.</param>
		public void SetPositionIndexes(int column, int row)
		{
			_row = row;
			_column = column;
		}

		/// <summary>
		/// Rules for finding valid piece movement cells.
		/// </summary>
		public virtual void FindCellsToHighlight()
		{
		}

		/// <summary>
		/// Tries to move piece to new position.
		/// </summary>
		/// <param name="column">Destination column index.</param>
		/// <param name="row">Destination row index.</param>
		public void Move(int column, int row)
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

			// Updating cell states.
			startCell.Side = Side.Free;
			endCell.Side = Side;

			if (ChessPieceMoveResetEvent != null) ChessPieceMoveResetEvent();

			// Updating piece position on the board.
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


		public void OnPointerClick(PointerEventData eventData)
		{
			// Sends this piece object to all listeners.
			if (ChessPieceClickedEvent != null) ChessPieceClickedEvent(this);
		}

		/// <summary>
		/// Highlights selected piece.
		/// </summary>
		public void HighlightPiece()
		{
			_image.DOKill();
			_image.transform.DOScale(1, 0);
			_image.transform
				.DOPunchScale(ScaleAmount * Vector2.one, HighlightAnimationDuration)
				.SetEase(HighlightEase);
		}

		/// <summary>
		/// Destroys piece.
		/// </summary>
		public void SelfDestruct()
		{
			_image.DOFade(0, DefeatAnimationDuration).SetEase(DefeatEase);
			_image.transform
				.DOScale(0, DefeatAnimationDuration)
				.SetEase(DefeatEase)
				.OnComplete(() => Destroy(gameObject));
		}

		/// <summary>
		/// Finds first cell that doesn't belong to any side
		/// and corresponds to given coordinates.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row idnex.</param>
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

		/// <summary>
		/// Finds first cell that doesn't belong to any
		/// or belongs to opposite side
		/// and corresponds to given coordinates.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row idnex.</param>
		/// <param name="original">Sender's side.</param>
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

		/// <summary>
		/// Method for finding valid movement cells in loop.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row idnex.</param>
		/// <param name="original">Sender's side.</param>
		/// <returns>True, if loop should continue,
		/// False otherwise.</returns>
		protected bool FindPositionWithValidation(int column, int row, Side original)
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
			if (cell == null) return false;

			if (cell.Side == opponent)
			{
				GameData.HighlightedCells.Add(cell);
				return false;
			}

			GameData.HighlightedCells.Add(cell);
			return true;
		}

		/// <summary>
		/// Finds first cell that belongs to opposite side
		/// and corresponds to given coordinates.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row idnex.</param>
		/// <param name="original">Sender's side.</param>
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

		/// <summary>
		/// Finds current piece cell.
		/// </summary>
		protected void FindSelf()
		{
			var cell =
				GameData
					.Cells
					.FirstOrDefault(x => x.Column == _column && x.Row == _row);
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
