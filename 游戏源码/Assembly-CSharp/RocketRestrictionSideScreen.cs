using System;
using UnityEngine;

// Token: 0x02001FBE RID: 8126
public class RocketRestrictionSideScreen : SideScreenContent
{
	// Token: 0x0600ABE0 RID: 44000 RVA: 0x0010FA82 File Offset: 0x0010DC82
	protected override void OnSpawn()
	{
		this.unrestrictedButton.onClick += this.ClickNone;
		this.spaceRestrictedButton.onClick += this.ClickSpace;
	}

	// Token: 0x0600ABE1 RID: 44001 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x0600ABE2 RID: 44002 RVA: 0x0010FAB2 File Offset: 0x0010DCB2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<RocketControlStation.StatesInstance>() != null;
	}

	// Token: 0x0600ABE3 RID: 44003 RVA: 0x0010FABD File Offset: 0x0010DCBD
	public override void SetTarget(GameObject new_target)
	{
		this.controlStation = new_target.GetComponent<RocketControlStation>();
		this.controlStationLogicSubHandle = this.controlStation.Subscribe(1861523068, new Action<object>(this.UpdateButtonStates));
		this.UpdateButtonStates(null);
	}

	// Token: 0x0600ABE4 RID: 44004 RVA: 0x0010FAF4 File Offset: 0x0010DCF4
	public override void ClearTarget()
	{
		if (this.controlStationLogicSubHandle != -1 && this.controlStation != null)
		{
			this.controlStation.Unsubscribe(this.controlStationLogicSubHandle);
			this.controlStationLogicSubHandle = -1;
		}
		this.controlStation = null;
	}

	// Token: 0x0600ABE5 RID: 44005 RVA: 0x0040C288 File Offset: 0x0040A488
	private void UpdateButtonStates(object data = null)
	{
		bool flag = this.controlStation.IsLogicInputConnected();
		if (!flag)
		{
			this.unrestrictedButton.isOn = !this.controlStation.RestrictWhenGrounded;
			this.spaceRestrictedButton.isOn = this.controlStation.RestrictWhenGrounded;
		}
		this.unrestrictedButton.gameObject.SetActive(!flag);
		this.spaceRestrictedButton.gameObject.SetActive(!flag);
		this.automationControlled.gameObject.SetActive(flag);
	}

	// Token: 0x0600ABE6 RID: 44006 RVA: 0x0010FB2C File Offset: 0x0010DD2C
	private void ClickNone()
	{
		this.controlStation.RestrictWhenGrounded = false;
		this.UpdateButtonStates(null);
	}

	// Token: 0x0600ABE7 RID: 44007 RVA: 0x0010FB41 File Offset: 0x0010DD41
	private void ClickSpace()
	{
		this.controlStation.RestrictWhenGrounded = true;
		this.UpdateButtonStates(null);
	}

	// Token: 0x04008713 RID: 34579
	private RocketControlStation controlStation;

	// Token: 0x04008714 RID: 34580
	[Header("Buttons")]
	public KToggle unrestrictedButton;

	// Token: 0x04008715 RID: 34581
	public KToggle spaceRestrictedButton;

	// Token: 0x04008716 RID: 34582
	public GameObject automationControlled;

	// Token: 0x04008717 RID: 34583
	private int controlStationLogicSubHandle = -1;
}
