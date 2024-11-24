using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FB9 RID: 8121
public class RequestCrewSideScreen : SideScreenContent
{
	// Token: 0x0600ABB0 RID: 43952 RVA: 0x0040B4EC File Offset: 0x004096EC
	protected override void OnSpawn()
	{
		this.changeCrewButton.onClick += this.OnChangeCrewButtonPressed;
		this.crewReleaseButton.onClick += this.CrewRelease;
		this.crewRequestButton.onClick += this.CrewRequest;
		this.toggleMap.Add(this.crewReleaseButton, PassengerRocketModule.RequestCrewState.Release);
		this.toggleMap.Add(this.crewRequestButton, PassengerRocketModule.RequestCrewState.Request);
		this.Refresh();
	}

	// Token: 0x0600ABB1 RID: 43953 RVA: 0x000CECD9 File Offset: 0x000CCED9
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x0600ABB2 RID: 43954 RVA: 0x0040B568 File Offset: 0x00409768
	public override bool IsValidForTarget(GameObject target)
	{
		PassengerRocketModule component = target.GetComponent<PassengerRocketModule>();
		RocketControlStation component2 = target.GetComponent<RocketControlStation>();
		if (component != null)
		{
			return component.GetMyWorld() != null;
		}
		if (component2 != null)
		{
			RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
			return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
		}
		return false;
	}

	// Token: 0x0600ABB3 RID: 43955 RVA: 0x0010F8A9 File Offset: 0x0010DAA9
	public override void SetTarget(GameObject target)
	{
		if (target.GetComponent<RocketControlStation>() != null)
		{
			this.rocketModule = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
		}
		else
		{
			this.rocketModule = target.GetComponent<PassengerRocketModule>();
		}
		this.Refresh();
	}

	// Token: 0x0600ABB4 RID: 43956 RVA: 0x0010F8E8 File Offset: 0x0010DAE8
	private void Refresh()
	{
		this.RefreshRequestButtons();
	}

	// Token: 0x0600ABB5 RID: 43957 RVA: 0x0010F8F0 File Offset: 0x0010DAF0
	private void CrewRelease()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
		this.RefreshRequestButtons();
	}

	// Token: 0x0600ABB6 RID: 43958 RVA: 0x0010F904 File Offset: 0x0010DB04
	private void CrewRequest()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
		this.RefreshRequestButtons();
	}

	// Token: 0x0600ABB7 RID: 43959 RVA: 0x0040B5CC File Offset: 0x004097CC
	private void RefreshRequestButtons()
	{
		foreach (KeyValuePair<KToggle, PassengerRocketModule.RequestCrewState> keyValuePair in this.toggleMap)
		{
			this.RefreshRequestButton(keyValuePair.Key);
		}
	}

	// Token: 0x0600ABB8 RID: 43960 RVA: 0x0040B628 File Offset: 0x00409828
	private void RefreshRequestButton(KToggle button)
	{
		ImageToggleState[] componentsInChildren;
		if (this.toggleMap[button] == this.rocketModule.PassengersRequested)
		{
			button.isOn = true;
			componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActive();
			}
			button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			return;
		}
		button.isOn = false;
		componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetInactive();
		}
		button.GetComponent<ImageToggleStateThrobber>().enabled = false;
	}

	// Token: 0x0600ABB9 RID: 43961 RVA: 0x0040B6B0 File Offset: 0x004098B0
	private void OnChangeCrewButtonPressed()
	{
		if (this.activeChangeCrewSideScreen == null)
		{
			this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
			this.activeChangeCrewSideScreen.SetTarget(this.rocketModule.gameObject);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.activeChangeCrewSideScreen = null;
	}

	// Token: 0x0600ABBA RID: 43962 RVA: 0x0010F918 File Offset: 0x0010DB18
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.activeChangeCrewSideScreen = null;
		}
	}

	// Token: 0x040086F3 RID: 34547
	private PassengerRocketModule rocketModule;

	// Token: 0x040086F4 RID: 34548
	public KToggle crewReleaseButton;

	// Token: 0x040086F5 RID: 34549
	public KToggle crewRequestButton;

	// Token: 0x040086F6 RID: 34550
	private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();

	// Token: 0x040086F7 RID: 34551
	public KButton changeCrewButton;

	// Token: 0x040086F8 RID: 34552
	public KScreen changeCrewSideScreenPrefab;

	// Token: 0x040086F9 RID: 34553
	private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;
}
