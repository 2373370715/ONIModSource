using STRINGS;
using TUNING;

public class FossilMine : ComplexFabricator
{
	[MyCmpAdd]
	protected new FossilMineSM fabricatorSM;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		fabricatorSM.idleAnimationName = "idle";
		fabricatorSM.idleQueue_StatusItem = Db.Get().BuildingStatusItems.FossilMineIdle;
		fabricatorSM.waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FossilMineEmpty;
		fabricatorSM.waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.FossilMinePendingWork;
		SideScreenSubtitleLabel = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_LIST_TITLE;
		SideScreenRecipeScreenTitle = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_RECIPE_SCREEN_TITLE;
		choreType = Db.Get().ChoreTypes.Art;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		workable.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		workable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_fossil_dig_kanim") };
		workable.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
	}

	public void SetActiveState(bool active)
	{
		if (active)
		{
			inStorage.showInUI = true;
			buildStorage.showInUI = true;
			outStorage.showInUI = true;
			fabricatorSM.Activate();
			if (workable is FossilMineWorkable)
			{
				(workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(shouldItBeShown: true);
			}
			base.enabled = active;
			return;
		}
		base.OnDisable();
		fabricatorSM.Deactivate();
		inStorage.showInUI = false;
		buildStorage.showInUI = false;
		outStorage.showInUI = false;
		if (workable is FossilMineWorkable)
		{
			(workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(shouldItBeShown: false);
		}
		base.enabled = false;
	}
}
