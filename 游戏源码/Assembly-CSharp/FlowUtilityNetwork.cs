using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A12 RID: 6674
public class FlowUtilityNetwork : UtilityNetwork
{
	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06008B07 RID: 35591 RVA: 0x000FAF30 File Offset: 0x000F9130
	public bool HasSinks
	{
		get
		{
			return this.sinks.Count > 0;
		}
	}

	// Token: 0x06008B08 RID: 35592 RVA: 0x000FAF40 File Offset: 0x000F9140
	public int GetActiveCount()
	{
		return this.sinks.Count;
	}

	// Token: 0x06008B09 RID: 35593 RVA: 0x0035E3CC File Offset: 0x0035C5CC
	public override void AddItem(object generic_item)
	{
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)generic_item;
		if (item != null)
		{
			switch (item.EndpointType)
			{
			case Endpoint.Source:
				if (this.sources.Contains(item))
				{
					return;
				}
				this.sources.Add(item);
				item.Network = this;
				return;
			case Endpoint.Sink:
				if (this.sinks.Contains(item))
				{
					return;
				}
				this.sinks.Add(item);
				item.Network = this;
				return;
			case Endpoint.Conduit:
				this.conduitCount++;
				return;
			default:
				item.Network = this;
				break;
			}
		}
	}

	// Token: 0x06008B0A RID: 35594 RVA: 0x0035E45C File Offset: 0x0035C65C
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < this.sinks.Count; i++)
		{
			FlowUtilityNetwork.IItem item = this.sinks[i];
			item.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode = grid[item.Cell];
			utilityNetworkGridNode.networkIdx = -1;
			grid[item.Cell] = utilityNetworkGridNode;
		}
		for (int j = 0; j < this.sources.Count; j++)
		{
			FlowUtilityNetwork.IItem item2 = this.sources[j];
			item2.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode2 = grid[item2.Cell];
			utilityNetworkGridNode2.networkIdx = -1;
			grid[item2.Cell] = utilityNetworkGridNode2;
		}
		this.conduitCount = 0;
		for (int k = 0; k < this.conduits.Count; k++)
		{
			FlowUtilityNetwork.IItem item3 = this.conduits[k];
			item3.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode3 = grid[item3.Cell];
			utilityNetworkGridNode3.networkIdx = -1;
			grid[item3.Cell] = utilityNetworkGridNode3;
		}
	}

	// Token: 0x040068AC RID: 26796
	public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x040068AD RID: 26797
	public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x040068AE RID: 26798
	public List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x040068AF RID: 26799
	public int conduitCount;

	// Token: 0x02001A13 RID: 6675
	public interface IItem
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06008B0C RID: 35596
		int Cell { get; }

		// Token: 0x17000915 RID: 2325
		// (set) Token: 0x06008B0D RID: 35597
		FlowUtilityNetwork Network { set; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06008B0E RID: 35598
		Endpoint EndpointType { get; }

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06008B0F RID: 35599
		ConduitType ConduitType { get; }

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06008B10 RID: 35600
		GameObject GameObject { get; }
	}

	// Token: 0x02001A14 RID: 6676
	public class NetworkItem : FlowUtilityNetwork.IItem
	{
		// Token: 0x06008B11 RID: 35601 RVA: 0x000FAF76 File Offset: 0x000F9176
		public NetworkItem(ConduitType conduit_type, Endpoint endpoint_type, int cell, GameObject parent)
		{
			this.conduitType = conduit_type;
			this.endpointType = endpoint_type;
			this.cell = cell;
			this.parent = parent;
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06008B12 RID: 35602 RVA: 0x000FAF9B File Offset: 0x000F919B
		public Endpoint EndpointType
		{
			get
			{
				return this.endpointType;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06008B13 RID: 35603 RVA: 0x000FAFA3 File Offset: 0x000F91A3
		public ConduitType ConduitType
		{
			get
			{
				return this.conduitType;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06008B14 RID: 35604 RVA: 0x000FAFAB File Offset: 0x000F91AB
		public int Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06008B15 RID: 35605 RVA: 0x000FAFB3 File Offset: 0x000F91B3
		// (set) Token: 0x06008B16 RID: 35606 RVA: 0x000FAFBB File Offset: 0x000F91BB
		public FlowUtilityNetwork Network
		{
			get
			{
				return this.network;
			}
			set
			{
				this.network = value;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06008B17 RID: 35607 RVA: 0x000FAFC4 File Offset: 0x000F91C4
		public GameObject GameObject
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x040068B0 RID: 26800
		private int cell;

		// Token: 0x040068B1 RID: 26801
		private FlowUtilityNetwork network;

		// Token: 0x040068B2 RID: 26802
		private Endpoint endpointType;

		// Token: 0x040068B3 RID: 26803
		private ConduitType conduitType;

		// Token: 0x040068B4 RID: 26804
		private GameObject parent;
	}
}
