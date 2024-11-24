using System;
using STRINGS;

// Token: 0x02001988 RID: 6536
public class ConditionPilotOnBoard : ProcessCondition
{
	// Token: 0x06008842 RID: 34882 RVA: 0x000F9126 File Offset: 0x000F7326
	public ConditionPilotOnBoard(PassengerRocketModule module)
	{
		this.module = module;
		this.rocketModule = module.GetComponent<RocketModuleCluster>();
	}

	// Token: 0x06008843 RID: 34883 RVA: 0x000F9141 File Offset: 0x000F7341
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.CheckPilotBoarded())
		{
			return ProcessCondition.Status.Ready;
		}
		if (this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06008844 RID: 34884 RVA: 0x0035379C File Offset: 0x0035199C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
	}

	// Token: 0x06008845 RID: 34885 RVA: 0x003537EC File Offset: 0x003519EC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
	}

	// Token: 0x06008846 RID: 34886 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400669B RID: 26267
	private PassengerRocketModule module;

	// Token: 0x0400669C RID: 26268
	private RocketModuleCluster rocketModule;
}
