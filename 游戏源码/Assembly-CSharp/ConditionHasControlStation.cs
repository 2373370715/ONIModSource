using System;
using STRINGS;

// Token: 0x02001980 RID: 6528
public class ConditionHasControlStation : ProcessCondition
{
	// Token: 0x06008819 RID: 34841 RVA: 0x000F8F94 File Offset: 0x000F7194
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

	// Token: 0x0600881A RID: 34842 RVA: 0x000F8FA3 File Offset: 0x000F71A3
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id, false).Count <= 0)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x0600881B RID: 34843 RVA: 0x000F8FD0 File Offset: 0x000F71D0
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
	}

	// Token: 0x0600881C RID: 34844 RVA: 0x000F8FEB File Offset: 0x000F71EB
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
	}

	// Token: 0x0600881D RID: 34845 RVA: 0x000F9006 File Offset: 0x000F7206
	public override bool ShowInUI()
	{
		return this.EvaluateCondition() == ProcessCondition.Status.Failure;
	}

	// Token: 0x04006691 RID: 26257
	private RocketModuleCluster module;
}
