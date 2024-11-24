using System;
using TUNING;
using UnityEngine;

// Token: 0x02000CF2 RID: 3314
[AddComponentMenu("KMonoBehaviour/Workable/CompostWorkable")]
public class CompostWorkable : Workable
{
	// Token: 0x060040A0 RID: 16544 RVA: 0x0023BE60 File Offset: 0x0023A060
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnStopWork(WorkerBase worker)
	{
	}
}
