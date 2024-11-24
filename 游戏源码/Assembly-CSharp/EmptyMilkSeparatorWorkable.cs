using System;
using TUNING;

public class EmptyMilkSeparatorWorkable : Workable
{
	public System.Action OnWork_PST_Begins;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		workLayer = Grid.SceneLayer.BuildingFront;
		workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_milk_separator_kanim") };
		attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		SetWorkTime(15f);
		synchronizeAnims = true;
	}

	public override void OnPendingCompleteWork(Worker worker)
	{
		OnWork_PST_Begins?.Invoke();
		base.OnPendingCompleteWork(worker);
	}
}
