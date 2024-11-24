using System;

// Token: 0x02001711 RID: 5905
public abstract class ProcessCondition
{
	// Token: 0x060079B2 RID: 31154
	public abstract ProcessCondition.Status EvaluateCondition();

	// Token: 0x060079B3 RID: 31155
	public abstract bool ShowInUI();

	// Token: 0x060079B4 RID: 31156
	public abstract string GetStatusMessage(ProcessCondition.Status status);

	// Token: 0x060079B5 RID: 31157 RVA: 0x000F00F7 File Offset: 0x000EE2F7
	public string GetStatusMessage()
	{
		return this.GetStatusMessage(this.EvaluateCondition());
	}

	// Token: 0x060079B6 RID: 31158
	public abstract string GetStatusTooltip(ProcessCondition.Status status);

	// Token: 0x060079B7 RID: 31159 RVA: 0x000F0105 File Offset: 0x000EE305
	public string GetStatusTooltip()
	{
		return this.GetStatusTooltip(this.EvaluateCondition());
	}

	// Token: 0x060079B8 RID: 31160 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		return null;
	}

	// Token: 0x060079B9 RID: 31161 RVA: 0x000F0113 File Offset: 0x000EE313
	public virtual ProcessCondition GetParentCondition()
	{
		return this.parentCondition;
	}

	// Token: 0x04005B2C RID: 23340
	protected ProcessCondition parentCondition;

	// Token: 0x02001712 RID: 5906
	public enum ProcessConditionType
	{
		// Token: 0x04005B2E RID: 23342
		RocketFlight,
		// Token: 0x04005B2F RID: 23343
		RocketPrep,
		// Token: 0x04005B30 RID: 23344
		RocketStorage,
		// Token: 0x04005B31 RID: 23345
		RocketBoard,
		// Token: 0x04005B32 RID: 23346
		All
	}

	// Token: 0x02001713 RID: 5907
	public enum Status
	{
		// Token: 0x04005B34 RID: 23348
		Failure,
		// Token: 0x04005B35 RID: 23349
		Warning,
		// Token: 0x04005B36 RID: 23350
		Ready
	}
}
