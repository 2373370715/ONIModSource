using System;

// Token: 0x02000CF4 RID: 3316
public interface ISecondaryInput
{
	// Token: 0x060040A6 RID: 16550
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x060040A7 RID: 16551
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
