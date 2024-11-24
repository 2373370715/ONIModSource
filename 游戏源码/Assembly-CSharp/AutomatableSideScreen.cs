using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F34 RID: 7988
public class AutomatableSideScreen : SideScreenContent
{
	// Token: 0x0600A888 RID: 43144 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600A889 RID: 43145 RVA: 0x003FD6AC File Offset: 0x003FB8AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
		this.allowManualToggle.onValueChanged += this.OnAllowManualChanged;
	}

	// Token: 0x0600A88A RID: 43146 RVA: 0x0010D78B File Offset: 0x0010B98B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Automatable>() != null;
	}

	// Token: 0x0600A88B RID: 43147 RVA: 0x003FD6FC File Offset: 0x003FB8FC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetAutomatable = target.GetComponent<Automatable>();
		if (this.targetAutomatable == null)
		{
			global::Debug.LogError("The target provided does not have an Automatable component");
			return;
		}
		this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
		this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
	}

	// Token: 0x0600A88C RID: 43148 RVA: 0x0010D799 File Offset: 0x0010B999
	private void OnAllowManualChanged(bool value)
	{
		this.targetAutomatable.SetAutomationOnly(!value);
		this.allowManualToggleCheckMark.enabled = value;
	}

	// Token: 0x04008481 RID: 33921
	public KToggle allowManualToggle;

	// Token: 0x04008482 RID: 33922
	public KImage allowManualToggleCheckMark;

	// Token: 0x04008483 RID: 33923
	public GameObject content;

	// Token: 0x04008484 RID: 33924
	private GameObject target;

	// Token: 0x04008485 RID: 33925
	public LocText DescriptionText;

	// Token: 0x04008486 RID: 33926
	private Automatable targetAutomatable;
}
