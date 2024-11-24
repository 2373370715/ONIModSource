using System;

// Token: 0x02000CF3 RID: 3315
public interface ISecondaryOutput
{
	// Token: 0x060040A4 RID: 16548
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x060040A5 RID: 16549
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
