using STRINGS;

namespace Database;

public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
{
	private float amountToHarvest;

	public HarvestAmountFromSpacePOI(float amountToHarvest)
	{
		this.amountToHarvest = amountToHarvest;
	}

	public override string GetProgress(bool complete)
	{
		return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI, amountToHarvest);
	}

	public override bool Success()
	{
		return SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI > amountToHarvest;
	}
}
