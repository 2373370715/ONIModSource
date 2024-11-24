using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A49 RID: 6729
public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem, ICircuitConnected
{
	// Token: 0x06008C55 RID: 35925 RVA: 0x000FBB78 File Offset: 0x000F9D78
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06008C56 RID: 35926 RVA: 0x000E872C File Offset: 0x000E692C
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06008C57 RID: 35927 RVA: 0x000FBB80 File Offset: 0x000F9D80
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06008C58 RID: 35928 RVA: 0x000FBBA3 File Offset: 0x000F9DA3
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
		Game.Instance.circuitManager.Connect(this);
	}

	// Token: 0x06008C59 RID: 35929 RVA: 0x000D793E File Offset: 0x000D5B3E
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x06008C5A RID: 35930 RVA: 0x000FBBC6 File Offset: 0x000F9DC6
	// (set) Token: 0x06008C5B RID: 35931 RVA: 0x000FBBCE File Offset: 0x000F9DCE
	public bool IsVirtual { get; private set; }

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x06008C5C RID: 35932 RVA: 0x000FBBD7 File Offset: 0x000F9DD7
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x06008C5D RID: 35933 RVA: 0x000FBBDF File Offset: 0x000F9DDF
	// (set) Token: 0x06008C5E RID: 35934 RVA: 0x000FBBE7 File Offset: 0x000F9DE7
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06008C5F RID: 35935 RVA: 0x00362B1C File Offset: 0x00360D1C
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06008C60 RID: 35936 RVA: 0x00362B48 File Offset: 0x00360D48
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x0400699F RID: 27039
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
