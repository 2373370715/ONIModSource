using System;
using TUNING;

// Token: 0x02000EBA RID: 3770
public class EmptyMilkSeparatorWorkable : Workable
{
	// Token: 0x06004BF9 RID: 19449 RVA: 0x002601E8 File Offset: 0x0025E3E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workLayer = Grid.SceneLayer.BuildingFront;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_milk_separator_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(15f);
		this.synchronizeAnims = true;
	}

	// Token: 0x06004BFA RID: 19450 RVA: 0x000D126D File Offset: 0x000CF46D
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		System.Action onWork_PST_Begins = this.OnWork_PST_Begins;
		if (onWork_PST_Begins != null)
		{
			onWork_PST_Begins();
		}
		base.OnPendingCompleteWork(worker);
	}

	// Token: 0x040034A7 RID: 13479
	public System.Action OnWork_PST_Begins;
}
