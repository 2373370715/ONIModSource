using System;
using System.Collections.Generic;

// Token: 0x02000CF5 RID: 3317
public interface IBridgedNetworkItem
{
	// Token: 0x060040A8 RID: 16552
	void AddNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x060040A9 RID: 16553
	bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x060040AA RID: 16554
	int GetNetworkCell();
}
