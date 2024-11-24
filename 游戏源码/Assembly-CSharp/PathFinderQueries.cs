using System;

// Token: 0x020007EE RID: 2030
public static class PathFinderQueries
{
	// Token: 0x0600244D RID: 9293 RVA: 0x001C94D4 File Offset: 0x001C76D4
	public static void Reset()
	{
		PathFinderQueries.cellQuery = new CellQuery();
		PathFinderQueries.cellCostQuery = new CellCostQuery();
		PathFinderQueries.cellArrayQuery = new CellArrayQuery();
		PathFinderQueries.cellOffsetQuery = new CellOffsetQuery();
		PathFinderQueries.safeCellQuery = new SafeCellQuery();
		PathFinderQueries.idleCellQuery = new IdleCellQuery();
		PathFinderQueries.breathableCellQuery = new BreathableCellQuery();
		PathFinderQueries.drawNavGridQuery = new DrawNavGridQuery();
		PathFinderQueries.plantableCellQuery = new PlantableCellQuery();
		PathFinderQueries.mineableCellQuery = new MineableCellQuery();
		PathFinderQueries.staterpillarCellQuery = new StaterpillarCellQuery();
		PathFinderQueries.floorCellQuery = new FloorCellQuery();
		PathFinderQueries.buildingPlacementQuery = new BuildingPlacementQuery();
	}

	// Token: 0x0400184E RID: 6222
	public static CellQuery cellQuery = new CellQuery();

	// Token: 0x0400184F RID: 6223
	public static CellCostQuery cellCostQuery = new CellCostQuery();

	// Token: 0x04001850 RID: 6224
	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	// Token: 0x04001851 RID: 6225
	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	// Token: 0x04001852 RID: 6226
	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	// Token: 0x04001853 RID: 6227
	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	// Token: 0x04001854 RID: 6228
	public static BreathableCellQuery breathableCellQuery = new BreathableCellQuery();

	// Token: 0x04001855 RID: 6229
	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	// Token: 0x04001856 RID: 6230
	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	// Token: 0x04001857 RID: 6231
	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	// Token: 0x04001858 RID: 6232
	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	// Token: 0x04001859 RID: 6233
	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	// Token: 0x0400185A RID: 6234
	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
