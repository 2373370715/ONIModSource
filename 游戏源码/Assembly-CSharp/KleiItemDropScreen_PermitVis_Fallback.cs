using UnityEngine;
using UnityEngine.UI;

public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	[SerializeField]
	private Image sprite;

	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		sprite.sprite = info.Sprite;
	}
}
