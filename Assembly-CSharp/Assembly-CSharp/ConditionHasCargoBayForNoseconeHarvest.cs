using System;
using System.Collections.Generic;
using STRINGS;

public class ConditionHasCargoBayForNoseconeHarvest : ProcessCondition
{
	public ConditionHasCargoBayForNoseconeHarvest(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

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

	public override bool ShowInUI()
	{
		return this.HasHarvestNosecone();
	}

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

	private LaunchableRocketCluster launchable;
}
