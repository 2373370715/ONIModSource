﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D4F RID: 3407
[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
	// Token: 0x060042BD RID: 17085 RVA: 0x002426E0 File Offset: 0x002408E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(15f);
		this.showProgressBar = true;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x0024278C File Offset: 0x0024098C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		EggIncubator component = base.GetComponent<EggIncubator>();
		if (component && component.Occupant)
		{
			IncubationMonitor.Instance smi = component.Occupant.GetSMI<IncubationMonitor.Instance>();
			if (smi != null)
			{
				smi.ApplySongBuff();
			}
		}
	}
}
