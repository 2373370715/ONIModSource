using System;
using TUNING;
using UnityEngine;

// Token: 0x02000ED1 RID: 3793
public class MissionControlClusterWorkable : Workable
{
	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06004C71 RID: 19569 RVA: 0x000D17ED File Offset: 0x000CF9ED
	// (set) Token: 0x06004C72 RID: 19570 RVA: 0x000D17F5 File Offset: 0x000CF9F5
	public Clustercraft TargetClustercraft
	{
		get
		{
			return this.targetClustercraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetClustercraft = value;
		}
	}

	// Token: 0x06004C73 RID: 19571 RVA: 0x00262830 File Offset: 0x00260A30
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

	// Token: 0x06004C74 RID: 19572 RVA: 0x000D180A File Offset: 0x000CFA0A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlClusterWorkables.Add(this);
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x000D181D File Offset: 0x000CFA1D
	protected override void OnCleanUp()
	{
		Components.MissionControlClusterWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x000D1830 File Offset: 0x000CFA30
	public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
	{
		return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x002628F0 File Offset: 0x00260AF0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetClustercraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004C78 RID: 19576 RVA: 0x000D183F File Offset: 0x000CFA3F
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x000D1859 File Offset: 0x000CFA59
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetClustercraft == null || !MissionControlClusterWorkable.IsRocketInRange(base.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x000D1896 File Offset: 0x000CFA96
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetClustercraft != null);
		base.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x000D18C6 File Offset: 0x000CFAC6
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetClustercraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003516 RID: 13590
	private Clustercraft targetClustercraft;

	// Token: 0x04003517 RID: 13591
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003518 RID: 13592
	private Guid workStatusItem = Guid.Empty;
}
