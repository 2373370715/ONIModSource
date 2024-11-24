using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02001004 RID: 4100
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
	// Token: 0x06005391 RID: 21393 RVA: 0x00278310 File Offset: 0x00276510
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = ToiletWorkableClean.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
	}

	// Token: 0x06005392 RID: 21394 RVA: 0x000D66FE File Offset: 0x000D48FE
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003A64 RID: 14948
	[Serialize]
	public int timesCleaned;

	// Token: 0x04003A65 RID: 14949
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"unclog_pre",
		"unclog_loop"
	};

	// Token: 0x04003A66 RID: 14950
	private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");
}
