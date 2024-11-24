using TUNING;
using UnityEngine;

public static class MooTuning
{
	public static readonly float STANDARD_LIFESPAN = 75f;

	public static readonly float STANDARD_CALORIES_PER_CYCLE = 200000f;

	public static readonly float STANDARD_STARVE_CYCLES = 6f;

	public static readonly float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;

	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	private static readonly float BECKONS_PER_LIFESPAN = 4f;

	private static readonly float BECKON_FUDGE_CYCLES = 11f;

	private static readonly float BECKON_CYCLES = Mathf.Floor((STANDARD_LIFESPAN - BECKON_FUDGE_CYCLES) / BECKONS_PER_LIFESPAN);

	public static readonly float WELLFED_EFFECT = 100f / (600f * BECKON_CYCLES);

	public static readonly float WELLFED_CALORIES_PER_CYCLE = STANDARD_CALORIES_PER_CYCLE * 0.9f;

	public static readonly float ELIGIBLE_MILKING_PERCENTAGE = 1f;

	public static readonly float MILK_PER_CYCLE = 50f;

	private static readonly float CYCLES_UNTIL_MILKING = 4f;

	public static readonly float MILK_CAPACITY = MILK_PER_CYCLE * CYCLES_UNTIL_MILKING;

	public static readonly float MILK_AMOUNT_AT_MILKING = MILK_PER_CYCLE * CYCLES_UNTIL_MILKING;

	public static readonly float MILK_PRODUCTION_PERCENTAGE_PER_SECOND = 100f / (600f * CYCLES_UNTIL_MILKING);
}
