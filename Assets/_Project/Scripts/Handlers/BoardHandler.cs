using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ChessProto
{
	public class BoardHandler : MonoBehaviour
	{
		public delegate void OnAnimationComplete();
		public event OnAnimationComplete EnablePieceMovementEvent;
		private event OnAnimationComplete StartCreationOfSidesEvent;

		[Header("Temporary Input Blocker")]
		[SerializeField] private GameObject _protector = null;

		[Header("Cell Placement Variables")]
		[SerializeField] private Cell _cell = null;
		[SerializeField] private Color _firstColor = Color.white;
		[SerializeField] private Color _secondColor = Color.white;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionX = 0;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionY = 0;
		[SerializeField] [Range(-2500, 2500)] private int _cellSpawnPositionX = 0;
		[SerializeField] [Range(-2500, 2500)] private int _cellSpawnPositionY = 0;

		[Header("Piece Placement Variables")]
		[SerializeField] private BasePiece _bishop = null;
		[SerializeField] private BasePiece _king = null;
		[SerializeField] private BasePiece _knight = null;
		[SerializeField] private BasePiece _pawn = null;
		[SerializeField] private BasePiece _queen = null;
		[SerializeField] private BasePiece _rook = null;
		[SerializeField] [Range(0, 1000)] private int _spawnOffset = 0;

		[Header("Overall Animation Variables")]
		[SerializeField] private Text _startMessage = null;
		[SerializeField] private Ease _startMessageSubsequenceEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _startMessageSubsequenceDuration = 0f;
		[SerializeField] private Ease _cellMovementEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _cellMovementDuration = 0f;
		[SerializeField] [Range(0, 2)] private float _cellMovementDelay = 0f;
		[SerializeField] private Ease _pieceMovementEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _pieceMovementDuration = 0f;
		[SerializeField] [Range(0, 2)] private float _pieceMovementDelay = 0f;

		private const int CellZise = GlobalVariables.CellSize;
		private const int BoardSize = 8;

		private const int EnemyPawnColumnIndex = 7;
		private const int EnemyOfficerColumnIndex = 8;

		private const int PlayerPawnColumnIndex = 2;
		private const int PlayerOfficerColumnIndex = 1;

		private const int RookIndexFirst = 1;
		private const int RookIndexSecond = 8;
		private const int KnightIndexFirst = 2;
		private const int KnightIndexSecond = 7;
		private const int BishopIndexFirst = 3;
		private const int BishopIndexSecond = 6;
		private const int KingIndex = 4;
		private const int QueenIndex = 5;

		private Transform _cellRoot = null;
		private Transform _enemyRoot = null;
		private Transform _playerRoot = null;

		private Sequence _animationSequence = null;

		void Start()
		{
			_cellRoot = FindObjectOfType<CellRoot>().transform;
			_enemyRoot = FindObjectOfType<EnemyPiecesRoot>().transform;
			_playerRoot = FindObjectOfType<PlayerPiecesRoot>().transform;

			CreateBoard();
			StartCreationOfSidesEvent += CreatePieces;
			EnablePieceMovementEvent += ShowStartMessage;
		}

		private void CreateBoard()
		{
			_animationSequence = DOTween.Sequence();
			_animationSequence.Pause();

			var initialSpawnPosition =
				new Vector2(_cellSpawnPositionX, _cellSpawnPositionY);
			var initialCellPosition =
				new Vector3(_firstCellPositionX, _firstCellPositionY);

			for (int y = 0; y < BoardSize; y++)
			{
				for (int x = 0; x < BoardSize; x++)
				{
					var cellPosition =
						new Vector2(
							initialCellPosition.x + (BoardSize - x) * CellZise,
							initialCellPosition.y - y * CellZise);

					var cell = Instantiate(_cell, _cellRoot);
					cell.transform.localPosition = initialSpawnPosition;
					cell.SetPositionIndexes(BoardSize - y, BoardSize - x);
					cell.SetColor((x + y) % 2 == 1 ? _firstColor : _secondColor);
					cell.Side = Side.Free;

					GameData.Cells.Add(cell);

					var twener =
						cell.transform
							.DOLocalMove(cellPosition, _cellMovementDuration)
							.SetEase(_cellMovementEase)
							.SetDelay((x + y) * _cellMovementDelay);
					_animationSequence.Insert(0, twener);
				}

				_animationSequence.Play().OnComplete(() => CompleteAnimationEvent(StartCreationOfSidesEvent));
			}
		}

		private void CreatePieces()
		{
			_animationSequence = DOTween.Sequence();
			_animationSequence.Pause();

			CreateSide(
				_enemyRoot,
				Side.Enemy,
				EnemyPawnColumnIndex,
				EnemyOfficerColumnIndex,
				_spawnOffset);

			CreateSide(
				_playerRoot,
				Side.Player,
				PlayerPawnColumnIndex,
				PlayerOfficerColumnIndex,
				-_spawnOffset);

			_animationSequence.Play().OnComplete(() => CompleteAnimationEvent(EnablePieceMovementEvent));
		}

		private void ShowStartMessage()
		{
			_animationSequence = DOTween.Sequence();
			_animationSequence.Pause();

			var tweener =
				_startMessage
					.DOFade(1, _startMessageSubsequenceDuration)
					.SetEase(_startMessageSubsequenceEase);
			_animationSequence.Insert(0, tweener);

			tweener =
				_startMessage.transform
					.DOScale(1, _startMessageSubsequenceDuration)
					.SetEase(_startMessageSubsequenceEase);
			_animationSequence.Insert(0, tweener);

			tweener =
				_startMessage.transform
					.DOScale(3, _startMessageSubsequenceDuration)
					.SetEase(_startMessageSubsequenceEase)
					.SetDelay(_startMessageSubsequenceDuration);
			_animationSequence.Append(tweener);

			tweener =
				_startMessage
					.DOFade(0, _startMessageSubsequenceDuration)
					.SetEase(_startMessageSubsequenceEase)
					.SetDelay(_startMessageSubsequenceDuration);
			_animationSequence.Join(tweener);

			tweener = _startMessage.transform.DOScale(0, 0);
			_animationSequence.Append(tweener);

			tweener = _protector.transform.DOScale(0, 0);
			_animationSequence.Append(tweener);

			_animationSequence.Play();
		}

		private void CreateSide(
			Transform root,
			Side side,
			int pawnPositionColumnIndex,
			int officerPositionColumnIndex,
			int spawnOffset)
		{
			var pawnPositions =
				GameData.Cells
					.Where(x => x.Column == pawnPositionColumnIndex)
					.ToList()
					.OrderBy(x => x.Column);

			var officerPositions =
				GameData.Cells
					.Where(x => x.Column == officerPositionColumnIndex)
					.ToList()
					.OrderBy(x => x.Column);

			var index = 0;
			foreach (var cell in pawnPositions)
			{
				InitializePiece(root, cell, _pawn, side, spawnOffset, index);
				index++;
			}

			foreach (var cell in officerPositions)
			{
				switch (cell.Row)
				{
					case RookIndexFirst:
					case RookIndexSecond:
						InitializePiece(root, cell, _rook, side, spawnOffset, index);
						break;

					case KnightIndexFirst:
					case KnightIndexSecond:
						InitializePiece(root, cell, _knight, side, spawnOffset, index);
						break;

					case BishopIndexFirst:
					case BishopIndexSecond:
						InitializePiece(root, cell, _bishop, side, spawnOffset, index);
						break;

					case KingIndex:
						InitializePiece(root, cell, _king, side, spawnOffset, index);
						break;

					case QueenIndex:
						InitializePiece(root, cell, _queen, side, spawnOffset, index);
						break;
				}
				index++;
			}
		}

		private void InitializePiece(
			Transform root,
			Cell cell,
			BasePiece referencePiece,
			Side side,
			int spawnOffset,
			int index)
		{
			cell.Side = side;

			var endPosition = cell.transform.localPosition;
			var spawnPosition = new Vector2(endPosition.x, endPosition.y + spawnOffset);

			var piece = Instantiate(referencePiece, root);
			piece.transform.localPosition = spawnPosition;
			piece.SetSide(side);
			piece.SetPositionIndexes(cell.Column, cell.Row);

			GameData.Pieces.Add(piece);

			UpdateMovementAnimationSequence(piece, endPosition, index);
		}

		private void UpdateMovementAnimationSequence(
			BasePiece piece,
			Vector2 position,
			int delayMultiplier)
		{
			var tweener =
				piece.transform
					.DOLocalMove(position, _pieceMovementDuration)
					.SetEase(_pieceMovementEase)
					.SetDelay(_pieceMovementDelay * delayMultiplier);
			_animationSequence.Insert(0, tweener);
		}

		private void CompleteAnimationEvent(OnAnimationComplete animationCompleteEvent)
		{
			if (animationCompleteEvent != null) animationCompleteEvent();
			_animationSequence = null;
		}
	}
}
