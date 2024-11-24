using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000DB RID: 219
public static class CrabTuning
{
	// Token: 0x06000391 RID: 913 RVA: 0x000A71DE File Offset: 0x000A53DE
	public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
	{
		return CrabTuning.IsValidTimeToDrop(smi) && CrabTuning.IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry) && smi.prefabID.HasTag(GameTags.Creatures.Happy);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x000A7214 File Offset: 0x000A5414
	public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
	{
		return !smi.spawnedThisCycle && (smi.timeOfLastDrop <= 0f || GameClock.Instance.GetTime() - smi.timeOfLastDrop > 600f);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0014F6E8 File Offset: 0x0014D8E8
	public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.transform.GetPosition());
		return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Ethanol;
	}

	// Token: 0x0400023F RID: 575
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

	// Token: 0x04000240 RID: 576
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

	// Token: 0x04000241 RID: 577
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

	// Token: 0x04000242 RID: 578
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000243 RID: 579
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000244 RID: 580
	public static float STANDARD_STOMACH_SIZE = CrabTuning.STANDARD_CALORIES_PER_CYCLE * CrabTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000245 RID: 581
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000246 RID: 582
	public static float EGG_MASS = 2f;

	// Token: 0x04000247 RID: 583
	public static CellOffset[] DEFEND_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(-1, 1)
	};
}
