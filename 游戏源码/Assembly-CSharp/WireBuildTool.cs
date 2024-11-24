using System;

// Token: 0x0200146D RID: 5229
public class WireBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06006C73 RID: 27763 RVA: 0x000E7362 File Offset: 0x000E5562
	public static void DestroyInstance()
	{
		WireBuildTool.Instance = null;
	}

	// Token: 0x06006C74 RID: 27764 RVA: 0x000E736A File Offset: 0x000E556A
	protected override void OnPrefabInit()
	{
		WireBuildTool.Instance = this;
		base.OnPrefabInit();
		this.viewMode = OverlayModes.Power.ID;
	}

	// Token: 0x06006C75 RID: 27765 RVA: 0x002E7F9C File Offset: 0x002E619C
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, this.path[i].cell);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					this.conduitMgr.AddConnection(utilityConnections, cell, false);
					this.conduitMgr.AddConnection(new_connection, cell2, false);
				}
			}
		}
	}

	// Token: 0x0400514E RID: 20814
	public static WireBuildTool Instance;
}
