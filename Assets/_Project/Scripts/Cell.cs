using UnityEngine;
using UnityEngine.UI;

namespace ChessProto
{
	public class Cell : MonoBehaviour
	{
		[Header("Cell Variables")]
		public int Row = 0;
		public int Column = 0;

		private Image _image = null;

		void OnEnable()
		{
			_image = GetComponent<Image>();
		}

		public void SetColor(Color color)
		{
			_image.color = color;
		}
	}
}
