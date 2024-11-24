using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020017B0 RID: 6064
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class ResearchCenter : Workable, IGameObjectEffectDescriptor, ISim200ms, IResearchCenter
{
	// Token: 0x06007CD9 RID: 31961 RVA: 0x0032305C File Offset: 0x0032125C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		ElementConverter elementConverter = this.elementConverter;
		elementConverter.onConvertMass = (Action<float>)Delegate.Combine(elementConverter.onConvertMass, new Action<float>(this.ConvertMassToResearchPoints));
	}

	// Token: 0x06007CDA RID: 31962 RVA: 0x003230F0 File Offset: 0x003212F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ResearchCenter>(-1914338957, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-125623018, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(187661686, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-1697596308, ResearchCenter.CheckHasMaterialDelegate);
		Components.ResearchCenters.Add(this);
		this.UpdateWorkingState(null);
	}

	// Token: 0x06007CDB RID: 31963 RVA: 0x0032315C File Offset: 0x0032135C
	private void ConvertMassToResearchPoints(float mass_consumed)
	{
		this.remainder_mass_points += mass_consumed / this.mass_per_point - (float)Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		int num = Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		num += Mathf.FloorToInt(this.remainder_mass_points);
		this.remainder_mass_points -= (float)Mathf.FloorToInt(this.remainder_mass_points);
		ResearchType researchType = Research.Instance.GetResearchType(this.research_point_type_id);
		if (num > 0)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, base.transform, 1.5f, false);
			for (int i = 0; i < num; i++)
			{
				Research.Instance.AddResearchPoints(this.research_point_type_id, 1f);
			}
		}
	}

	// Token: 0x06007CDC RID: 31964 RVA: 0x00323220 File Offset: 0x00321420
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsActive && this.operational.IsOperational && this.chore == null && this.HasMaterial())
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x06007CDD RID: 31965 RVA: 0x00323270 File Offset: 0x00321470
	protected virtual Chore CreateChore()
	{
		return new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
		{
			preemption_cb = new Func<Chore.Precondition.Context, bool>(ResearchCenter.CanPreemptCB)
		};
	}

	// Token: 0x06007CDE RID: 31966 RVA: 0x003232B8 File Offset: 0x003214B8
	private static bool CanPreemptCB(Chore.Precondition.Context context)
	{
		WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
		float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
		WorkerBase worker = context.consumerState.worker;
		return Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate() > num && context.chore.gameObject.GetComponent<ResearchCenter>().GetPercentComplete() < 1f;
	}

	// Token: 0x06007CDF RID: 31967 RVA: 0x00323338 File Offset: 0x00321538
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID[this.research_point_type_id];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x06007CE0 RID: 31968 RVA: 0x000F2348 File Offset: 0x000F0548
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06007CE1 RID: 31969 RVA: 0x003233AC File Offset: 0x003215AC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float efficiencyMultiplier = this.GetEfficiencyMultiplier(worker);
		float num = 2f + efficiencyMultiplier;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.elementConverter.SetWorkSpeedMultiplier(num);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06007CE2 RID: 31970 RVA: 0x000F237A File Offset: 0x000F057A
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06007CE3 RID: 31971 RVA: 0x003233F4 File Offset: 0x003215F4
	protected bool ResearchComponentCompleted()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			float num = 0f;
			float num2 = 0f;
			activeResearch.progressInventory.PointsByTypeID.TryGetValue(this.research_point_type_id, out num);
			activeResearch.tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2);
			if (num >= num2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007CE4 RID: 31972 RVA: 0x00309A24 File Offset: 0x00307C24
	protected bool IsAllResearchComplete()
	{
		using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06007CE5 RID: 31973 RVA: 0x00323454 File Offset: 0x00321654
	protected virtual void UpdateWorkingState(object data)
	{
		bool flag = false;
		bool flag2 = false;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			flag = true;
			if (activeResearch.tech.costsByResearchTypeID.ContainsKey(this.research_point_type_id) && Research.Instance.Get(activeResearch.tech).progressInventory.PointsByTypeID[this.research_point_type_id] < activeResearch.tech.costsByResearchTypeID[this.research_point_type_id])
			{
				flag2 = true;
			}
		}
		if (this.operational.GetFlag(EnergyConsumer.PoweredFlag) && !this.IsAllResearchComplete())
		{
			if (flag)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
				if (!flag2 && !this.ResearchComponentCompleted())
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, null);
				}
				else
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
				}
			}
			else
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, null);
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
			}
		}
		else
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
		}
		this.operational.SetFlag(ResearchCenter.ResearchSelectedFlag, flag && flag2);
		if ((!flag || !flag2) && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06007CE6 RID: 31974 RVA: 0x000F23B8 File Offset: 0x000F05B8
	private void ClearResearchScreen()
	{
		Game.Instance.Trigger(-1974454597, null);
	}

	// Token: 0x06007CE7 RID: 31975 RVA: 0x000F23CA File Offset: 0x000F05CA
	public string GetResearchType()
	{
		return this.research_point_type_id;
	}

	// Token: 0x06007CE8 RID: 31976 RVA: 0x000F23D2 File Offset: 0x000F05D2
	private void CheckHasMaterial(object o = null)
	{
		if (!this.HasMaterial() && this.chore != null)
		{
			this.chore.Cancel("No material remaining");
			this.chore = null;
		}
	}

	// Token: 0x06007CE9 RID: 31977 RVA: 0x000F23FB File Offset: 0x000F05FB
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06007CEA RID: 31978 RVA: 0x0032361C File Offset: 0x0032181C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateWorkingState));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.UpdateWorkingState));
		base.Unsubscribe(-1852328367, new Action<object>(this.UpdateWorkingState));
		Components.ResearchCenters.Remove(this);
		this.ClearResearchScreen();
	}

	// Token: 0x06007CEB RID: 31979 RVA: 0x00323690 File Offset: 0x00321890
	public string GetStatusString()
	{
		string text = RESEARCH.MESSAGING.NORESEARCHSELECTED;
		if (Research.Instance.GetActiveResearch() != null)
		{
			text = "<b>" + Research.Instance.GetActiveResearch().tech.Name + "</b>";
			int num = 0;
			foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0f)
				{
					num++;
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair2 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key] != 0f && keyValuePair2.Key == this.research_point_type_id)
				{
					text = text + "\n   - " + Research.Instance.researchTypes.GetResearchType(keyValuePair2.Key).name;
					text = string.Concat(new string[]
					{
						text,
						": ",
						keyValuePair2.Value.ToString(),
						"/",
						Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key].ToString()
					});
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair3 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair3.Key] != 0f && !(keyValuePair3.Key == this.research_point_type_id))
				{
					if (num > 1)
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEALSOREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
					else
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06007CEC RID: 31980 RVA: 0x00323970 File Offset: 0x00321B70
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false));
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x06007CED RID: 31981 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x04005E7F RID: 24191
	private Chore chore;

	// Token: 0x04005E80 RID: 24192
	[MyCmpAdd]
	protected Notifier notifier;

	// Token: 0x04005E81 RID: 24193
	[MyCmpAdd]
	protected Operational operational;

	// Token: 0x04005E82 RID: 24194
	[MyCmpAdd]
	protected Storage storage;

	// Token: 0x04005E83 RID: 24195
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04005E84 RID: 24196
	[SerializeField]
	public string research_point_type_id;

	// Token: 0x04005E85 RID: 24197
	[SerializeField]
	public Tag inputMaterial;

	// Token: 0x04005E86 RID: 24198
	[SerializeField]
	public float mass_per_point;

	// Token: 0x04005E87 RID: 24199
	[SerializeField]
	private float remainder_mass_points;

	// Token: 0x04005E88 RID: 24200
	public static readonly Operational.Flag ResearchSelectedFlag = new Operational.Flag("researchSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04005E89 RID: 24201
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> UpdateWorkingStateDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.UpdateWorkingState(data);
	});

	// Token: 0x04005E8A RID: 24202
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> CheckHasMaterialDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.CheckHasMaterial(data);
	});
}
