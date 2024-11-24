using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200109B RID: 4251
public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x0600573B RID: 22331 RVA: 0x00285348 File Offset: 0x00283548
	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		float num2 = 0f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(base.gameObject);
		if (amountInstance != null)
		{
			num2 = amountInstance.value / amountInstance.GetMax();
		}
		return num2 >= num;
	}

	// Token: 0x0600573C RID: 22332 RVA: 0x00285394 File Offset: 0x00283594
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.growing.gameObject);
		float growthUnitToMaturityRatio = this.GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), base.GetComponent<KPrefabID>());
		float b = amountInstance.value * growthUnitToMaturityRatio;
		float num = Mathf.Min(desiredUnitsToConsume, b);
		this.growing.ConsumeGrowthUnits(num, growthUnitToMaturityRatio);
		return num;
	}

	// Token: 0x0600573D RID: 22333 RVA: 0x002853FC File Offset: 0x002835FC
	public float PlantProductGrowthPerCycle()
	{
		Crop crop = base.GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x0600573E RID: 22334 RVA: 0x00285444 File Offset: 0x00283644
	private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
	{
		ResourceSet<Trait> traits = Db.Get().traits;
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

	// Token: 0x0600573F RID: 22335 RVA: 0x002854C0 File Offset: 0x002836C0
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x000AD332 File Offset: 0x000AB532
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x06005741 RID: 22337 RVA: 0x000A65EC File Offset: 0x000A47EC
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04003CEF RID: 15599
	[MyCmpGet]
	private Growing growing;
}
