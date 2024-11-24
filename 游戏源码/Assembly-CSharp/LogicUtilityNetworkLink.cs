using System;
using System.Collections.Generic;

// Token: 0x020014B9 RID: 5305
public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06006E8D RID: 28301 RVA: 0x000E872C File Offset: 0x000E692C
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006E8E RID: 28302 RVA: 0x000E8734 File Offset: 0x000E6934
	protected override void OnConnect(int cell1, int cell2)
	{
		this.cell_one = cell1;
		this.cell_two = cell2;
		Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Connect(this);
	}

	// Token: 0x06006E8F RID: 28303 RVA: 0x000E8765 File Offset: 0x000E6965
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Disconnect(this);
	}

	// Token: 0x06006E90 RID: 28304 RVA: 0x000D019B File Offset: 0x000CE39B
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06006E91 RID: 28305 RVA: 0x002EF9CC File Offset: 0x002EDBCC
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06006E92 RID: 28306 RVA: 0x002EF9F8 File Offset: 0x002EDBF8
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040052AB RID: 21163
	public LogicWire.BitDepth bitDepth;

	// Token: 0x040052AC RID: 21164
	public int cell_one;

	// Token: 0x040052AD RID: 21165
	public int cell_two;
}
