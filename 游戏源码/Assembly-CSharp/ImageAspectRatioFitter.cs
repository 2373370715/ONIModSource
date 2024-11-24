using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	[SerializeField]
	private Image targetImage;

	private void UpdateAspectRatio()
	{
		if (targetImage != null && targetImage.sprite != null)
		{
			base.aspectRatio = targetImage.sprite.rect.width / targetImage.sprite.rect.height;
		}
		else
		{
			base.aspectRatio = 1f;
		}
	}

	protected override void OnTransformParentChanged()
	{
		UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}
}
