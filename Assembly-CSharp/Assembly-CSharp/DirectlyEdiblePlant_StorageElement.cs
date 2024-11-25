using STRINGS;
using UnityEngine;

public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions {
    public CellOffset[] edibleCellOffsets;
    public float        minimum_mass_percentageRequiredToEat = 0.25f;

    [MyCmpGet]
    private KPrefabID prefabID;

    public float rateProducedPerCycle;

    [MyCmpReq]
    private Storage storage;

    public float storageCapacity;
    public Tag   tagToConsume = Tag.Invalid;
    public float MassGeneratedPerCycle => rateProducedPerCycle * storageCapacity;

    public bool CanPlantBeEaten() {
        var tag = GetTagToConsume();
        return storage.GetMassAvailable(tag) / storage.capacityKg >= minimum_mass_percentageRequiredToEat;
    }

    public float ConsumePlant(float desiredUnitsToConsume) {
        if (storage.MassStored() <= 0f) return 0f;

        var tag           = GetTagToConsume();
        var massAvailable = storage.GetMassAvailable(tag);
        var num           = Mathf.Min(desiredUnitsToConsume, massAvailable);
        storage.ConsumeIgnoringDisease(tag, num);
        return num;
    }

    public float PlantProductGrowthPerCycle() { return MassGeneratedPerCycle; }

    public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond) {
        return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE,
                             GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond,
                                                       GameUtil.TimeSlice.PerCycle,
                                                       GameUtil.MetricMassFormat.Kilogram),
                             tagToConsume.ProperName());
    }

    public CellOffset[]       GetAllowedOffsets() { return edibleCellOffsets; }
    public Diet.Info.FoodType GetDietFoodType()   { return Diet.Info.FoodType.EatPlantStorage; }

    protected override void OnPrefabInit() {
        storageCapacity = storage.capacityKg;
        base.OnPrefabInit();
    }

    private Tag GetTagToConsume() {
        if (!(tagToConsume != Tag.Invalid)) return storage.items[0].GetComponent<KPrefabID>().PrefabTag;

        return tagToConsume;
    }
}