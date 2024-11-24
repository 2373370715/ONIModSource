using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	public bool Required = true;

	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	[Serialize]
	private bool activated;

	[Serialize]
	private bool awaitingActivation;

	private Guid statusItem;

	private Chore activateChore;

	public System.Action onActivate;

	[SerializeField]
	private ButtonMenuTextOverride textOverride;

	public bool IsActivated => activated;

	public string SidescreenButtonText
	{
		get
		{
			if (activateChore != null)
			{
				return textOverride.IsValid ? textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return textOverride.IsValid ? textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	public string SidescreenButtonTooltip
	{
		get
		{
			if (activateChore != null)
			{
				return textOverride.IsValid ? textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return textOverride.IsValid ? textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		UpdateFlag();
		if (awaitingActivation && activateChore == null)
		{
			CreateChore();
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		activated = true;
		if (onActivate != null)
		{
			onActivate();
		}
		awaitingActivation = false;
		UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	private void UpdateFlag()
	{
		GetComponent<Operational>().SetFlag(Required ? activatedFlagRequirement : activatedFlagFunctional, activated);
		GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !activated);
		Trigger(-1909216579, IsActivated);
	}

	private void CreateChore()
	{
		if (activateChore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
			if (!string.IsNullOrEmpty(requiredSkillPerk))
			{
				shouldShowSkillPerkStatusItem = true;
				requireMinionToWork = true;
				UpdateStatusItem();
			}
		}
	}

	private void CancelChore()
	{
		if (activateChore != null)
		{
			activateChore.Cancel("User cancelled");
			activateChore = null;
		}
	}

	public int HorizontalGroupID()
	{
		return -1;
	}

	public bool SidescreenEnabled()
	{
		return !activated;
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		textOverride = text;
	}

	public void OnSidescreenButtonPressed()
	{
		if (activateChore == null)
		{
			CreateChore();
		}
		else
		{
			CancelChore();
		}
		awaitingActivation = activateChore != null;
	}

	public bool SidescreenButtonInteractable()
	{
		return !activated;
	}

	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}
}
