using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class RawImageAspectRatioFitter : AspectRatioFitter
{
	[SerializeField]
	private RawImage targetImage;

	private void UpdateAspectRatio()
	{
		if (targetImage != null && targetImage.texture != null)
		{
			base.aspectRatio = (float)targetImage.texture.width / (float)targetImage.texture.height;
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
