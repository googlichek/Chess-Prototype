using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessProto
{
	public class BasePiece : MonoBehaviour, IPointerClickHandler
	{
		public delegate void OnChessPieceClicked();
		public event OnChessPieceClicked ChessPieceClickedEvent;

		[SerializeField] private Sprite _whitePiece = null;
		[SerializeField] private Sprite _blackPiece = null;

		public void SetSideColor(SideColor sideColor)
		{
			Image image = GetComponent<Image>();
			if (image == null) return;

			switch (sideColor)
			{
				case SideColor.Black:
					image.sprite = _blackPiece;
					break;
				case SideColor.White:
					image.sprite = _whitePiece;
					break;
			}
		}

		public virtual void Move()
		{
		}

		public virtual void HighlightPositions()
		{
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log(gameObject.name + " was clicked");
		}
	}
}
