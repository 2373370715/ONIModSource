using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x0200197D RID: 6525
public class ConditionHasAstronaut : ProcessCondition
{
	// Token: 0x06008809 RID: 34825 RVA: 0x000F8EDC File Offset: 0x000F70DC
	public ConditionHasAstronaut(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x0600880A RID: 34826 RVA: 0x00352CB8 File Offset: 0x00350EB8
	public override ProcessCondition.Status EvaluateCondition()
	{
		List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null)
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x0600880B RID: 34827 RVA: 0x000F8EEB File Offset: 0x000F70EB
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x0600880C RID: 34828 RVA: 0x000F8F06 File Offset: 0x000F7106
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x0600880D RID: 34829 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400668E RID: 26254
	private CommandModule module;
}
