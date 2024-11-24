using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000985 RID: 2437
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000BC8F2 File Offset: 0x000BAAF2
	public bool IsActivated
	{
		get
		{
			return this.activated;
		}
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000BC8FA File Offset: 0x000BAAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000BC902 File Offset: 0x000BAB02
	protected override void OnSpawn()
	{
		this.UpdateFlag();
		if (this.awaitingActivation && this.activateChore == null)
		{
			this.CreateChore();
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000BC920 File Offset: 0x000BAB20
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.activated = true;
		if (this.onActivate != null)
		{
			this.onActivate();
		}
		this.awaitingActivation = false;
		this.UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x001EB1F8 File Offset: 0x001E93F8
	private void UpdateFlag()
	{
		base.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated, null);
		base.Trigger(-1909216579, this.IsActivated);
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x001EB268 File Offset: 0x001E9468
	private void CreateChore()
	{
		if (this.activateChore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		this.activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.shouldShowSkillPerkStatusItem = true;
			this.requireMinionToWork = true;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000BC95B File Offset: 0x000BAB5B
	private void CancelChore()
	{
		if (this.activateChore == null)
		{
			return;
		}
		this.activateChore.Cancel("User cancelled");
		this.activateChore = null;
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06002C26 RID: 11302 RVA: 0x001EB2D8 File Offset: 0x001E94D8
	public string SidescreenButtonText
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06002C27 RID: 11303 RVA: 0x001EB338 File Offset: 0x001E9538
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000BC97D File Offset: 0x000BAB7D
	public bool SidescreenEnabled()
	{
		return !this.activated;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000BC988 File Offset: 0x000BAB88
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		this.textOverride = text;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000BC991 File Offset: 0x000BAB91
	public void OnSidescreenButtonPressed()
	{
		if (this.activateChore == null)
		{
			this.CreateChore();
		}
		else
		{
			this.CancelChore();
		}
		this.awaitingActivation = (this.activateChore != null);
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000BC97D File Offset: 0x000BAB7D
	public bool SidescreenButtonInteractable()
	{
		return !this.activated;
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001DA7 RID: 7591
	public bool Required = true;

	// Token: 0x04001DA8 RID: 7592
	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	// Token: 0x04001DA9 RID: 7593
	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	// Token: 0x04001DAA RID: 7594
	[Serialize]
	private bool activated;

	// Token: 0x04001DAB RID: 7595
	[Serialize]
	private bool awaitingActivation;

	// Token: 0x04001DAC RID: 7596
	private Guid statusItem;

	// Token: 0x04001DAD RID: 7597
	private Chore activateChore;

	// Token: 0x04001DAE RID: 7598
	public System.Action onActivate;

	// Token: 0x04001DAF RID: 7599
	[SerializeField]
	private ButtonMenuTextOverride textOverride;
}
