using System;
using TUNING;
using UnityEngine;

public class MissionControlWorkable : Workable
{
	private Spacecraft targetSpacecraft;

	[MyCmpReq]
	private Operational operational;

	private Guid workStatusItem = Guid.Empty;

	public Spacecraft TargetSpacecraft
	{
		get
		{
			return targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = GetWorkTime();
			targetSpacecraft = value;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_mission_control_station_kanim") };
		SetWorkTime(90f);
		showProgressBar = true;
		lightEfficiencyBonus = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, TargetSpacecraft);
		operational.SetActive(value: true);
	}

	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	protected override void OnCompleteWork(Worker worker)
	{
		Debug.Assert(TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(workStatusItem);
		TargetSpacecraft = null;
		operational.SetActive(value: false);
	}
}
