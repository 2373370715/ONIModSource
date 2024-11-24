using System;

// Token: 0x0200118F RID: 4495
public interface IManageGrowingStates
{
	// Token: 0x06005BAC RID: 23468
	float TimeUntilNextHarvest();

	// Token: 0x06005BAD RID: 23469
	float PercentGrown();

	// Token: 0x06005BAE RID: 23470
	Crop GetGropComponent();

	// Token: 0x06005BAF RID: 23471
	void OverrideMaturityLevel(float percentage);

	// Token: 0x06005BB0 RID: 23472
	float DomesticGrowthTime();

	// Token: 0x06005BB1 RID: 23473
	float WildGrowthTime();
}
