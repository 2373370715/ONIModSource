using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D07 RID: 7431
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
	// Token: 0x06009B18 RID: 39704 RVA: 0x003BCBA8 File Offset: 0x003BADA8
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
		float num = sprite.rect.width / sprite.rect.height;
		this.layout.preferredWidth = this.layout.preferredHeight * num;
	}

	// Token: 0x0400793A RID: 31034
	[SerializeField]
	private Image image;

	// Token: 0x0400793B RID: 31035
	[SerializeField]
	private LayoutElement layout;
}
