using UnityEngine;

namespace ChessProto
{
	public class Cell : MonoBehaviour
	{
		[Header("Cell Variables")]
		[SerializeField] private Color _firstColor = Color.white;
		[SerializeField] private Color _secondColor = Color.white;
	}
}
