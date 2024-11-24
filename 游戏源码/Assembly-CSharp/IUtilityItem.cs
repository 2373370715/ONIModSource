using System;

// Token: 0x02001A0E RID: 6670
public interface IUtilityItem
{
	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06008AF0 RID: 35568
	// (set) Token: 0x06008AF1 RID: 35569
	UtilityConnections Connections { get; set; }

	// Token: 0x06008AF2 RID: 35570
	void UpdateConnections(UtilityConnections Connections);

	// Token: 0x06008AF3 RID: 35571
	int GetNetworkID();

	// Token: 0x06008AF4 RID: 35572
	UtilityNetwork GetNetworkForDirection(Direction d);
}
