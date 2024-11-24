using System;
using STRINGS;
using TUNING;

// Token: 0x02000346 RID: 838
public class FossilMine : ComplexFabricator
{
	// Token: 0x06000D87 RID: 3463 RVA: 0x00173D2C File Offset: 0x00171F2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fabricatorSM.idleAnimationName = "idle";
		this.fabricatorSM.idleQueue_StatusItem = Db.Get().BuildingStatusItems.FossilMineIdle;
		this.fabricatorSM.waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FossilMineEmpty;
		this.fabricatorSM.waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.FossilMinePendingWork;
		this.SideScreenSubtitleLabel = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_LIST_TITLE;
		this.SideScreenRecipeScreenTitle = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_RECIPE_SCREEN_TITLE;
		this.choreType = Db.Get().ChoreTypes.Art;
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00173DD4 File Offset: 0x00171FD4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossil_dig_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00173E9C File Offset: 0x0017209C
	public void SetActiveState(bool active)
	{
		if (active)
		{
			this.inStorage.showInUI = true;
			this.buildStorage.showInUI = true;
			this.outStorage.showInUI = true;
			this.fabricatorSM.Activate();
			if (this.workable is FossilMineWorkable)
			{
				(this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(true);
			}
			base.enabled = active;
			return;
		}
		base.OnDisable();
		this.fabricatorSM.Deactivate();
		this.inStorage.showInUI = false;
		this.buildStorage.showInUI = false;
		this.outStorage.showInUI = false;
		if (this.workable is FossilMineWorkable)
		{
			(this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(false);
		}
		base.enabled = false;
	}

	// Token: 0x040009C5 RID: 2501
	[MyCmpAdd]
	protected new FossilMineSM fabricatorSM;
}
