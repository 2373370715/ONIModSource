using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001155 RID: 4437
[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x000DB13B File Offset: 0x000D933B
	public string cropId
	{
		get
		{
			return this.cropVal.cropId;
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x000DB148 File Offset: 0x000D9348
	// (set) Token: 0x06005AAA RID: 23210 RVA: 0x000DB150 File Offset: 0x000D9350
	public Storage PlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
		set
		{
			this.planterStorage = value;
		}
	}

	// Token: 0x06005AAB RID: 23211 RVA: 0x000DB159 File Offset: 0x000D9359
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Crops.Add(this);
		this.yield = this.GetAttributes().Add(Db.Get().PlantAttributes.YieldAmount);
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x000DB18C File Offset: 0x000D938C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
	}

	// Token: 0x06005AAD RID: 23213 RVA: 0x000DB1A5 File Offset: 0x000D93A5
	public void Configure(Crop.CropVal cropval)
	{
		this.cropVal = cropval;
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x000DB1AE File Offset: 0x000D93AE
	public bool CanGrow()
	{
		return this.cropVal.renewable;
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x002951C0 File Offset: 0x002933C0
	public void SpawnConfiguredFruit(object callbackParam)
	{
		if (this == null)
		{
			return;
		}
		Crop.CropVal cropVal = this.cropVal;
		if (!string.IsNullOrEmpty(cropVal.cropId))
		{
			this.SpawnSomeFruit(cropVal.cropId, this.yield.GetTotalValue());
			base.Trigger(-1072826864, this);
		}
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x00295214 File Offset: 0x00293414
	public void SpawnSomeFruit(Tag cropID, float amount)
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(cropID), base.transform.GetPosition() + new Vector3(0f, 0.75f, 0f), Grid.SceneLayer.Ore, null, 0);
		if (gameObject != null)
		{
			MutantPlant component = base.GetComponent<MutantPlant>();
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component != null && component.IsOriginal && component2 != null && base.GetComponent<SeedProducer>().RollForMutation())
			{
				component2.Mutate();
			}
			gameObject.SetActive(true);
			PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
			component3.Units = amount;
			component3.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			base.Trigger(35625290, gameObject);
			Edible component4 = gameObject.GetComponent<Edible>();
			if (component4)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component4.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", component4.GetProperName()), UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
				return;
			}
		}
		else
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"tried to spawn an invalid crop prefab:",
				cropID
			});
		}
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x000DB1BB File Offset: 0x000D93BB
	protected override void OnCleanUp()
	{
		Components.Crops.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnHarvest(object obj)
	{
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x000C9B47 File Offset: 0x000C7D47
	public List<Descriptor> RequirementDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x00295338 File Offset: 0x00293538
	public List<Descriptor> InformationDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Tag tag = new Tag(this.cropVal.cropId);
		GameObject prefab = Assets.GetPrefab(tag);
		Edible component = prefab.GetComponent<Edible>();
		Klei.AI.Attribute yieldAmount = Db.Get().PlantAttributes.YieldAmount;
		float preModifiedAttributeValue = go.GetComponent<Modifiers>().GetPreModifiedAttributeValue(yieldAmount);
		if (component != null)
		{
			DebugUtil.Assert(GameTags.DisplayAsCalories.Contains(tag), "Trying to display crop info for an edible fruit which isn't displayed as calories!", tag.ToString());
			float caloriesPerUnit = component.FoodInfo.CaloriesPerUnit;
			float calories = caloriesPerUnit * preModifiedAttributeValue;
			string text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
			Descriptor item = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, "", GameUtil.GetFormattedCalories(caloriesPerUnit, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else
		{
			string text;
			if (GameTags.DisplayAsUnits.Contains(tag))
			{
				text = GameUtil.GetFormattedUnits((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, false, "");
			}
			else
			{
				text = GameUtil.GetFormattedMass((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			}
			Descriptor item2 = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD_NONFOOD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD_NONFOOD, text), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x002954A8 File Offset: 0x002936A8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(go))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.InformationDescriptors(go))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04003FF9 RID: 16377
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003FFA RID: 16378
	public Crop.CropVal cropVal;

	// Token: 0x04003FFB RID: 16379
	private AttributeInstance yield;

	// Token: 0x04003FFC RID: 16380
	public string domesticatedDesc = "";

	// Token: 0x04003FFD RID: 16381
	private Storage planterStorage;

	// Token: 0x04003FFE RID: 16382
	private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>(delegate(Crop component, object data)
	{
		component.OnHarvest(data);
	});

	// Token: 0x02001156 RID: 4438
	[Serializable]
	public struct CropVal
	{
		// Token: 0x06005AB8 RID: 23224 RVA: 0x000DB1FD File Offset: 0x000D93FD
		public CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
		{
			this.cropId = crop_id;
			this.cropDuration = crop_duration;
			this.numProduced = num_produced;
			this.renewable = renewable;
		}

		// Token: 0x04003FFF RID: 16383
		public string cropId;

		// Token: 0x04004000 RID: 16384
		public float cropDuration;

		// Token: 0x04004001 RID: 16385
		public int numProduced;

		// Token: 0x04004002 RID: 16386
		public bool renewable;
	}
}
