using System;
using STRINGS;

// Token: 0x0200198C RID: 6540
public class ConditionSufficientFood : ProcessCondition
{
	// Token: 0x06008857 RID: 34903 RVA: 0x000F9229 File Offset: 0x000F7429
	public ConditionSufficientFood(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06008858 RID: 34904 RVA: 0x000F9238 File Offset: 0x000F7438
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008859 RID: 34905 RVA: 0x000F925B File Offset: 0x000F745B
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.NAME;
		}
		return UI.STARMAP.NOFOOD.NAME;
	}

	// Token: 0x0600885A RID: 34906 RVA: 0x000F9276 File Offset: 0x000F7476
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.TOOLTIP;
		}
		return UI.STARMAP.NOFOOD.TOOLTIP;
	}

	// Token: 0x0600885B RID: 34907 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x040066A2 RID: 26274
	private CommandModule module;
}
