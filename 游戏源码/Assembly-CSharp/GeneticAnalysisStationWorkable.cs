using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D97 RID: 3479
public class GeneticAnalysisStationWorkable : Workable
{
	// Token: 0x06004443 RID: 17475 RVA: 0x00247404 File Offset: 0x00245604
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_genetic_analysisstation_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06004444 RID: 17476 RVA: 0x000CC2BC File Offset: 0x000CA4BC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.storage.FindFirst(GameTags.UnidentifiedSeed));
	}

	// Token: 0x06004445 RID: 17477 RVA: 0x000CC2F0 File Offset: 0x000CA4F0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, false);
	}

	// Token: 0x06004446 RID: 17478 RVA: 0x000CC315 File Offset: 0x000CA515
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.IdentifyMutant();
	}

	// Token: 0x06004447 RID: 17479 RVA: 0x002474C4 File Offset: 0x002456C4
	public void IdentifyMutant()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.UnidentifiedSeed);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??"
		});
		if (gameObject != null)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			Pickupable pickupable;
			if (component.PrimaryElement.Units > 1f)
			{
				pickupable = component.Take(1f);
			}
			else
			{
				pickupable = this.storage.Drop(gameObject, true).GetComponent<Pickupable>();
			}
			pickupable.transform.SetPosition(base.transform.GetPosition() + this.finishedSeedDropOffset);
			MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
			component2.Analyze();
			SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(component2.SpeciesID);
		}
	}

	// Token: 0x04002ED6 RID: 11990
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04002ED7 RID: 11991
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002ED8 RID: 11992
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04002ED9 RID: 11993
	private Notification notification;

	// Token: 0x04002EDA RID: 11994
	public GeneticAnalysisStation.StatesInstance statesInstance;
}
