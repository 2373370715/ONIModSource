using UnityEngine;
using UnityEngine.UI;

public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
	private enum Style
	{
		WidthExpandsBasedOnHeight,
		HeightExpandsBasedOnWidth
	}

	[SerializeField]
	private float heightToWidthRatio;

	[SerializeField]
	private Style style;

	private void UpdateState()
	{
		switch (style)
		{
		case Style.WidthExpandsBasedOnHeight:
			flexibleHeight = 1f;
			preferredHeight = -1f;
			minHeight = -1f;
			flexibleWidth = 0f;
			preferredWidth = this.rectTransform().sizeDelta.y * heightToWidthRatio;
			minWidth = preferredWidth;
			ignoreLayout = false;
			break;
		case Style.HeightExpandsBasedOnWidth:
			flexibleWidth = 1f;
			preferredWidth = -1f;
			minWidth = -1f;
			flexibleHeight = 0f;
			preferredHeight = this.rectTransform().sizeDelta.x / heightToWidthRatio;
			minHeight = preferredHeight;
			ignoreLayout = false;
			break;
		}
	}

	protected override void OnTransformParentChanged()
	{
		UpdateState();
		base.OnTransformParentChanged();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		UpdateState();
		base.OnRectTransformDimensionsChange();
	}
}
