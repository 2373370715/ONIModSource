﻿using System;
using System.Collections.Generic;
using TUNING;

public static class DeerTuning
{
		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "WoodDeerEgg".ToTag(),
			weight = 1f
		}
	};

		public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

		public const float STANDARD_STARVE_CYCLES = 10f;

		public const float STANDARD_STOMACH_SIZE = 1000000f;

		public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

		public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

		public static float EGG_MASS = 2f;

		public static float DROP_ANTLER_DURATION = 1200f;
}
