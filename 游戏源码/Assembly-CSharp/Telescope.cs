using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms, BuildingStatusItems.ISkyVisInfo
{
	private OxygenBreather.IGasProvider workerGasProvider;

	private Operational operational;

	private float percentClear;

	private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);

	private Storage storage;

	public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition
	{
		id = "ContainsOxygen",
		sortOrder = 1,
		description = DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen) != null;
		}
	};

	private Chore chore;

	private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);

	float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
	{
		return percentClear;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SpacecraftManager.instance.Subscribe(532901469, UpdateWorkingState);
		Components.Telescopes.Add(this);
		OnWorkableEventCB = (Action<Workable, WorkableEvent>)Delegate.Combine(OnWorkableEventCB, new Action<Workable, WorkableEvent>(OnWorkableEvent));
		operational = GetComponent<Operational>();
		storage = GetComponent<Storage>();
		UpdateWorkingState(null);
	}

	protected override void OnCleanUp()
	{
		Components.Telescopes.Remove(this);
		SpacecraftManager.instance.Unsubscribe(532901469, UpdateWorkingState);
		base.OnCleanUp();
	}

	public void Sim200ms(float dt)
	{
		GetComponent<Building>().GetExtents();
		(bool isAnyVisible, float percentVisible01) visibilityOf = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(base.gameObject);
		bool item = visibilityOf.isAnyVisible;
		float num = (percentClear = visibilityOf.percentVisible01);
		KSelectable component = GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, this);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && num < 1f, this);
		Operational component2 = GetComponent<Operational>();
		component2.SetFlag(visibleSkyFlag, item);
		if (!component2.IsActive && component2.IsOperational && chore == null)
		{
			chore = CreateChore();
			SetWorkTime(float.PositiveInfinity);
		}
	}

	private void OnWorkableEvent(Workable workable, WorkableEvent ev)
	{
		Worker worker = base.worker;
		if (worker == null)
		{
			return;
		}
		OxygenBreather component = worker.GetComponent<OxygenBreather>();
		KPrefabID component2 = worker.GetComponent<KPrefabID>();
		KSelectable component3 = GetComponent<KSelectable>();
		switch (ev)
		{
		case WorkableEvent.WorkStarted:
			ShowProgressBar(show: true);
			progressBar.SetUpdateFunc(() => SpacecraftManager.instance.HasAnalysisTarget() ? (SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE) : 0f);
			workerGasProvider = component.GetGasProvider();
			component.SetGasProvider(this);
			component.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
			component2.AddTag(GameTags.Shaded);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			break;
		case WorkableEvent.WorkStopped:
			component.SetGasProvider(workerGasProvider);
			component.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
			ShowProgressBar(show: false);
			component2.RemoveTag(GameTags.Shaded);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			break;
		}
	}

	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(percentClear);
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (SpacecraftManager.instance.HasAnalysisTarget())
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			SpaceDestination destination = SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID);
			float num = 1f / (float)destination.OneBasedDistance;
			float num2 = ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED;
			float dEFAULT_CYCLES_PER_DISCOVERY = ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY;
			float num3 = num2 / dEFAULT_CYCLES_PER_DISCOVERY / 600f;
			float points = dt * num * num3;
			SpacecraftManager.instance.EarnDestinationAnalysisPoints(starmapAnalysisDestinationID, points);
		}
		return base.OnWorkTick(worker, dt);
	}

	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
		descriptors.Add(item);
		return descriptors;
	}

	protected Chore CreateChore()
	{
		WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, this);
		workChore.AddPrecondition(ContainsOxygen);
		return workChore;
	}

	protected void UpdateWorkingState(object data)
	{
		bool flag = false;
		if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
		{
			flag = true;
		}
		GetComponent<KSelectable>().ToggleStatusItem(on: !flag && !SpacecraftManager.instance.AreAllDestinationsAnalyzed(), status_item: Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected);
		operational.SetFlag(Telescope.flag, flag);
		if (!flag && (bool)base.worker)
		{
			StopWork(base.worker, aborted: true);
		}
	}

	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	public bool ShouldEmitCO2()
	{
		return false;
	}

	public bool ShouldStoreCO2()
	{
		return false;
	}

	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (storage.items.Count <= 0)
		{
			return false;
		}
		GameObject gameObject = storage.items[0];
		if (gameObject == null)
		{
			return false;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		bool result = component.Mass >= amount;
		component.Mass = Mathf.Max(0f, component.Mass - amount);
		return result;
	}

	public bool IsLowOxygen()
	{
		if (storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = storage.FindFirstWithMass(GameTags.Breathable);
		if (!(primaryElement == null))
		{
			return primaryElement.Mass == 0f;
		}
		return true;
	}
}
