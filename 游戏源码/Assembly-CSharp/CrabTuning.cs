using System.Collections.Generic;
using TUNING;

public static class CrabTuning
{
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.97f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.01f
		}
	};

	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WOOD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.02f
		}
	};

	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_FRESH = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.65f
		}
	};

	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	public static float STANDARD_STARVE_CYCLES = 10f;

	public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;

	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	public static float EGG_MASS = 2f;

	public static CellOffset[] DEFEND_OFFSETS = new CellOffset[5]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(-1, 1)
	};

	public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
	{
		if (IsValidTimeToDrop(smi) && IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry))
		{
			return smi.prefabID.HasTag(GameTags.Creatures.Happy);
		}
		return false;
	}

	public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
	{
		if (!smi.spawnedThisCycle)
		{
			if (!(smi.timeOfLastDrop <= 0f))
			{
				return GameClock.Instance.GetTime() - smi.timeOfLastDrop > 600f;
			}
			return true;
		}
		return false;
	}

	public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.transform.GetPosition());
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.Element[num].id == SimHashes.Ethanol)
		{
			return false;
		}
		return true;
	}
}
