using System;
using TUNING;
using UnityEngine;

// Token: 0x02000ED2 RID: 3794
public class MissionControlWorkable : Workable
{
	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06004C7D RID: 19581 RVA: 0x000D190E File Offset: 0x000CFB0E
	// (set) Token: 0x06004C7E RID: 19582 RVA: 0x000D1916 File Offset: 0x000CFB16
	public Spacecraft TargetSpacecraft
	{
		get
		{
			return this.targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetSpacecraft = value;
		}
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x00262830 File Offset: 0x00260A30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_mission_control_station_kanim")
		};
		base.SetWorkTime(90f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x000D192B File Offset: 0x000CFB2B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	// Token: 0x06004C81 RID: 19585 RVA: 0x000D193E File Offset: 0x000CFB3E
	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x0026293C File Offset: 0x00260B3C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetSpacecraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x000D183F File Offset: 0x000CFA3F
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x000D1951 File Offset: 0x000CFB51
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x000D196B File Offset: 0x000CFB6B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(this.TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x000D1998 File Offset: 0x000CFB98
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetSpacecraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003519 RID: 13593
	private Spacecraft targetSpacecraft;

	// Token: 0x0400351A RID: 13594
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400351B RID: 13595
	private Guid workStatusItem = Guid.Empty;
}
