using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200109E RID: 4254
public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06005748 RID: 22344 RVA: 0x000D8F8F File Offset: 0x000D718F
	public float MassGeneratedPerCycle
	{
		get
		{
			return this.rateProducedPerCycle * this.storageCapacity;
		}
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x000D8F9E File Offset: 0x000D719E
	protected override void OnPrefabInit()
	{
		this.storageCapacity = this.storage.capacityKg;
		base.OnPrefabInit();
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x002854E4 File Offset: 0x002836E4
	public bool CanPlantBeEaten()
	{
		Tag tag = this.GetTagToConsume();
		return this.storage.GetMassAvailable(tag) / this.storage.capacityKg >= this.minimum_mass_percentageRequiredToEat;
	}

	// Token: 0x0600574B RID: 22347 RVA: 0x0028551C File Offset: 0x0028371C
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		if (this.storage.MassStored() <= 0f)
		{
			return 0f;
		}
		Tag tag = this.GetTagToConsume();
		float massAvailable = this.storage.GetMassAvailable(tag);
		float num = Mathf.Min(desiredUnitsToConsume, massAvailable);
		this.storage.ConsumeIgnoringDisease(tag, num);
		return num;
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x000D8FB7 File Offset: 0x000D71B7
	public float PlantProductGrowthPerCycle()
	{
		return this.MassGeneratedPerCycle;
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x000D8FBF File Offset: 0x000D71BF
	private Tag GetTagToConsume()
	{
		if (!(this.tagToConsume != Tag.Invalid))
		{
			return this.storage.items[0].GetComponent<KPrefabID>().PrefabTag;
		}
		return this.tagToConsume;
	}

	// Token: 0x0600574E RID: 22350 RVA: 0x000D8FF5 File Offset: 0x000D71F5
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), this.tagToConsume.ProperName());
	}

	// Token: 0x0600574F RID: 22351 RVA: 0x000D901F File Offset: 0x000D721F
	public CellOffset[] GetAllowedOffsets()
	{
		return this.edibleCellOffsets;
	}

	// Token: 0x06005750 RID: 22352 RVA: 0x000A6603 File Offset: 0x000A4803
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantStorage;
	}

	// Token: 0x04003CF3 RID: 15603
	public CellOffset[] edibleCellOffsets;

	// Token: 0x04003CF4 RID: 15604
	public Tag tagToConsume = Tag.Invalid;

	// Token: 0x04003CF5 RID: 15605
	public float rateProducedPerCycle;

	// Token: 0x04003CF6 RID: 15606
	public float storageCapacity;

	// Token: 0x04003CF7 RID: 15607
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003CF8 RID: 15608
	[MyCmpGet]
	private KPrefabID prefabID;

	// Token: 0x04003CF9 RID: 15609
	public float minimum_mass_percentageRequiredToEat = 0.25f;
}
