using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E3E RID: 7742
public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
	// Token: 0x0600A238 RID: 41528 RVA: 0x003DC52C File Offset: 0x003DA72C
	private void UpdateState()
	{
		MotdBox_ImageButtonLayoutElement.Style style = this.style;
		if (style == MotdBox_ImageButtonLayoutElement.Style.WidthExpandsBasedOnHeight)
		{
			this.flexibleHeight = 1f;
			this.preferredHeight = -1f;
			this.minHeight = -1f;
			this.flexibleWidth = 0f;
			this.preferredWidth = this.rectTransform().sizeDelta.y * this.heightToWidthRatio;
			this.minWidth = this.preferredWidth;
			this.ignoreLayout = false;
			return;
		}
		if (style != MotdBox_ImageButtonLayoutElement.Style.HeightExpandsBasedOnWidth)
		{
			return;
		}
		this.flexibleWidth = 1f;
		this.preferredWidth = -1f;
		this.minWidth = -1f;
		this.flexibleHeight = 0f;
		this.preferredHeight = this.rectTransform().sizeDelta.x / this.heightToWidthRatio;
		this.minHeight = this.preferredHeight;
		this.ignoreLayout = false;
	}

	// Token: 0x0600A239 RID: 41529 RVA: 0x0010943F File Offset: 0x0010763F
	protected override void OnTransformParentChanged()
	{
		this.UpdateState();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600A23A RID: 41530 RVA: 0x0010944D File Offset: 0x0010764D
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateState();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04007E8E RID: 32398
	[SerializeField]
	private float heightToWidthRatio;

	// Token: 0x04007E8F RID: 32399
	[SerializeField]
	private MotdBox_ImageButtonLayoutElement.Style style;

	// Token: 0x02001E3F RID: 7743
	private enum Style
	{
		// Token: 0x04007E91 RID: 32401
		WidthExpandsBasedOnHeight,
		// Token: 0x04007E92 RID: 32402
		HeightExpandsBasedOnWidth
	}
}
