using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessProto
{
	public class BasePiece : MonoBehaviour, IPointerClickHandler
	{
		public delegate void OnChessPieceClicked(BasePiece piece);
		public event OnChessPieceClicked ChessPieceClickedEvent;

		[SerializeField] private Sprite _whitePiece = null;
		[SerializeField] private Sprite _blackPiece = null;

		public void SetSideColor(Side side)
		{
			Image image = GetComponent<Image>();
			if (image == null) return;

			switch (side)
			{
				case Side.Enemy:
					image.sprite = _blackPiece;
					break;
				case Side.Player:
					image.sprite = _whitePiece;
					break;
			}
		}

		public virtual void Move(Side side)
		{
		}

		public virtual void HighlightPositions(Side side)
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (ChessPieceClickedEvent != null) ChessPieceClickedEvent(this);
		}
	}
}
