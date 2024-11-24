using System.Collections.Generic;
using UnityEngine;

public class StaterpillarCellQuery : PathFinderQuery
{
	public List<int> result_cells = new List<int>();

	private int max_results;

	private GameObject tester;

	private ObjectLayer connectorLayer;

	public StaterpillarCellQuery Reset(int max_results, GameObject tester, ObjectLayer conduitLayer)
	{
		this.max_results = max_results;
		this.tester = tester;
		result_cells.Clear();
		connectorLayer = conduitLayer switch
		{
			ObjectLayer.Wire => ObjectLayer.WireConnectors, 
			ObjectLayer.GasConduit => ObjectLayer.GasConduitConnection, 
			ObjectLayer.LiquidConduit => ObjectLayer.LiquidConduitConnection, 
			ObjectLayer.SolidConduit => ObjectLayer.SolidConduitConnection, 
			_ => conduitLayer, 
		};
		return this;
	}

	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!result_cells.Contains(cell) && CheckValidRoofCell(cell))
		{
			result_cells.Add(cell);
		}
		return result_cells.Count >= max_results;
	}

	private bool CheckValidRoofCell(int testCell)
	{
		if (!tester.GetComponent<Navigator>().NavGrid.NavTable.IsValid(testCell, NavType.Ceiling))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Down);
		if (!Grid.ObjectLayers[1].ContainsKey(testCell) && !Grid.ObjectLayers[1].ContainsKey(cellInDirection) && !Grid.Objects[cellInDirection, (int)connectorLayer] && Grid.IsValidBuildingCell(testCell) && Grid.IsValidCell(cellInDirection) && Grid.IsValidBuildingCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection))
		{
			return true;
		}
		return false;
	}
}
