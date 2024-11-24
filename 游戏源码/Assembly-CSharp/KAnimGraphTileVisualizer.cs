using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x0600312A RID: 12586 RVA: 0x000BFF0C File Offset: 0x000BE10C
	// (set) Token: 0x0600312B RID: 12587 RVA: 0x000BFF14 File Offset: 0x000BE114
	public UtilityConnections Connections
	{
		get
		{
			return this._connections;
		}
		set
		{
			this._connections = value;
			base.Trigger(-1041684577, this._connections);
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600312C RID: 12588 RVA: 0x001FE3AC File Offset: 0x001FC5AC
	public IUtilityNetworkMgr ConnectionManager
	{
		get
		{
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				return Game.Instance.gasConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				return Game.Instance.liquidConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				return Game.Instance.electricalConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				return Game.Instance.logicCircuitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				return Game.Instance.travelTubeSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				return Game.Instance.solidConduitSystem;
			default:
				return null;
			}
		}
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x001FE424 File Offset: 0x001FC624
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.connectionManager = this.ConnectionManager;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
		Building component = base.GetComponent<Building>();
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x001FE490 File Offset: 0x001FC690
	protected override void OnCleanUp()
	{
		if (this.connectionManager != null && !this.skipCleanup)
		{
			this.skipRefresh = true;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
			Building component = base.GetComponent<Building>();
			TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		}
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x001FE4FC File Offset: 0x001FC6FC
	[ContextMenu("Refresh")]
	public void Refresh()
	{
		if (this.connectionManager == null || this.skipRefresh)
		{
			return;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			string text = this.connectionManager.GetVisualizerString(cell);
			if (base.GetComponent<BuildingUnderConstruction>() != null && component.HasAnimation(text + "_place"))
			{
				text += "_place";
			}
			if (text != null && text != "")
			{
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x001FE5BC File Offset: 0x001FC7BC
	public int GetNetworkID()
	{
		UtilityNetwork network = this.GetNetwork();
		if (network == null)
		{
			return -1;
		}
		return network.id;
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x001FE5DC File Offset: 0x001FC7DC
	private UtilityNetwork GetNetwork()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, Direction.None);
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x001FE608 File Offset: 0x001FC808
	public UtilityNetwork GetNetworkForDirection(Direction d)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, d);
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x001FE634 File Offset: 0x001FC834
	public void UpdateConnections(UtilityConnections new_connections)
	{
		this._connections = new_connections;
		if (this.connectionManager != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x001FE674 File Offset: 0x001FC874
	public KAnimGraphTileVisualizer GetNeighbour(Direction d)
	{
		KAnimGraphTileVisualizer result = null;
		Vector2I vector2I;
		Grid.PosToXY(base.transform.GetPosition(), out vector2I);
		int num = -1;
		switch (d)
		{
		case Direction.Up:
			if (vector2I.y < Grid.HeightInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y + 1);
			}
			break;
		case Direction.Right:
			if (vector2I.x < Grid.WidthInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x + 1, vector2I.y);
			}
			break;
		case Direction.Down:
			if (vector2I.y > 0)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y - 1);
			}
			break;
		case Direction.Left:
			if (vector2I.x > 0)
			{
				num = Grid.XYToCell(vector2I.x - 1, vector2I.y);
			}
			break;
		}
		if (num != -1)
		{
			ObjectLayer layer;
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				layer = ObjectLayer.GasConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				layer = ObjectLayer.LiquidConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				layer = ObjectLayer.WireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				layer = ObjectLayer.LogicWireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				layer = ObjectLayer.TravelTubeTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				layer = ObjectLayer.SolidConduitTile;
				break;
			default:
				throw new ArgumentNullException("wtf");
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (gameObject != null)
			{
				result = gameObject.GetComponent<KAnimGraphTileVisualizer>();
			}
		}
		return result;
	}

	// Token: 0x04002127 RID: 8487
	[Serialize]
	private UtilityConnections _connections;

	// Token: 0x04002128 RID: 8488
	public bool isPhysicalBuilding;

	// Token: 0x04002129 RID: 8489
	public bool skipCleanup;

	// Token: 0x0400212A RID: 8490
	public bool skipRefresh;

	// Token: 0x0400212B RID: 8491
	public KAnimGraphTileVisualizer.ConnectionSource connectionSource;

	// Token: 0x0400212C RID: 8492
	[NonSerialized]
	public IUtilityNetworkMgr connectionManager;

	// Token: 0x02000A6F RID: 2671
	public enum ConnectionSource
	{
		// Token: 0x0400212E RID: 8494
		Gas,
		// Token: 0x0400212F RID: 8495
		Liquid,
		// Token: 0x04002130 RID: 8496
		Electrical,
		// Token: 0x04002131 RID: 8497
		Logic,
		// Token: 0x04002132 RID: 8498
		Tube,
		// Token: 0x04002133 RID: 8499
		Solid
	}
}
