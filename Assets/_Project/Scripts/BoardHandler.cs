using DG.Tweening;
using UnityEngine;

namespace ChessProto
{
	public class BoardHandler : MonoBehaviour
	{
		[Header("Cell Placement Variables")]
		[SerializeField] private Cell _cell = null;
		[SerializeField] private Color _firstColor = Color.white;
		[SerializeField] private Color _secondColor = Color.white;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionX = 0;
		[SerializeField] [Range(-1024, 1024)] private int _firstCellPositionY = 0;
		[SerializeField] [Range(-2000, 2000)] private int _cellSpawnPositionX = -2000;
		[SerializeField] [Range(-2000, 2000)] private int _cellSpawnPositionY = 0;
		
		[Header("Tweening Variables")]
		[SerializeField] private Ease _cellMovementEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _cellMovementDuration = 0.5f;
		[SerializeField] [Range(0, 2)] private float _cellMovementDelay = 0.1f;

		private const int CellZise = GlobalVariables.CellSize;
		private const int BoardSize = 8;

		private Transform _cellRoot = null;

		void Start()
		{
			_cellRoot = FindObjectOfType<CellRoot>().transform;

			SetUpBoard();
		}

		private void SetUpBoard()
		{
			CreateBoard();
		}

		private void CreateBoard()
		{
			Vector2 initialSpawnPosition =
				new Vector2(_cellSpawnPositionX, _cellSpawnPositionY);

			Vector2 initialCellPosition =
				new Vector3(_firstCellPositionX, _firstCellPositionY);

			for (int y = 0; y < BoardSize; y++)
			{
				Vector2 spawnPosition =
					new Vector2(initialSpawnPosition.x, initialSpawnPosition.y - y * CellZise);

				Debug.Log(spawnPosition);

				for (int x = 0; x < BoardSize; x++)
				{
					Vector2 cellPosition =
						new Vector2(
							initialCellPosition.x + (BoardSize - x) * CellZise,
							initialCellPosition.y - y * CellZise);

					Cell cell = Instantiate(_cell, spawnPosition, Quaternion.identity, _cellRoot);

					cell.Column = BoardSize - x;
					cell.Row = y + 1;

					cell.SetColor((x + y) % 2 == 1 ? _firstColor : _secondColor);

					cell.transform
						.DOLocalMove(cellPosition, _cellMovementDuration)
						.SetEase(_cellMovementEase)
						.SetDelay((x + y) * _cellMovementDelay);
				}
			}
		}
	}
}
