using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D65 RID: 7525
public class KleiPermitDioramaVis_Wallpaper : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D26 RID: 40230 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D27 RID: 40231 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009D28 RID: 40232 RVA: 0x003C68CC File Offset: 0x003C4ACC
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemSprite.rectTransform().sizeDelta = Vector2.one * 176f;
		this.itemSprite.sprite = permitPresentationInfo.sprite;
		if (!this.itemSpriteDidInit)
		{
			this.itemSpriteDidInit = true;
			this.itemSpritePosStart = this.itemSprite.rectTransform.anchoredPosition + new Vector2(0f, 16f);
			this.itemSpritePosEnd = this.itemSprite.rectTransform.anchoredPosition;
		}
		this.itemSprite.StartCoroutine(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float alpha)
			{
				this.itemSprite.color = new Color(1f, 1f, 1f, alpha);
			}, 0f, 1f, 0.2f, Easing.SmoothStep, 0.1f),
			Updater.Ease(delegate(Vector2 position)
			{
				this.itemSprite.rectTransform.anchoredPosition = position;
			}, this.itemSpritePosStart, this.itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)
		}));
	}

	// Token: 0x04007B29 RID: 31529
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04007B2A RID: 31530
	private bool itemSpriteDidInit;

	// Token: 0x04007B2B RID: 31531
	private Vector2 itemSpritePosStart;

	// Token: 0x04007B2C RID: 31532
	private Vector2 itemSpritePosEnd;
}
