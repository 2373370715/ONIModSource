using System;
using TUNING;
using UnityEngine;

// Token: 0x02000DFB RID: 3579
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
	// Token: 0x06004663 RID: 18019 RVA: 0x000CD9C0 File Offset: 0x000CBBC0
	private IceCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x0024EB00 File Offset: 0x0024CD00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workerStatusItem = null;
	}

	// Token: 0x06004665 RID: 18021 RVA: 0x000CD9CF File Offset: 0x000CBBCF
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x000CDA0D File Offset: 0x000CBC0D
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x000CDA1C File Offset: 0x000CBC1C
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x000CDA1C File Offset: 0x000CBC1C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x040030AB RID: 12459
	[MyCmpGet]
	private Operational operational;
}
