using STRINGS;

public class ConditionHasCargoBayForNoseconeHarvest : ProcessCondition
{
	private LaunchableRocketCluster launchable;

	public ConditionHasCargoBayForNoseconeHarvest(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	public override Status EvaluateCondition()
	{
		if (!HasHarvestNosecone())
		{
			return Status.Ready;
		}
		foreach (Ref<RocketModuleCluster> part in launchable.parts)
		{
			if ((bool)part.Get().GetComponent<CargoBayCluster>())
			{
				return Status.Ready;
			}
		}
		return Status.Warning;
	}

	public override string GetStatusMessage(Status status)
	{
		string result = "";
		switch (status)
		{
		case Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.READY;
			break;
		case Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.FAILURE;
			break;
		case Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.WARNING;
			break;
		}
		return result;
	}

	public override string GetStatusTooltip(Status status)
	{
		string result = "";
		switch (status)
		{
		case Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.READY;
			break;
		case Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.FAILURE;
			break;
		case Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.WARNING;
			break;
		}
		return result;
	}

	public override bool ShowInUI()
	{
		return HasHarvestNosecone();
	}

	private bool HasHarvestNosecone()
	{
		foreach (Ref<RocketModuleCluster> part in launchable.parts)
		{
			if (part.Get().HasTag("NoseconeHarvest"))
			{
				return true;
			}
		}
		return false;
	}
}
