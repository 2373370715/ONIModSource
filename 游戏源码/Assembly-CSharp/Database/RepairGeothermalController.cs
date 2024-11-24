using STRINGS;

namespace Database;

public class RepairGeothermalController : VictoryColonyAchievementRequirement
{
	public override string Description()
	{
		return GetProgress(Success());
	}

	public override string GetProgress(bool complete)
	{
		return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
	}

	public override string Name()
	{
		return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
	}

	public override bool Success()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}
}
