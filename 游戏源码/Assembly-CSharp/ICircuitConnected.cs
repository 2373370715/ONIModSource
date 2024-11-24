using System;

// Token: 0x0200106F RID: 4207
public interface ICircuitConnected
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060055C9 RID: 21961
	bool IsVirtual { get; }

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x060055CA RID: 21962
	int PowerCell { get; }

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x060055CB RID: 21963
	object VirtualCircuitKey { get; }
}
