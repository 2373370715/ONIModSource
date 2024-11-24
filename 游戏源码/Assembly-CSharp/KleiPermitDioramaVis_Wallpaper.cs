using Database;
using UnityEngine;
using UnityEngine.UI;

public class KleiPermitDioramaVis_Wallpaper : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private Image itemSprite;

	private bool itemSpriteDidInit;

	private Vector2 itemSpritePosStart;

	private Vector2 itemSpritePosEnd;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
	}

	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		itemSprite.rectTransform().sizeDelta = Vector2.one * 176f;
		itemSprite.sprite = permitPresentationInfo.sprite;
		if (!itemSpriteDidInit)
		{
			itemSpriteDidInit = true;
			itemSpritePosStart = itemSprite.rectTransform.anchoredPosition + new Vector2(0f, 16f);
			itemSpritePosEnd = itemSprite.rectTransform.anchoredPosition;
		}
		itemSprite.StartCoroutine(Updater.Parallel(Updater.Ease(delegate(float alpha)
		{
			itemSprite.color = new Color(1f, 1f, 1f, alpha);
		}, 0f, 1f, 0.2f, Easing.SmoothStep, 0.1f), Updater.Ease(delegate(Vector2 position)
		{
			itemSprite.rectTransform.anchoredPosition = position;
		}, itemSpritePosStart, itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)));
	}
}
