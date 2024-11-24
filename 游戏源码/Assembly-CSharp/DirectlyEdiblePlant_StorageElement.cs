using STRINGS;
using UnityEngine;

public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
	public CellOffset[] edibleCellOffsets;

	public Tag tagToConsume = Tag.Invalid;

	public float rateProducedPerCycle;

	public float storageCapacity;

	[MyCmpReq]
	private Storage storage;

	[MyCmpGet]
	private KPrefabID prefabID;

	public float minimum_mass_percentageRequiredToEat = 0.25f;

	public float MassGeneratedPerCycle => rateProducedPerCycle * storageCapacity;

	protected override void OnPrefabInit()
	{
		storageCapacity = storage.capacityKg;
		base.OnPrefabInit();
	}

	public bool CanPlantBeEaten()
	{
		Tag tag = GetTagToConsume();
		return storage.GetMassAvailable(tag) / storage.capacityKg >= minimum_mass_percentageRequiredToEat;
	}

	public float ConsumePlant(float desiredUnitsToConsume)
	{
		if (storage.MassStored() <= 0f)
		{
			return 0f;
		}
		Tag tag = GetTagToConsume();
		float massAvailable = storage.GetMassAvailable(tag);
		float num = Mathf.Min(desiredUnitsToConsume, massAvailable);
		storage.ConsumeIgnoringDisease(tag, num);
		return num;
	}

	public float PlantProductGrowthPerCycle()
	{
		return MassGeneratedPerCycle;
	}

	private Tag GetTagToConsume()
	{
		if (!(tagToConsume != Tag.Invalid))
		{
			return storage.items[0].GetComponent<KPrefabID>().PrefabTag;
		}
		return tagToConsume;
	}

	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram), tagToConsume.ProperName());
	}

	public CellOffset[] GetAllowedOffsets()
	{
		return edibleCellOffsets;
	}
}
