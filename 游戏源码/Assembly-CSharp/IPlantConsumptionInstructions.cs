using System;

// Token: 0x020009DC RID: 2524
public interface IPlantConsumptionInstructions
{
	// Token: 0x06002E5E RID: 11870
	CellOffset[] GetAllowedOffsets();

	// Token: 0x06002E5F RID: 11871
	float ConsumePlant(float desiredUnitsToConsume);

	// Token: 0x06002E60 RID: 11872
	float PlantProductGrowthPerCycle();

	// Token: 0x06002E61 RID: 11873
	bool CanPlantBeEaten();

	// Token: 0x06002E62 RID: 11874
	string GetFormattedConsumptionPerCycle(float consumer_caloriesLossPerCaloriesPerKG);

	// Token: 0x06002E63 RID: 11875
	Diet.Info.FoodType GetDietFoodType();
}
