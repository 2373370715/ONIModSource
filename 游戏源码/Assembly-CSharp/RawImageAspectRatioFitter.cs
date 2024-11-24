using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EBB RID: 7867
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class RawImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x0600A54C RID: 42316 RVA: 0x003EC0AC File Offset: 0x003EA2AC
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.texture != null)
		{
			base.aspectRatio = (float)this.targetImage.texture.width / (float)this.targetImage.texture.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x0600A54D RID: 42317 RVA: 0x0010B3DB File Offset: 0x001095DB
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600A54E RID: 42318 RVA: 0x0010B3E9 File Offset: 0x001095E9
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04008176 RID: 33142
	[SerializeField]
	private RawImage targetImage;
}
