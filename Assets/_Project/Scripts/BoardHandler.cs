using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace ChessProto
{
	public class BoardHandler : MonoBehaviour
	{
		private delegate void OnAnimationComplete();
		private event OnAnimationComplete StartCreationOfSides;

		[Header("Cell Placement Variables")]
		[SerializeField] private Cell _cell = null;
		[SerializeField] private Color _firstColor = Color.white;
		[SerializeField] private Color _secondColor = Color.white;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionX = 0;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionY = 0;
		[SerializeField] [Range(-2500, 2500)] private int _cellSpawnPositionX = 0;
		[SerializeField] [Range(-2500, 2500)] private int _cellSpawnPositionY = 0;

		[Header("Piece Placement Variables")]
		[SerializeField] private ChessPiece _bishop = null;
		[SerializeField] private ChessPiece _king = null;
		[SerializeField] private ChessPiece _knight = null;
		[SerializeField] private ChessPiece _pawn = null;
		[SerializeField] private ChessPiece _queen = null;
		[SerializeField] private ChessPiece _rook = null;
		[SerializeField] [Range(0, 1000)] private int _spawnOffset = 0;

		[Header("Tweening Variables")]
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

		private readonly List<Cell> _cells = new List<Cell>();
		private List<IChessPiece> _enemyPieces = new List<IChessPiece>();
		private List<IChessPiece> _playerPieces = new List<IChessPiece>();

		private Sequence _sequence = null;

		void Start()
		{
			_cellRoot = FindObjectOfType<CellRoot>().transform;
			_enemyRoot = FindObjectOfType<EnemyPiecesRoot>().transform;
			_playerRoot = FindObjectOfType<PlayerPiecesRoot>().transform;

			CreateBoard();
			StartCreationOfSides += CreatePieces;
		}

		private void CreateBoard()
		{
			_sequence = DOTween.Sequence();
			_sequence.Pause();

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
					cell.SetIndexes(BoardSize - y, BoardSize - x);
					cell.SetColor((x + y) % 2 == 1 ? _firstColor : _secondColor);

					_cells.Add(cell);

					var twener =
						cell.transform
							.DOLocalMove(cellPosition, _cellMovementDuration)
							.SetEase(_cellMovementEase)
							.SetDelay((x + y) * _cellMovementDelay);
					_sequence.Insert(0, twener);
				}

				_sequence.Play().OnComplete(() =>
				{
					if (StartCreationOfSides != null) StartCreationOfSides();
					_sequence = null;
				});
			}
		}

		private void CreatePieces()
		{
			_sequence = DOTween.Sequence();
			_sequence.Pause();

			CreateSide(
				_enemyPieces,
				_enemyRoot,
				SideColor.Black,
				EnemyPawnColumnIndex,
				EnemyOfficerColumnIndex,
				_spawnOffset);

			CreateSide(
				_playerPieces,
				_playerRoot,
				SideColor.White,
				PlayerPawnColumnIndex,
				PlayerOfficerColumnIndex,
				-_spawnOffset);

			_sequence.Play();
		}

		private void CreateSide(
			List<IChessPiece> sidePieces,
			Transform root,
			SideColor sideColor,
			int pawnPositionColumnIndex,
			int officerPositionColumnIndex,
			int spawnOffset)
		{
			var pawnPositions =
				_cells
					.Where(x => x.Column == pawnPositionColumnIndex)
					.ToList()
					.OrderBy(x => x.Column);

			var officerPositions =
				_cells
					.Where(x => x.Column == officerPositionColumnIndex)
					.ToList()
					.OrderBy(x => x.Column);

			int index = 0;
			foreach (var cell in pawnPositions)
			{
				InitializePiece(
					sidePieces, root, cell, _pawn, sideColor, spawnOffset, index);
				index++;
			}

			foreach (var cell in officerPositions)
			{
				switch (cell.Row)
				{
					case RookIndexFirst:
					case RookIndexSecond:
						InitializePiece(
							sidePieces, root, cell, _rook, sideColor, spawnOffset, index);
						break;

					case KnightIndexFirst:
					case KnightIndexSecond:
						InitializePiece(
							sidePieces, root, cell, _knight, sideColor, spawnOffset, index);
						break;

					case BishopIndexFirst:
					case BishopIndexSecond:
						InitializePiece(
							sidePieces, root, cell, _bishop, sideColor, spawnOffset, index);
						break;

					case KingIndex:
						InitializePiece(
							sidePieces, root, cell, _king, sideColor, spawnOffset, index);
						break;

					case QueenIndex:
						InitializePiece(
							sidePieces, root, cell, _queen, sideColor, spawnOffset, index);
						break;
				}

				index++;
			}
		}

		private void InitializePiece(
			List<IChessPiece> sidePieces,
			Transform root,
			Cell cell,
			ChessPiece referencePiece,
			SideColor sideColor,
			int spawnOffset,
			int index)
		{
			var endPosition = cell.transform.localPosition;
			var spawnPosition = new Vector2(endPosition.x, endPosition.y + spawnOffset);

			var piece = SpawnPiece(root, referencePiece, sideColor, spawnPosition);
			sidePieces.Add(piece);

			UpdateMovementAnimationSequence(piece, endPosition, index);
		}

		private ChessPiece SpawnPiece(
			Transform root,
			ChessPiece referencePiece,
			SideColor sideColor,
			Vector2 position)
		{
			var piece = Instantiate(referencePiece, root);
			piece.transform.localPosition = position;
			piece.SetSideColor(sideColor);

			return piece;
		}

		private void UpdateMovementAnimationSequence(
			ChessPiece piece,
			Vector2 position,
			int delayMultiplier)
		{
			var tweener =
				piece.transform
					.DOLocalMove(position, _pieceMovementDuration)
					.SetEase(_pieceMovementEase)
					.SetDelay(_pieceMovementDelay * delayMultiplier);
			_sequence.Insert(0, tweener);
		}
	}
}
