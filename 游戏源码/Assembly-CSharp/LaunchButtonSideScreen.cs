using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001F7D RID: 8061
public class LaunchButtonSideScreen : SideScreenContent
{
	// Token: 0x0600AA13 RID: 43539 RVA: 0x0010E818 File Offset: 0x0010CA18
	protected override void OnSpawn()
	{
		this.Refresh();
		this.launchButton.onClick += this.TriggerLaunch;
	}

	// Token: 0x0600AA14 RID: 43540 RVA: 0x0010E837 File Offset: 0x0010CA37
	public override int GetSideScreenSortOrder()
	{
		return -100;
	}

	// Token: 0x0600AA15 RID: 43541 RVA: 0x0010E83B File Offset: 0x0010CA3B
	public override bool IsValidForTarget(GameObject target)
	{
		return (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<LaunchPad>() && target.GetComponent<LaunchPad>().HasRocketWithCommandModule());
	}

	// Token: 0x0600AA16 RID: 43542 RVA: 0x00403E04 File Offset: 0x00402004
	public override void SetTarget(GameObject target)
	{
		bool flag = this.rocketModule == null || this.rocketModule.gameObject != target;
		this.selectedPad = null;
		this.rocketModule = target.GetComponent<RocketModuleCluster>();
		if (this.rocketModule == null)
		{
			this.selectedPad = target.GetComponent<LaunchPad>();
			if (this.selectedPad != null)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.selectedPad.LandedRocket.CraftInterface.ClusterModules)
				{
					if (@ref.Get().GetComponent<LaunchableRocketCluster>())
					{
						this.rocketModule = @ref.Get().GetComponent<RocketModuleCluster>();
						break;
					}
				}
			}
		}
		if (this.selectedPad == null)
		{
			CraftModuleInterface craftInterface = this.rocketModule.CraftInterface;
			this.selectedPad = craftInterface.CurrentPad;
		}
		if (flag)
		{
			this.acknowledgeWarnings = false;
		}
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
		this.Refresh();
	}

	// Token: 0x0600AA17 RID: 43543 RVA: 0x00403F48 File Offset: 0x00402148
	public override void ClearTarget()
	{
		if (this.rocketModule != null)
		{
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule = null;
		}
	}

	// Token: 0x0600AA18 RID: 43544 RVA: 0x00403FA0 File Offset: 0x004021A0
	private void TriggerLaunch()
	{
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		if (flag)
		{
			this.acknowledgeWarnings = true;
		}
		else if (flag2)
		{
			this.rocketModule.CraftInterface.CancelLaunch();
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.rocketModule.CraftInterface.TriggerLaunch(false);
		}
		this.Refresh();
	}

	// Token: 0x0600AA19 RID: 43545 RVA: 0x0010E874 File Offset: 0x0010CA74
	public void Update()
	{
		if (Time.unscaledTime > this.lastRefreshTime + 1f)
		{
			this.lastRefreshTime = Time.unscaledTime;
			this.Refresh();
		}
	}

	// Token: 0x0600AA1A RID: 43546 RVA: 0x00404018 File Offset: 0x00402218
	private void Refresh()
	{
		if (this.rocketModule == null || this.selectedPad == null)
		{
			return;
		}
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		bool flag3 = this.selectedPad.IsLogicInputConnected();
		bool flag4 = flag3 ? this.rocketModule.CraftInterface.CheckReadyForAutomatedLaunchCommand() : this.rocketModule.CraftInterface.CheckPreppedForLaunch();
		bool flag5 = this.rocketModule.CraftInterface.HasTag(GameTags.RocketNotOnGround);
		if (flag3)
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP;
		}
		else if (DebugHandler.InstantBuildMode || flag4)
		{
			this.launchButton.isInteractable = true;
			if (flag2)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON_TOOLTIP;
			}
			else if (flag)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON_TOOLTIP;
			}
			else
			{
				LocString loc_string = DebugHandler.InstantBuildMode ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_DEBUG : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
				this.launchButton.GetComponentInChildren<LocText>().text = loc_string;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_TOOLTIP;
			}
		}
		else
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_NOT_READY_TOOLTIP;
		}
		UnityEngine.Object interiorWorld = this.rocketModule.CraftInterface.GetInteriorWorld();
		RoboPilotModule robotPilotModule = this.rocketModule.CraftInterface.GetRobotPilotModule();
		PassengerRocketModule passengerModule = this.rocketModule.CraftInterface.GetPassengerModule();
		if (!(interiorWorld != null))
		{
			if (robotPilotModule != null)
			{
				if (!flag4)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
					return;
				}
				if (!flag2)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
					return;
				}
				if (!flag5)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
					return;
				}
			}
			else
			{
				this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			}
			return;
		}
		List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(this.rocketModule.CraftInterface.GetInteriorWorld().id, false);
		RocketControlStationLaunchWorkable rocketControlStationLaunchWorkable = null;
		if (worldItems != null && worldItems.Count > 0)
		{
			rocketControlStationLaunchWorkable = worldItems[0].GetComponent<RocketControlStationLaunchWorkable>();
		}
		if (passengerModule == null || rocketControlStationLaunchWorkable == null || robotPilotModule == null)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		bool flag6 = passengerModule.CheckPassengersBoarded(robotPilotModule == null);
		if (!flag6 && robotPilotModule != null)
		{
			flag6 |= !passengerModule.HasCrewAssigned();
		}
		bool flag7 = !passengerModule.CheckExtraPassengers();
		bool flag8 = robotPilotModule != null || rocketControlStationLaunchWorkable.worker != null;
		if (!flag4)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		if (!flag2)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
			return;
		}
		if (!flag6)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.LOADING_CREW;
			return;
		}
		if (!flag7)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.UNLOADING_PASSENGERS;
			return;
		}
		if (!flag8)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.WAITING_FOR_PILOT;
			return;
		}
		if (!flag5)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.TAKING_OFF;
	}

	// Token: 0x040085B9 RID: 34233
	public KButton launchButton;

	// Token: 0x040085BA RID: 34234
	public LocText statusText;

	// Token: 0x040085BB RID: 34235
	private RocketModuleCluster rocketModule;

	// Token: 0x040085BC RID: 34236
	private LaunchPad selectedPad;

	// Token: 0x040085BD RID: 34237
	private bool acknowledgeWarnings;

	// Token: 0x040085BE RID: 34238
	private float lastRefreshTime;

	// Token: 0x040085BF RID: 34239
	private const float UPDATE_FREQUENCY = 1f;

	// Token: 0x040085C0 RID: 34240
	private static readonly EventSystem.IntraObjectHandler<LaunchButtonSideScreen> RefreshDelegate = new EventSystem.IntraObjectHandler<LaunchButtonSideScreen>(delegate(LaunchButtonSideScreen cmp, object data)
	{
		cmp.Refresh();
	});
}
