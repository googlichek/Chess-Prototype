using UnityEngine;
using UnityEngine.UI;

namespace ChessProto
{
	public class ChessPiece : MonoBehaviour, IChessPiece
	{
		[SerializeField] Sprite _whitePiece = null;
		[SerializeField] Sprite _blackPiece = null;

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
	}
}
