using System;

// Token: 0x020019F7 RID: 6647
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
	// Token: 0x06008A84 RID: 35460 RVA: 0x000E872C File Offset: 0x000E692C
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06008A85 RID: 35461 RVA: 0x000FAAF2 File Offset: 0x000F8CF2
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
	}

	// Token: 0x06008A86 RID: 35462 RVA: 0x000FAB05 File Offset: 0x000F8D05
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
	}

	// Token: 0x06008A87 RID: 35463 RVA: 0x000D67D0 File Offset: 0x000D49D0
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}
}
