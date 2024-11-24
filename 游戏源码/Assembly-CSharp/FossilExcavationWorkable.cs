using System;
using TUNING;

public abstract class FossilExcavationWorkable : Workable
{
	protected Guid waitingWorkStatusItemHandle;

	protected StatusItem waitingForExcavationWorkStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationOrdered;

	protected abstract bool IsMarkedForExcavation();

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		workingStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationInProgress;
		SetWorkerStatusItem(Db.Get().DuplicantStatusItems.FossilHunt_WorkerExcavating);
		requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_fossils_small_kanim") };
		attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		lightEfficiencyBonus = true;
		synchronizeAnims = false;
		shouldShowSkillPerkStatusItem = false;
	}

	protected override void UpdateStatusItem(object data = null)
	{
		base.UpdateStatusItem(data);
		KSelectable component = GetComponent<KSelectable>();
		if (waitingWorkStatusItemHandle != default(Guid))
		{
			component.RemoveStatusItem(waitingWorkStatusItemHandle);
		}
		if (base.worker == null && IsMarkedForExcavation())
		{
			waitingWorkStatusItemHandle = component.AddStatusItem(waitingForExcavationWorkStatusItem);
		}
	}
}
