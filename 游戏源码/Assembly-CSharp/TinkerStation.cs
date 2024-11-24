using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020019EA RID: 6634
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms
{
	// Token: 0x1700090A RID: 2314
	// (set) Token: 0x06008A26 RID: 35366 RVA: 0x000BD94D File Offset: 0x000BBB4D
	public AttributeConverter AttributeConverter
	{
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (set) Token: 0x06008A27 RID: 35367 RVA: 0x000BD95E File Offset: 0x000BBB5E
	public float AttributeExperienceMultiplier
	{
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x1700090C RID: 2316
	// (set) Token: 0x06008A28 RID: 35368 RVA: 0x000BD967 File Offset: 0x000BBB67
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x1700090D RID: 2317
	// (set) Token: 0x06008A29 RID: 35369 RVA: 0x000BD970 File Offset: 0x000BBB70
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x06008A2A RID: 35370 RVA: 0x0035A3B4 File Offset: 0x003585B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		if (this.useFilteredStorage)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreType);
			this.filteredStorage = new FilteredStorage(this, null, null, false, byHash);
		}
		base.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06008A2B RID: 35371 RVA: 0x000FA6F8 File Offset: 0x000F88F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.useFilteredStorage && this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06008A2C RID: 35372 RVA: 0x000FA71B File Offset: 0x000F891B
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06008A2D RID: 35373 RVA: 0x0035A44C File Offset: 0x0035864C
	private bool CorrectRolePrecondition(MinionIdentity worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		return component != null && component.HasPerk(this.requiredSkillPerk);
	}

	// Token: 0x06008A2E RID: 35374 RVA: 0x0035A47C File Offset: 0x0035867C
	private void OnOperationalChanged(object data)
	{
		RoomTracker component = base.GetComponent<RoomTracker>();
		if (component != null && component.room != null)
		{
			component.room.RetriggerBuildings();
		}
	}

	// Token: 0x06008A2F RID: 35375 RVA: 0x000FA736 File Offset: 0x000F8936
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06008A30 RID: 35376 RVA: 0x000FA776 File Offset: 0x000F8976
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06008A31 RID: 35377 RVA: 0x0035A4AC File Offset: 0x003586AC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker);
		if (primaryElement != null)
		{
			SimHashes elementID = primaryElement.ElementID;
			this.storage.ConsumeIgnoringDisease(elementID.CreateTag(), this.massPerTinker);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), base.transform.GetPosition() + Vector3.up, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.SetElement(elementID, true);
			component.Temperature = this.outputTemperature;
			gameObject.SetActive(true);
		}
		this.chore = null;
	}

	// Token: 0x06008A32 RID: 35378 RVA: 0x000FA7B4 File Offset: 0x000F89B4
	public void Sim1000ms(float dt)
	{
		this.UpdateChore();
	}

	// Token: 0x06008A33 RID: 35379 RVA: 0x0035A550 File Offset: 0x00358750
	private void UpdateChore()
	{
		if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
				base.SetWorkTime(this.workTime);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Can't tinker");
			this.chore = null;
		}
	}

	// Token: 0x06008A34 RID: 35380 RVA: 0x000FA7BC File Offset: 0x000F89BC
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06008A35 RID: 35381 RVA: 0x0035A604 File Offset: 0x00358804
	private bool ToolsRequested()
	{
		return MaterialNeeds.GetAmount(this.outputPrefab, base.gameObject.GetMyWorldId(), false) > 0f && this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0f;
	}

	// Token: 0x06008A36 RID: 35382 RVA: 0x0035A654 File Offset: 0x00358854
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		string arg = this.inputMaterial.ProperName();
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
		List<Tinkerable> list = new List<Tinkerable>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
		{
			Tinkerable component = gameObject.GetComponent<Tinkerable>();
			if (component.tinkerMaterialTag == this.outputPrefab)
			{
				list.Add(component);
			}
		}
		if (list.Count > 0)
		{
			Effect effect = Db.Get().effects.Get(list[0].addedEffect);
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, effect.Name, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
			descriptors.Add(new Descriptor(this.EffectTitle, this.EffectTooltip, Descriptor.DescriptorType.Effect, false));
			foreach (Tinkerable cmp in list)
			{
				Descriptor item = new Descriptor(string.Format(this.EffectItemString, cmp.GetProperName()), string.Format(this.EffectItemTooltip, cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
				item.IncreaseIndent();
				descriptors.Add(item);
			}
		}
		return descriptors;
	}

	// Token: 0x06008A37 RID: 35383 RVA: 0x000FA7D0 File Offset: 0x000F89D0
	public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
	{
		TinkerStation result = go.AddOrGet<TinkerStation>();
		go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
		return result;
	}

	// Token: 0x040067FF RID: 26623
	public HashedString choreType;

	// Token: 0x04006800 RID: 26624
	public HashedString fetchChoreType;

	// Token: 0x04006801 RID: 26625
	private Chore chore;

	// Token: 0x04006802 RID: 26626
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04006803 RID: 26627
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04006804 RID: 26628
	public bool useFilteredStorage;

	// Token: 0x04006805 RID: 26629
	protected FilteredStorage filteredStorage;

	// Token: 0x04006806 RID: 26630
	public bool alwaysTinker;

	// Token: 0x04006807 RID: 26631
	public float massPerTinker;

	// Token: 0x04006808 RID: 26632
	public Tag inputMaterial;

	// Token: 0x04006809 RID: 26633
	public Tag outputPrefab;

	// Token: 0x0400680A RID: 26634
	public float outputTemperature;

	// Token: 0x0400680B RID: 26635
	public string EffectTitle = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;

	// Token: 0x0400680C RID: 26636
	public string EffectTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;

	// Token: 0x0400680D RID: 26637
	public string EffectItemString = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x0400680E RID: 26638
	public string EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x0400680F RID: 26639
	private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
