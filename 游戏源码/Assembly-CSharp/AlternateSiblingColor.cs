using UnityEngine;
using UnityEngine.UI;

public class AlternateSiblingColor : KMonoBehaviour
{
	public Color evenColor;

	public Color oddColor;

	public Image image;

	private int mySiblingIndex;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		RefreshColor(siblingIndex % 2 == 0);
	}

	private void RefreshColor(bool evenIndex)
	{
		if (!(image == null))
		{
			image.color = (evenIndex ? evenColor : oddColor);
		}
	}

	private void Update()
	{
		if (mySiblingIndex != base.transform.GetSiblingIndex())
		{
			mySiblingIndex = base.transform.GetSiblingIndex();
			RefreshColor(mySiblingIndex % 2 == 0);
		}
	}
}
