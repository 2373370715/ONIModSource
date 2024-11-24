using System;

// Token: 0x02001290 RID: 4752
public interface IEnergyConsumer : ICircuitConnected
{
	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x0600619F RID: 24991
	float WattsUsed { get; }

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060061A0 RID: 24992
	float WattsNeededWhenActive { get; }

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060061A1 RID: 24993
	int PowerSortOrder { get; }

	// Token: 0x060061A2 RID: 24994
	void SetConnectionStatus(CircuitManager.ConnectionStatus status);

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060061A3 RID: 24995
	string Name { get; }

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x060061A4 RID: 24996
	bool IsConnected { get; }

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x060061A5 RID: 24997
	bool IsPowered { get; }
}
