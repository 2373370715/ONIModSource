using System;

// Token: 0x020000D4 RID: 212
public static class BeeHiveTuning
{
	// Token: 0x0400021C RID: 540
	public static float ORE_DELIVERY_AMOUNT = 1f;

	// Token: 0x0400021D RID: 541
	public static float KG_ORE_EATEN_PER_CYCLE = BeeHiveTuning.ORE_DELIVERY_AMOUNT * 10f;

	// Token: 0x0400021E RID: 542
	public static float STANDARD_CALORIES_PER_CYCLE = 1500000f;

	// Token: 0x0400021F RID: 543
	public static float STANDARD_STARVE_CYCLES = 30f;

	// Token: 0x04000220 RID: 544
	public static float STANDARD_STOMACH_SIZE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE * BeeHiveTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000221 RID: 545
	public static float CALORIES_PER_KG_OF_ORE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / BeeHiveTuning.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000222 RID: 546
	public static float POOP_CONVERSTION_RATE = 0.9f;

	// Token: 0x04000223 RID: 547
	public static Tag CONSUMED_ORE = SimHashes.UraniumOre.CreateTag();

	// Token: 0x04000224 RID: 548
	public static Tag PRODUCED_ORE = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x04000225 RID: 549
	public static float HIVE_GROWTH_TIME = 2f;

	// Token: 0x04000226 RID: 550
	public static float WASTE_DROPPED_ON_DEATH = 5f;

	// Token: 0x04000227 RID: 551
	public static int GERMS_DROPPED_ON_DEATH = 10000;
}
