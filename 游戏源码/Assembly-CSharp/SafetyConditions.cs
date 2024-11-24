using System;
using System.Collections.Generic;

// Token: 0x02000801 RID: 2049
public class SafetyConditions
{
	// Token: 0x0600249C RID: 9372 RVA: 0x001CA0C8 File Offset: 0x001C82C8
	public SafetyConditions()
	{
		int num = 1;
		this.IsNearby = new SafetyChecker.Condition("IsNearby", num *= 2, (int cell, int cost, SafetyChecker.Context context) => cost > 5);
		this.IsNotLedge = new SafetyChecker.Condition("IsNotLedge", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int i = Grid.CellBelow(Grid.CellLeft(cell));
			if (Grid.Solid[i])
			{
				return false;
			}
			int i2 = Grid.CellBelow(Grid.CellRight(cell));
			return Grid.Solid[i2];
		});
		this.IsNotLiquid = new SafetyChecker.Condition("IsNotLiquid", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !Grid.Element[cell].IsLiquid);
		this.IsNotLadder = new SafetyChecker.Condition("IsNotLadder", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole));
		this.IsNotDoor = new SafetyChecker.Condition("IsNotDoor", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return !Grid.HasDoor[cell] && Grid.IsValidCell(num2) && !Grid.HasDoor[num2];
		});
		this.IsCorrectTemperature = new SafetyChecker.Condition("IsCorrectTemperature", num *= 2, (int cell, int cost, SafetyChecker.Context context) => Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		this.IsWarming = new SafetyChecker.Condition("IsWarming", num *= 2, (int cell, int cost, SafetyChecker.Context context) => WarmthProvider.IsWarmCell(cell));
		this.IsCooling = new SafetyChecker.Condition("IsCooling", num *= 2, (int cell, int cost, SafetyChecker.Context context) => false);
		this.HasSomeOxygen = new SafetyChecker.Condition("HasSomeOxygen", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || context.oxygenBreather.IsBreathableElementAtCell(cell, null));
		this.IsClear = new SafetyChecker.Condition("IsClear", num * 2, (int cell, int cost, SafetyChecker.Context context) => context.minionBrain.IsCellClear(cell));
		this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsWarming
		}.ToArray());
		this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsCooling
		}.ToArray());
		List<SafetyChecker.Condition> list = new List<SafetyChecker.Condition>();
		list.Add(this.HasSomeOxygen);
		list.Add(this.IsNotDoor);
		this.RecoverBreathChecker = new SafetyChecker(list.ToArray());
		List<SafetyChecker.Condition> list2 = new List<SafetyChecker.Condition>(list);
		list2.Add(this.IsNotLiquid);
		list2.Add(this.IsCorrectTemperature);
		this.SafeCellChecker = new SafetyChecker(list2.ToArray());
		this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>(list2)
		{
			this.IsClear,
			this.IsNotLadder
		}.ToArray());
		this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotLiquid,
			this.IsNotLedge,
			this.IsNearby
		}.ToArray());
	}

	// Token: 0x040018A3 RID: 6307
	public SafetyChecker.Condition IsNotLiquid;

	// Token: 0x040018A4 RID: 6308
	public SafetyChecker.Condition IsNotLadder;

	// Token: 0x040018A5 RID: 6309
	public SafetyChecker.Condition IsCorrectTemperature;

	// Token: 0x040018A6 RID: 6310
	public SafetyChecker.Condition IsWarming;

	// Token: 0x040018A7 RID: 6311
	public SafetyChecker.Condition IsCooling;

	// Token: 0x040018A8 RID: 6312
	public SafetyChecker.Condition HasSomeOxygen;

	// Token: 0x040018A9 RID: 6313
	public SafetyChecker.Condition IsClear;

	// Token: 0x040018AA RID: 6314
	public SafetyChecker.Condition IsNotFoundation;

	// Token: 0x040018AB RID: 6315
	public SafetyChecker.Condition IsNotDoor;

	// Token: 0x040018AC RID: 6316
	public SafetyChecker.Condition IsNotLedge;

	// Token: 0x040018AD RID: 6317
	public SafetyChecker.Condition IsNearby;

	// Token: 0x040018AE RID: 6318
	public SafetyChecker WarmUpChecker;

	// Token: 0x040018AF RID: 6319
	public SafetyChecker CoolDownChecker;

	// Token: 0x040018B0 RID: 6320
	public SafetyChecker RecoverBreathChecker;

	// Token: 0x040018B1 RID: 6321
	public SafetyChecker VomitCellChecker;

	// Token: 0x040018B2 RID: 6322
	public SafetyChecker SafeCellChecker;

	// Token: 0x040018B3 RID: 6323
	public SafetyChecker IdleCellChecker;
}
