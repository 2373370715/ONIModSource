using System;
using System.Collections.Generic;

// Token: 0x02001A1A RID: 6682
public interface IUtilityNetworkMgr
{
	// Token: 0x06008B30 RID: 35632
	bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason);

	// Token: 0x06008B31 RID: 35633
	void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

	// Token: 0x06008B32 RID: 35634
	void StashVisualGrids();

	// Token: 0x06008B33 RID: 35635
	void UnstashVisualGrids();

	// Token: 0x06008B34 RID: 35636
	string GetVisualizerString(int cell);

	// Token: 0x06008B35 RID: 35637
	string GetVisualizerString(UtilityConnections connections);

	// Token: 0x06008B36 RID: 35638
	UtilityConnections GetConnections(int cell, bool is_physical_building);

	// Token: 0x06008B37 RID: 35639
	UtilityConnections GetDisplayConnections(int cell);

	// Token: 0x06008B38 RID: 35640
	void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

	// Token: 0x06008B39 RID: 35641
	void ClearCell(int cell, bool is_physical_building);

	// Token: 0x06008B3A RID: 35642
	void ForceRebuildNetworks();

	// Token: 0x06008B3B RID: 35643
	void AddToNetworks(int cell, object item, bool is_endpoint);

	// Token: 0x06008B3C RID: 35644
	void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

	// Token: 0x06008B3D RID: 35645
	object GetEndpoint(int cell);

	// Token: 0x06008B3E RID: 35646
	UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

	// Token: 0x06008B3F RID: 35647
	UtilityNetwork GetNetworkForCell(int cell);

	// Token: 0x06008B40 RID: 35648
	void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06008B41 RID: 35649
	void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06008B42 RID: 35650
	IList<UtilityNetwork> GetNetworks();
}
