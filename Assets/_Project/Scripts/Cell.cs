using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessProto
{
	/// <summary>
	/// Describes cell behaviour.
	/// </summary>
	public class Cell : MonoBehaviour, IPointerClickHandler
	{
		public delegate void OnCellClicked(Cell cell);
		public event OnCellClicked CellClickedEvent;

		/// <summary>
		/// Row index of the piece.
		/// </summary>
		public int Row { get { return _row; } }

		/// <summary>
		/// Column index of the piece.
		/// </summary>
		public int Column { get { return _column; } }

		/// <summary>
		/// Indicates to which side this cell belongs.
		/// </summary>
		[Header("Cell Variables")]
		public Side Side = Side.None;
		[SerializeField] private Image _border = null;
		[SerializeField] private Ease _animationEase = Ease.Linear;
		[SerializeField] [Range(0, 2)] private float _scaleAmount = 0;
		[SerializeField] [Range(0, 1)] private float _animationDuration = 0;

		private Image _image = null;

		private int _row = 0;
		private int _column = 0;

		void OnEnable()
		{
			_image = GetComponent<Image>();
		}

		/// <summary>
		/// Sets cell color.
		/// </summary>
		/// <param name="color">Given color.</param>
		public void SetColor(Color color)
		{
			_image.color = color;
		}

		/// <summary>
		/// Sets board position indexes of this cell.
		/// </summary>
		/// <param name="column">Column index.</param>
		/// <param name="row">Row index.</param>
		public void SetPositionIndexes(int column, int row)
		{
			_row = row;
			_column = column;
		}

		/// <summary>
		/// Highlights cell.
		/// </summary>
		public void Highlight()
		{
			_border.DOKill();
			_border.transform.DOScale(1, 0);

			_border.DOFade(1, _animationDuration).SetEase(_animationEase);
			_border.transform
				.DOScale(_scaleAmount, _animationDuration)
				.SetEase(_animationEase)
				.SetLoops(-1, LoopType.Yoyo);
		}

		/// <summary>
		/// Stops cell highlighting.
		/// </summary>
		public void StopHighlighting()
		{
			_border.DOFade(0, _animationDuration).SetEase(_animationEase);
			_border.transform.DOScale(1, _animationDuration).SetEase(_animationEase);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (CellClickedEvent != null) CellClickedEvent(this);
		}
	}
}
