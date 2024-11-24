using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodexImageLayoutMB : UIBehaviour
{
	public RectTransform rectTransform;

	public LayoutElement layoutElement;

	public Image image;

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (image.preserveAspect && image.sprite != null && (bool)image.sprite)
		{
			float num = image.sprite.rect.height / image.sprite.rect.width;
			layoutElement.preferredHeight = num * rectTransform.sizeDelta.x;
			layoutElement.minHeight = layoutElement.preferredHeight;
			return;
		}
		layoutElement.preferredHeight = -1f;
		layoutElement.preferredWidth = -1f;
		layoutElement.minHeight = -1f;
		layoutElement.minWidth = -1f;
		layoutElement.flexibleHeight = -1f;
		layoutElement.flexibleWidth = -1f;
		layoutElement.ignoreLayout = false;
	}
}
