using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CFF RID: 7423
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x06009AE9 RID: 39657 RVA: 0x003BC7E0 File Offset: 0x003BA9E0
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.sprite != null)
		{
			base.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x06009AEA RID: 39658 RVA: 0x00104C37 File Offset: 0x00102E37
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x06009AEB RID: 39659 RVA: 0x00104C45 File Offset: 0x00102E45
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x0400791A RID: 31002
	[SerializeField]
	private Image targetImage;
}
