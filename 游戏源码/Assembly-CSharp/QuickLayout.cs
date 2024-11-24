using System;
using UnityEngine;

// Token: 0x02001EB9 RID: 7865
public class QuickLayout : KMonoBehaviour
{
	// Token: 0x0600A544 RID: 42308 RVA: 0x0010B3B3 File Offset: 0x001095B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ForceUpdate();
	}

	// Token: 0x0600A545 RID: 42309 RVA: 0x0010B3C1 File Offset: 0x001095C1
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x0600A546 RID: 42310 RVA: 0x0010B3C9 File Offset: 0x001095C9
	private void LateUpdate()
	{
		this.Run(false);
	}

	// Token: 0x0600A547 RID: 42311 RVA: 0x0010B3D2 File Offset: 0x001095D2
	public void ForceUpdate()
	{
		this.Run(true);
	}

	// Token: 0x0600A548 RID: 42312 RVA: 0x003EBDC4 File Offset: 0x003E9FC4
	private void Run(bool forceUpdate = false)
	{
		forceUpdate = (forceUpdate || this._elementSize != this.elementSize);
		forceUpdate = (forceUpdate || this._spacing != this.spacing);
		forceUpdate = (forceUpdate || this._layoutDirection != this.layoutDirection);
		forceUpdate = (forceUpdate || this._offset != this.offset);
		if (forceUpdate)
		{
			this._elementSize = this.elementSize;
			this._spacing = this.spacing;
			this._layoutDirection = this.layoutDirection;
			this._offset = this.offset;
		}
		int num = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		if (num != this.oldActiveChildCount || forceUpdate)
		{
			this.Layout();
			this.oldActiveChildCount = num;
		}
	}

	// Token: 0x0600A549 RID: 42313 RVA: 0x003EBEBC File Offset: 0x003EA0BC
	public void Layout()
	{
		Vector3 vector = this._offset;
		bool flag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				flag = true;
				base.transform.GetChild(i).rectTransform().anchoredPosition = vector;
				vector += (float)(this._elementSize + this._spacing) * this.GetDirectionVector();
			}
		}
		if (this.driveParentRectSize != null)
		{
			if (!flag)
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0f);
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
					return;
				}
			}
			else
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(vector.y));
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(vector.x), this.driveParentRectSize.sizeDelta.y);
				}
			}
		}
	}

	// Token: 0x0600A54A RID: 42314 RVA: 0x003EC054 File Offset: 0x003EA254
	private Vector2 GetDirectionVector()
	{
		Vector2 result = Vector3.zero;
		switch (this._layoutDirection)
		{
		case QuickLayout.LayoutDirection.TopToBottom:
			result = Vector2.down;
			break;
		case QuickLayout.LayoutDirection.BottomToTop:
			result = Vector2.up;
			break;
		case QuickLayout.LayoutDirection.LeftToRight:
			result = Vector2.right;
			break;
		case QuickLayout.LayoutDirection.RightToLeft:
			result = Vector2.left;
			break;
		}
		return result;
	}

	// Token: 0x04008167 RID: 33127
	[Header("Configuration")]
	[SerializeField]
	private int elementSize;

	// Token: 0x04008168 RID: 33128
	[SerializeField]
	private int spacing;

	// Token: 0x04008169 RID: 33129
	[SerializeField]
	private QuickLayout.LayoutDirection layoutDirection;

	// Token: 0x0400816A RID: 33130
	[SerializeField]
	private Vector2 offset;

	// Token: 0x0400816B RID: 33131
	[SerializeField]
	private RectTransform driveParentRectSize;

	// Token: 0x0400816C RID: 33132
	private int _elementSize;

	// Token: 0x0400816D RID: 33133
	private int _spacing;

	// Token: 0x0400816E RID: 33134
	private QuickLayout.LayoutDirection _layoutDirection;

	// Token: 0x0400816F RID: 33135
	private Vector2 _offset;

	// Token: 0x04008170 RID: 33136
	private int oldActiveChildCount;

	// Token: 0x02001EBA RID: 7866
	private enum LayoutDirection
	{
		// Token: 0x04008172 RID: 33138
		TopToBottom,
		// Token: 0x04008173 RID: 33139
		BottomToTop,
		// Token: 0x04008174 RID: 33140
		LeftToRight,
		// Token: 0x04008175 RID: 33141
		RightToLeft
	}
}
