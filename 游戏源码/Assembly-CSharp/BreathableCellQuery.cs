using System;

// Token: 0x020007F5 RID: 2037
public class BreathableCellQuery : PathFinderQuery
{
	// Token: 0x06002470 RID: 9328 RVA: 0x000B7B1F File Offset: 0x000B5D1F
	public BreathableCellQuery Reset(Brain brain)
	{
		this.breather = brain.GetComponent<OxygenBreather>();
		return this;
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000B7B2E File Offset: 0x000B5D2E
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return this.breather.IsBreathableElementAtCell(cell, null);
	}

	// Token: 0x04001887 RID: 6279
	private OxygenBreather breather;
}
