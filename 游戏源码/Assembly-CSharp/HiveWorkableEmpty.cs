using System;
using TUNING;
using UnityEngine;

// Token: 0x02000DFA RID: 3578
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
	// Token: 0x0600465F RID: 18015 RVA: 0x0024EA58 File Offset: 0x0024CC58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x000CD961 File Offset: 0x000CBB61
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (!this.wasStung)
		{
			SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung = true;
		}
	}

	// Token: 0x040030A8 RID: 12456
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040030A9 RID: 12457
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x040030AA RID: 12458
	public bool wasStung;
}
