using System;
using TUNING;
using UnityEngine;

// Token: 0x020000EF RID: 239
public static class MooTuning
{
	// Token: 0x0400028F RID: 655
	public static readonly float STANDARD_LIFESPAN = 75f;

	// Token: 0x04000290 RID: 656
	public static readonly float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x04000291 RID: 657
	public static readonly float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x04000292 RID: 658
	public static readonly float STANDARD_STOMACH_SIZE = MooTuning.STANDARD_CALORIES_PER_CYCLE * MooTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000293 RID: 659
	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x04000294 RID: 660
	private static readonly float BECKONS_PER_LIFESPAN = 4f;

	// Token: 0x04000295 RID: 661
	private static readonly float BECKON_FUDGE_CYCLES = 11f;

	// Token: 0x04000296 RID: 662
	private static readonly float BECKON_CYCLES = Mathf.Floor((MooTuning.STANDARD_LIFESPAN - MooTuning.BECKON_FUDGE_CYCLES) / MooTuning.BECKONS_PER_LIFESPAN);

	// Token: 0x04000297 RID: 663
	public static readonly float WELLFED_EFFECT = 100f / (600f * MooTuning.BECKON_CYCLES);

	// Token: 0x04000298 RID: 664
	public static readonly float WELLFED_CALORIES_PER_CYCLE = MooTuning.STANDARD_CALORIES_PER_CYCLE * 0.9f;

	// Token: 0x04000299 RID: 665
	public static readonly float ELIGIBLE_MILKING_PERCENTAGE = 1f;

	// Token: 0x0400029A RID: 666
	public static readonly float MILK_PER_CYCLE = 50f;

	// Token: 0x0400029B RID: 667
	private static readonly float CYCLES_UNTIL_MILKING = 4f;

	// Token: 0x0400029C RID: 668
	public static readonly float MILK_CAPACITY = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x0400029D RID: 669
	public static readonly float MILK_AMOUNT_AT_MILKING = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x0400029E RID: 670
	public static readonly float MILK_PRODUCTION_PERCENTAGE_PER_SECOND = 100f / (600f * MooTuning.CYCLES_UNTIL_MILKING);
}
