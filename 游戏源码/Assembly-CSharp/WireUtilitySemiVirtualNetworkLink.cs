using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A4A RID: 6730
public class WireUtilitySemiVirtualNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
{
	// Token: 0x06008C62 RID: 35938 RVA: 0x000FBBF0 File Offset: 0x000F9DF0
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06008C63 RID: 35939 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06008C64 RID: 35940 RVA: 0x00362B70 File Offset: 0x00360D70
	protected override void OnSpawn()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.VirtualCircuitKey = component.CraftInterface;
		}
		else
		{
			CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
			if (component2 != null)
			{
				this.VirtualCircuitKey = component2;
			}
		}
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x06008C65 RID: 35941 RVA: 0x00362BD4 File Offset: 0x00360DD4
	public void SetLinkConnected(bool connect)
	{
		if (connect && this.visualizeOnly)
		{
			this.visualizeOnly = false;
			if (base.isSpawned)
			{
				base.Connect();
				return;
			}
		}
		else if (!connect && !this.visualizeOnly)
		{
			if (base.isSpawned)
			{
				base.Disconnect();
			}
			this.visualizeOnly = true;
		}
	}

	// Token: 0x06008C66 RID: 35942 RVA: 0x000FBBF8 File Offset: 0x000F9DF8
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06008C67 RID: 35943 RVA: 0x000FBC10 File Offset: 0x000F9E10
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06008C68 RID: 35944 RVA: 0x000D793E File Offset: 0x000D5B3E
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06008C69 RID: 35945 RVA: 0x000FBC28 File Offset: 0x000F9E28
	// (set) Token: 0x06008C6A RID: 35946 RVA: 0x000FBC30 File Offset: 0x000F9E30
	public bool IsVirtual { get; private set; }

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06008C6B RID: 35947 RVA: 0x000FBBD7 File Offset: 0x000F9DD7
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x06008C6C RID: 35948 RVA: 0x000FBC39 File Offset: 0x000F9E39
	// (set) Token: 0x06008C6D RID: 35949 RVA: 0x000FBC41 File Offset: 0x000F9E41
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06008C6E RID: 35950 RVA: 0x00362C24 File Offset: 0x00360E24
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06008C6F RID: 35951 RVA: 0x00362C50 File Offset: 0x00360E50
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040069A2 RID: 27042
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
