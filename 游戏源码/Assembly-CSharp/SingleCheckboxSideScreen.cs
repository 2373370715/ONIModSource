using System;
using UnityEngine;

// Token: 0x02001FCE RID: 8142
public class SingleCheckboxSideScreen : SideScreenContent
{
	// Token: 0x0600AC58 RID: 44120 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AC59 RID: 44121 RVA: 0x00110055 File Offset: 0x0010E255
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggle.onValueChanged += this.OnValueChanged;
	}

	// Token: 0x0600AC5A RID: 44122 RVA: 0x00110074 File Offset: 0x0010E274
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ICheckboxControl>() != null || target.GetSMI<ICheckboxControl>() != null;
	}

	// Token: 0x0600AC5B RID: 44123 RVA: 0x0040E508 File Offset: 0x0040C708
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.target = target.GetComponent<ICheckboxControl>();
		if (this.target == null)
		{
			this.target = target.GetSMI<ICheckboxControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The target provided does not have an ICheckboxControl component");
			return;
		}
		this.label.text = this.target.CheckboxLabel;
		this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
		this.titleKey = this.target.CheckboxTitleKey;
		this.toggle.isOn = this.target.GetCheckboxValue();
		this.toggleCheckMark.enabled = this.toggle.isOn;
	}

	// Token: 0x0600AC5C RID: 44124 RVA: 0x00110089 File Offset: 0x0010E289
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.target = null;
	}

	// Token: 0x0600AC5D RID: 44125 RVA: 0x00110098 File Offset: 0x0010E298
	private void OnValueChanged(bool value)
	{
		this.target.SetCheckboxValue(value);
		this.toggleCheckMark.enabled = value;
	}

	// Token: 0x04008769 RID: 34665
	public KToggle toggle;

	// Token: 0x0400876A RID: 34666
	public KImage toggleCheckMark;

	// Token: 0x0400876B RID: 34667
	public LocText label;

	// Token: 0x0400876C RID: 34668
	private ICheckboxControl target;
}
