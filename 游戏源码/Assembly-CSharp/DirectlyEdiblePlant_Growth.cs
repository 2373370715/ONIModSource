using Klei.AI;
using TUNING;
using UnityEngine;

public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
	[MyCmpGet]
	private Growing growing;

	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		float num2 = 0f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(base.gameObject);
		if (amountInstance != null)
		{
			num2 = amountInstance.value / amountInstance.GetMax();
		}
		if (num2 < num)
		{
			return false;
		}
		return true;
	}

	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float num = 1f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(growing.gameObject);
		num = GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), GetComponent<KPrefabID>());
		float b = amountInstance.value * num;
		float num2 = Mathf.Min(desiredUnitsToConsume, b);
		growing.ConsumeGrowthUnits(num2, num);
		return num2;
	}

	public float PlantProductGrowthPerCycle()
	{
		Crop crop = GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
	{
		ModifierSet.TraitSet traits = Db.Get().traits;
		Tag prefabTag = prefab_id.PrefabTag;
		Trait trait = traits.Get(prefabTag.ToString() + "Original");
		if (trait != null)
		{
			AttributeModifier attributeModifier = trait.SelfModifiers.Find((AttributeModifier match) => match.AttributeId == "MaturityMax");
			if (attributeModifier != null)
			{
				return attributeModifier.Value / maturityMax;
			}
		}
		return 1f;
	}

	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}
}
