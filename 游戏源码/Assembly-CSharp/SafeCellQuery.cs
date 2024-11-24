using System;

// Token: 0x02000808 RID: 2056
public class SafeCellQuery : PathFinderQuery
{
	// Token: 0x060024BB RID: 9403 RVA: 0x000B7EE7 File Offset: 0x000B60E7
	public SafeCellQuery Reset(MinionBrain brain, bool avoid_light)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetCellFlags = (SafeCellQuery.SafeFlags)0;
		this.avoid_light = avoid_light;
		return this;
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x001CA5B8 File Offset: 0x001C87B8
	public static SafeCellQuery.SafeFlags GetFlags(int cell, MinionBrain brain, bool avoid_light = false)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		bool flag = brain.IsCellClear(cell);
		bool flag2 = !Grid.Element[cell].IsLiquid;
		bool flag3 = !Grid.Element[num].IsLiquid;
		bool flag4 = Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f;
		bool flag5 = Grid.Radiation[cell] < 250f;
		bool flag6 = brain.OxygenBreather == null || brain.OxygenBreather.IsBreathableElementAtCell(cell, Grid.DefaultOffset);
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
		bool flag8 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		bool flag9 = !avoid_light || SleepChore.IsDarkAtCell(cell);
		if (cell == Grid.PosToCell(brain))
		{
			flag6 = (brain.OxygenBreather == null || !brain.OxygenBreather.IsSuffocating);
		}
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		if (flag)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsClear;
		}
		if (flag4)
		{
			safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
		}
		if (flag5)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotRadiated;
		}
		if (flag6)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
		}
		if (flag7)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
		}
		if (flag8)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
		}
		if (flag2)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
		}
		if (flag3)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
		}
		if (flag9)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
		}
		return safeFlags;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x001CA788 File Offset: 0x001C8988
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light);
		bool flag = flags > this.targetCellFlags;
		bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
		if (flag || flag2)
		{
			this.targetCellFlags = flags;
			this.targetCost = cost;
			this.targetCell = cell;
		}
		return false;
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000B7F15 File Offset: 0x000B6115
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x040018D0 RID: 6352
	private MinionBrain brain;

	// Token: 0x040018D1 RID: 6353
	private int targetCell;

	// Token: 0x040018D2 RID: 6354
	private int targetCost;

	// Token: 0x040018D3 RID: 6355
	public SafeCellQuery.SafeFlags targetCellFlags;

	// Token: 0x040018D4 RID: 6356
	private bool avoid_light;

	// Token: 0x02000809 RID: 2057
	public enum SafeFlags
	{
		// Token: 0x040018D6 RID: 6358
		IsClear = 1,
		// Token: 0x040018D7 RID: 6359
		IsLightOk,
		// Token: 0x040018D8 RID: 6360
		IsNotLadder = 4,
		// Token: 0x040018D9 RID: 6361
		IsNotTube = 8,
		// Token: 0x040018DA RID: 6362
		CorrectTemperature = 16,
		// Token: 0x040018DB RID: 6363
		IsNotRadiated = 32,
		// Token: 0x040018DC RID: 6364
		IsBreathable = 64,
		// Token: 0x040018DD RID: 6365
		IsNotLiquidOnMyFace = 128,
		// Token: 0x040018DE RID: 6366
		IsNotLiquid = 256
	}
}
