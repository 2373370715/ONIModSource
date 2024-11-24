using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001D56 RID: 7510
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	// Token: 0x06009CE3 RID: 40163 RVA: 0x0010614E File Offset: 0x0010434E
	protected override void OnRectTransformDimensionsChange()
	{
		this.Layout();
	}

	// Token: 0x06009CE4 RID: 40164 RVA: 0x00106156 File Offset: 0x00104356
	public void Layout()
	{
		KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);
	}

	// Token: 0x06009CE5 RID: 40165 RVA: 0x003C6104 File Offset: 0x003C4304
	public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
	{
		float aspectRatio = 2.125f;
		AspectRatioFitter aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
		aspectRatioFitter.aspectRatio = aspectRatio;
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
		float num = 1700f;
		float a = Mathf.Max(0.1f, root.rect.width) / num;
		float num2 = 800f;
		float b = Mathf.Max(0.1f, root.rect.height) / num2;
		float d = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * d;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}

	// Token: 0x04007AFB RID: 31483
	public const float REFERENCE_WIDTH = 1700f;

	// Token: 0x04007AFC RID: 31484
	public const float REFERENCE_HEIGHT = 800f;

	// Token: 0x04007AFD RID: 31485
	[SerializeField]
	private RectTransform root;

	// Token: 0x04007AFE RID: 31486
	[SerializeField]
	private RectTransform scaleTarget;

	// Token: 0x04007AFF RID: 31487
	[SerializeField]
	private RectTransform slot;
}
