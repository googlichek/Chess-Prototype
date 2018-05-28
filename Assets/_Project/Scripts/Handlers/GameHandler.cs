using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ChessProto
{
	/// <summary>
	/// Handles game flow.
	/// Listens to all events that pieces and cells send.
	/// </summary>
	public class GameHandler : MonoBehaviour
	{
		[Header("Message Variables")]
		[SerializeField] private Text _message = null;
		[SerializeField] private Ease _messageSubsequenceEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _messageSubsequenceDuration = 0f;
		[SerializeField] private string _start = "Let's Start!";
		[SerializeField] private string _wrongMove = "Wrong move!";
		[SerializeField] private string _wrongSide = "Wrong side!";

		private BoardHandler _boardHandler = null;
		private BasePiece _activePiece = null;

		private Side _activeSide = Side.None;

		private bool _pieceMovementEnabled = true;

		void Awake()
		{
			GameData.Pieces = new List<BasePiece>();
			GameData.Cells = new List<Cell>();
			GameData.HighlightedCells = new List<Cell>();
		}

		void Start()
		{
			_pieceMovementEnabled = false;

			_boardHandler = FindObjectOfType<BoardHandler>();
			_boardHandler.EnableBoardEvent += SubscribeToEvents;

			_activeSide = Side.Player;
		}

		private void ShowMessage(string message)
		{
			if (!_pieceMovementEnabled) return;
			_pieceMovementEnabled = false;

			_message.text = message;
			var sequence = CreateMessageAnimationSequence();
			sequence.Play().OnComplete(() => _pieceMovementEnabled = true);
		}

		/// <summary>
		/// Displays message.
		/// </summary>
		private Sequence CreateMessageAnimationSequence()
		{
			var animationSequence = DOTween.Sequence();
			animationSequence.Pause();

			var tweener =
				_message
					.DOFade(1, _messageSubsequenceDuration)
					.SetEase(_messageSubsequenceEase);
			animationSequence.Insert(0, tweener);

			tweener =
				_message.transform
					.DOScale(1, _messageSubsequenceDuration)
					.SetEase(_messageSubsequenceEase);
			animationSequence.Insert(0, tweener);

			tweener =
				_message
					.DOFade(0, _messageSubsequenceDuration)
					.SetEase(_messageSubsequenceEase)
					.SetDelay(_messageSubsequenceDuration);
			animationSequence.Append(tweener);

			tweener = _message.transform.DOScale(0, 0);
			animationSequence.Append(tweener);

			return animationSequence;
		}

		private void SubscribeToEvents()
		{
			foreach (var cell in GameData.Cells)
				cell.CellClickedEvent += CellClickedEventRecieved;

			foreach (var piece in GameData.Pieces)
				SubscribeToPieceEvents(piece);

			// Clearing list to assure safe destruction of pieces.
			GameData.Pieces.Clear();

			// After creating board & pieces it's safe to allow input.
			_pieceMovementEnabled = true;

			ShowMessage(_start);
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

			if (_activePiece == null)
			{
				ShowMessage(_wrongMove);
				return;
			}

			if (GameData.HighlightedCells.Contains(cell))
			{
				_activePiece.Move(cell.Column, cell.Row);
			}
			else
			{
				ShowMessage(_wrongMove);
				ResetBoardActivities();
			}
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

		/// <summary>
		/// Checks wether piece belongs to opposing side
		/// and can be attacked by _activePiece.
		/// Initiates movement, if so.
		/// </summary>
		/// <param name="piece">Opposing piece.</param>
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
				piece.Highlight();
				ShowMessage(_wrongSide);
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
			SwitchSides();

		}

		/// <summary>
		/// Tries to set piece for making a move.
		/// </summary>
		/// <param name="piece">Piece to be moved.</param>
		private void BeginBoardActivities(BasePiece piece)
		{
			if (_activeSide != piece.Side)
			{
				ShowMessage(_wrongSide);
				piece.Highlight();
				return;
			}

			_activePiece = piece;
			piece.Highlight();

			piece.FindCellsToHighlight();
			foreach (var highlightedCell in GameData.HighlightedCells)
				highlightedCell.Highlight();
		}

		/// <summary>
		/// Resets board state.
		/// </summary>
		private void ResetBoardActivities()
		{
			foreach (var highlightedCell in GameData.HighlightedCells)
				highlightedCell.StopHighlighting();
			GameData.HighlightedCells.Clear();

			_activePiece = null;
		}

		private void SwitchSides()
		{
			switch (_activeSide)
			{
				case Side.Player:
					_activeSide = Side.Enemy;
					break;
				case Side.Enemy:
					_activeSide = Side.Player;
					break;
			}
		}
	}
}
