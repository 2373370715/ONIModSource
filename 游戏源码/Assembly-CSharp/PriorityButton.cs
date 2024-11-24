using System;
using UnityEngine;

// Token: 0x02001EB2 RID: 7858
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x0600A503 RID: 42243 RVA: 0x0010B0A0 File Offset: 0x001092A0
	// (set) Token: 0x0600A504 RID: 42244 RVA: 0x003EA744 File Offset: 0x003E8944
	public PrioritySetting priority
	{
		get
		{
			return this._priority;
		}
		set
		{
			this._priority = value;
			if (this.its != null)
			{
				if (this.priority.priority_class == PriorityScreen.PriorityClass.high)
				{
					this.its.colorStyleSetting = this.highStyle;
				}
				else
				{
					this.its.colorStyleSetting = this.normalStyle;
				}
				this.its.RefreshColorStyle();
				this.its.ResetColor();
			}
		}
	}

	// Token: 0x0600A505 RID: 42245 RVA: 0x0010B0A8 File Offset: 0x001092A8
	protected override void OnPrefabInit()
	{
		this.toggle.onClick += this.OnClick;
	}

	// Token: 0x0600A506 RID: 42246 RVA: 0x0010B0C1 File Offset: 0x001092C1
	private void OnClick()
	{
		if (this.playSelectionSound)
		{
			PriorityScreen.PlayPriorityConfirmSound(this.priority);
		}
		if (this.onClick != null)
		{
			this.onClick(this.priority);
		}
	}

	// Token: 0x04008128 RID: 33064
	public KToggle toggle;

	// Token: 0x04008129 RID: 33065
	public LocText text;

	// Token: 0x0400812A RID: 33066
	public ToolTip tooltip;

	// Token: 0x0400812B RID: 33067
	[MyCmpGet]
	private ImageToggleState its;

	// Token: 0x0400812C RID: 33068
	public ColorStyleSetting normalStyle;

	// Token: 0x0400812D RID: 33069
	public ColorStyleSetting highStyle;

	// Token: 0x0400812E RID: 33070
	public bool playSelectionSound = true;

	// Token: 0x0400812F RID: 33071
	public Action<PrioritySetting> onClick;

	// Token: 0x04008130 RID: 33072
	private PrioritySetting _priority;
}
