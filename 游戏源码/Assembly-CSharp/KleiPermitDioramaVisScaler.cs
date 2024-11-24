using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	public const float REFERENCE_WIDTH = 1700f;

	public const float REFERENCE_HEIGHT = 800f;

	[SerializeField]
	private RectTransform root;

	[SerializeField]
	private RectTransform scaleTarget;

	[SerializeField]
	private RectTransform slot;

	protected override void OnRectTransformDimensionsChange()
	{
		Layout();
	}

	public void Layout()
	{
		Layout(root, scaleTarget, slot);
	}

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
		float num3 = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * num3;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}
}
