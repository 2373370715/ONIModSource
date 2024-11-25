﻿using System;

public class WireBuildTool : BaseUtilityBuildTool
{
		public static void DestroyInstance()
	{
		WireBuildTool.Instance = null;
	}

		protected override void OnPrefabInit()
	{
		WireBuildTool.Instance = this;
		base.OnPrefabInit();
		this.viewMode = OverlayModes.Power.ID;
	}

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

		public static WireBuildTool Instance;
}
