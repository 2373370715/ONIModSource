using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200198A RID: 6538
public class ConditionRobotPilotReady : ProcessCondition
{
	// Token: 0x0600884C RID: 34892 RVA: 0x000F917C File Offset: 0x000F737C
	public ConditionRobotPilotReady(RoboPilotModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
		if (this.craftRegisterType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.craftInterface = module.GetComponent<RocketModuleCluster>().CraftInterface;
		}
	}

	// Token: 0x0600884D RID: 34893 RVA: 0x0035399C File Offset: 0x00351B9C
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				UnityEngine.Object component = this.craftInterface.GetComponent<Clustercraft>();
				ClusterTraveler component2 = this.craftInterface.GetComponent<ClusterTraveler>();
				if (component == null || component2 == null || component2.CurrentPath == null)
				{
					return ProcessCondition.Status.Failure;
				}
				int num = component2.RemainingTravelNodes();
				bool flag = this.module.HasResourcesToMove(num * 2);
				bool flag2 = this.module.HasResourcesToMove(num);
				if (flag)
				{
					result = ProcessCondition.Status.Ready;
				}
				else if (this.RocketHasDupeControlStation())
				{
					result = ProcessCondition.Status.Warning;
				}
				else if (flag2)
				{
					result = ProcessCondition.Status.Warning;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination == null)
			{
				result = ((this.module.GetDataBanksStored() >= 1f) ? ProcessCondition.Status.Warning : ProcessCondition.Status.Failure);
			}
			else if (this.module.HasResourcesToMove(spacecraftDestination.OneBasedDistance * 2))
			{
				result = ProcessCondition.Status.Ready;
			}
			else if (this.module.GetDataBanksStored() >= 1f)
			{
				result = ProcessCondition.Status.Warning;
			}
		}
		return result;
	}

	// Token: 0x0600884E RID: 34894 RVA: 0x00353AA4 File Offset: 0x00351CA4
	private bool RocketHasDupeControlStation()
	{
		if (this.craftInterface != null)
		{
			WorldContainer component = this.craftInterface.GetComponent<WorldContainer>();
			if (component != null && Components.RocketControlStations.GetWorldItems(component.id, false).Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600884F RID: 34895 RVA: 0x000F91B6 File Offset: 0x000F73B6
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.READY;
		}
		if (status != ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.FAILURE;
		}
		if (this.RocketHasDupeControlStation())
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING;
	}

	// Token: 0x06008850 RID: 34896 RVA: 0x00353AF0 File Offset: 0x00351CF0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
			if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
			{
				if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft && this.craftInterface.GetClusterDestinationSelector().IsAtDestination())
				{
					return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION;
				}
			}
			else
			{
				int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
				if (SpacecraftManager.instance.GetSpacecraftDestination(id) == null)
				{
					return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION;
				}
			}
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY;
		}
		if (status != ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE;
		}
		if (this.RocketHasDupeControlStation())
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING;
	}

	// Token: 0x06008851 RID: 34897 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400669E RID: 26270
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x0400669F RID: 26271
	private RoboPilotModule module;

	// Token: 0x040066A0 RID: 26272
	private CraftModuleInterface craftInterface;
}
