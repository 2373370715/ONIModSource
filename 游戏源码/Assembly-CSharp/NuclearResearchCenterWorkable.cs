using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200166F RID: 5743
public class NuclearResearchCenterWorkable : Workable
{
	// Token: 0x0600769B RID: 30363 RVA: 0x00309C74 File Offset: 0x00307E74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		this.radiationStorage = base.GetComponent<HighEnergyParticleStorage>();
		this.nrc = base.GetComponent<NuclearResearchCenter>();
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x0600769C RID: 30364 RVA: 0x000D208E File Offset: 0x000D028E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x0600769D RID: 30365 RVA: 0x00309D00 File Offset: 0x00307F00
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = dt / this.nrc.timePerPoint;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.radiationStorage.ConsumeAndGet(num * this.nrc.materialPerPoint);
		this.pointsProduced += num;
		if (this.pointsProduced >= 1f)
		{
			int num2 = Mathf.FloorToInt(this.pointsProduced);
			this.pointsProduced -= (float)num2;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, Research.Instance.GetResearchType("nuclear").name, base.transform, 1.5f, false);
			Research.Instance.AddResearchPoints("nuclear", (float)num2);
		}
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		return this.radiationStorage.IsEmpty() || activeResearch == null || activeResearch.PercentageCompleteResearchType("nuclear") >= 1f;
	}

	// Token: 0x0600769E RID: 30366 RVA: 0x000EDF24 File Offset: 0x000EC124
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x000EDF4E File Offset: 0x000EC14E
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
	}

	// Token: 0x060076A0 RID: 30368 RVA: 0x000EDF57 File Offset: 0x000EC157
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x060076A1 RID: 30369 RVA: 0x00309DF8 File Offset: 0x00307FF8
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID["nuclear"];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue("nuclear", out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x060076A2 RID: 30370 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x040058C4 RID: 22724
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040058C5 RID: 22725
	[Serialize]
	private float pointsProduced;

	// Token: 0x040058C6 RID: 22726
	private NuclearResearchCenter nrc;

	// Token: 0x040058C7 RID: 22727
	private HighEnergyParticleStorage radiationStorage;
}
