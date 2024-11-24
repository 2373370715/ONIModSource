using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x0200197F RID: 6527
public class ConditionHasCargoBayForNoseconeHarvest : ProcessCondition
{
	// Token: 0x06008813 RID: 34835 RVA: 0x000F8F7D File Offset: 0x000F717D
	public ConditionHasCargoBayForNoseconeHarvest(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06008814 RID: 34836 RVA: 0x00352D68 File Offset: 0x00350F68
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.HasHarvestNosecone())
		{
			return ProcessCondition.Status.Ready;
		}
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetComponent<CargoBayCluster>())
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Warning;
	}

	// Token: 0x06008815 RID: 34837 RVA: 0x00352DD4 File Offset: 0x00350FD4
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.READY;
			break;
		}
		return result;
	}

	// Token: 0x06008816 RID: 34838 RVA: 0x00352E24 File Offset: 0x00351024
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.READY;
			break;
		}
		return result;
	}

	// Token: 0x06008817 RID: 34839 RVA: 0x000F8F8C File Offset: 0x000F718C
	public override bool ShowInUI()
	{
		return this.HasHarvestNosecone();
	}

	// Token: 0x06008818 RID: 34840 RVA: 0x00352E74 File Offset: 0x00351074
	private bool HasHarvestNosecone()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag("NoseconeHarvest"))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04006690 RID: 26256
	private LaunchableRocketCluster launchable;
}
