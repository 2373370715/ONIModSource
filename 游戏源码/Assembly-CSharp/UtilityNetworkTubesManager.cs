using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001A1E RID: 6686
public class UtilityNetworkTubesManager : UtilityNetworkManager<TravelTubeNetwork, TravelTube>
{
	// Token: 0x06008B6F RID: 35695 RVA: 0x000FB246 File Offset: 0x000F9446
	public UtilityNetworkTubesManager(int game_width, int game_height, int tile_layer) : base(game_width, game_height, tile_layer)
	{
	}

	// Token: 0x06008B70 RID: 35696 RVA: 0x000FB251 File Offset: 0x000F9451
	public override bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		return this.TestForUTurnLeft(cell, new_connection, is_physical_building, out fail_reason) && this.TestForUTurnRight(cell, new_connection, is_physical_building, out fail_reason) && this.TestForNoAdjacentBridge(cell, new_connection, out fail_reason);
	}

	// Token: 0x06008B71 RID: 35697 RVA: 0x000FB279 File Offset: 0x000F9479
	public override void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		base.SetConnections(connections, cell, is_physical_building);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
	}

	// Token: 0x06008B72 RID: 35698 RVA: 0x0035FC20 File Offset: 0x0035DE20
	private bool TestForUTurnLeft(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.LeftDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06008B73 RID: 35699 RVA: 0x0035FC7C File Offset: 0x0035DE7C
	private bool TestForUTurnRight(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.RightDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06008B74 RID: 35700 RVA: 0x0035FCD8 File Offset: 0x0035DED8
	private bool TestForNoAdjacentBridge(int cell, UtilityConnections connection, out string fail_reason)
	{
		UtilityConnections direction = connection.LeftDirection();
		UtilityConnections direction2 = connection.RightDirection();
		int cell2 = direction.CellInDirection(cell);
		int cell3 = direction2.CellInDirection(cell);
		GameObject gameObject = Grid.Objects[cell2, 9];
		GameObject gameObject2 = Grid.Objects[cell3, 9];
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_STRAIGHT_BRIDGES;
		return (gameObject == null || gameObject.GetComponent<TravelTubeBridge>() == null) && (gameObject2 == null || gameObject2.GetComponent<TravelTubeBridge>() == null);
	}

	// Token: 0x06008B75 RID: 35701 RVA: 0x000FB28F File Offset: 0x000F948F
	private bool HasConnection(int cell, UtilityConnections connection, bool is_physical_building)
	{
		return (base.GetConnections(cell, is_physical_building) & connection) > (UtilityConnections)0;
	}
}
