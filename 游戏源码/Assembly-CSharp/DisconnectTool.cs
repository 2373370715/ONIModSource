using System;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectTool : FilteredDragTool
{
	public struct VisData
	{
		public readonly int cell1;

		public readonly int cell2;

		public GameObject go;

		public VisData(int cell1, int cell2, GameObject go)
		{
			this.cell1 = cell1;
			this.cell2 = cell2;
			this.go = go;
		}

		public bool Equals(int cell1, int cell2)
		{
			if (this.cell1 != cell1 || this.cell2 != cell2)
			{
				if (this.cell1 == cell2)
				{
					return this.cell2 == cell1;
				}
				return false;
			}
			return true;
		}
	}

	[SerializeField]
	private GameObject disconnectVisSingleModePrefab;

	[SerializeField]
	private GameObject disconnectVisMultiModePrefab;

	private GameObjectPool disconnectVisPool;

	private List<VisData> visualizersInUse = new List<VisData>();

	private int lastRefreshedCell;

	private bool singleDisconnectMode = true;

	public static DisconnectTool Instance;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		disconnectVisPool = new GameObjectPool(InstantiateDisconnectVis, singleDisconnectMode ? 1 : 10);
		if (singleDisconnectMode)
		{
			lineModeMaxLength = 2;
		}
	}

	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	protected override Mode GetMode()
	{
		if (!singleDisconnectMode)
		{
			return Mode.Box;
		}
		return Mode.Line;
	}

	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		if (singleDisconnectMode)
		{
			upPos = SnapToLine(upPos);
		}
		RunOnRegion(downPos, upPos, DisconnectCellsAction);
		ClearVisualizers();
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		lastRefreshedCell = -1;
	}

	private void DisconnectCellsAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		Building component = objectOnCell.GetComponent<Building>();
		KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
		if (component2 != null)
		{
			UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, is_physical_building: false) & ~removeConnections;
			component2.UpdateConnections(new_connections);
			component2.Refresh();
		}
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		utilityComponent.GetNetworkManager().ForceRebuildNetworks();
	}

	private void RunOnRegion(Vector3 pos1, Vector3 pos2, Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
	{
		Vector2 regularizedPos = GetRegularizedPos(Vector2.Min(pos1, pos2), minimize: true);
		Vector2 regularizedPos2 = GetRegularizedPos(Vector2.Max(pos1, pos2), minimize: false);
		Vector2I min = new Vector2I((int)regularizedPos.x, (int)regularizedPos.y);
		Vector2I max = new Vector2I((int)regularizedPos2.x, (int)regularizedPos2.y);
		for (int i = min.x; i < max.x; i++)
		{
			for (int j = min.y; j < max.y; j++)
			{
				int num = Grid.XYToCell(i, j);
				if (!Grid.IsVisible(num))
				{
					continue;
				}
				for (int k = 0; k < 45; k++)
				{
					GameObject gameObject = Grid.Objects[num, k];
					if (gameObject == null)
					{
						continue;
					}
					string filterLayerFromGameObject = GetFilterLayerFromGameObject(gameObject);
					if (!IsActiveLayer(filterLayerFromGameObject))
					{
						continue;
					}
					Building component = gameObject.GetComponent<Building>();
					if (component == null)
					{
						continue;
					}
					IHaveUtilityNetworkMgr component2 = component.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
					if (!component2.IsNullOrDestroyed())
					{
						UtilityConnections connections = component2.GetNetworkManager().GetConnections(num, is_physical_building: false);
						UtilityConnections utilityConnections = (UtilityConnections)0;
						if ((connections & UtilityConnections.Left) > (UtilityConnections)0 && IsInsideRegion(min, max, num, -1, 0))
						{
							utilityConnections |= UtilityConnections.Left;
						}
						if ((connections & UtilityConnections.Right) > (UtilityConnections)0 && IsInsideRegion(min, max, num, 1, 0))
						{
							utilityConnections |= UtilityConnections.Right;
						}
						if ((connections & UtilityConnections.Up) > (UtilityConnections)0 && IsInsideRegion(min, max, num, 0, 1))
						{
							utilityConnections |= UtilityConnections.Up;
						}
						if ((connections & UtilityConnections.Down) > (UtilityConnections)0 && IsInsideRegion(min, max, num, 0, -1))
						{
							utilityConnections |= UtilityConnections.Down;
						}
						if (utilityConnections > (UtilityConnections)0)
						{
							action(num, gameObject, component2, utilityConnections);
						}
					}
				}
			}
		}
	}

	private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
	{
		Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out var x, out var y);
		if (x >= min.x && x < max.x && y >= min.y)
		{
			return y < max.y;
		}
		return false;
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		if (base.Dragging)
		{
			cursorPos = ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
			if (singleDisconnectMode)
			{
				cursorPos = SnapToLine(cursorPos);
			}
			int num = Grid.PosToCell(cursorPos);
			if (lastRefreshedCell != num)
			{
				lastRefreshedCell = num;
				ClearVisualizers();
				RunOnRegion(downPos, cursorPos, VisualizeAction);
			}
		}
	}

	private GameObject InstantiateDisconnectVis()
	{
		GameObject obj = GameUtil.KInstantiate(singleDisconnectMode ? disconnectVisSingleModePrefab : disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront);
		obj.SetActive(value: false);
		return obj;
	}

	private void VisualizeAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		if ((removeConnections & UtilityConnections.Down) != 0)
		{
			CreateVisualizer(cell, Grid.CellBelow(cell), rotate: true);
		}
		if ((removeConnections & UtilityConnections.Right) != 0)
		{
			CreateVisualizer(cell, Grid.CellRight(cell), rotate: false);
		}
	}

	private void CreateVisualizer(int cell1, int cell2, bool rotate)
	{
		foreach (VisData item in visualizersInUse)
		{
			if (item.Equals(cell1, cell2))
			{
				return;
			}
		}
		Vector3 a = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
		Vector3 b = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
		GameObject instance = disconnectVisPool.GetInstance();
		instance.transform.rotation = Quaternion.Euler(0f, 0f, rotate ? 90 : 0);
		instance.transform.SetPosition(Vector3.Lerp(a, b, 0.5f));
		instance.SetActive(value: true);
		visualizersInUse.Add(new VisData(cell1, cell2, instance));
	}

	private void ClearVisualizers()
	{
		foreach (VisData item in visualizersInUse)
		{
			item.go.SetActive(value: false);
			disconnectVisPool.ReleaseInstance(item.go);
		}
		visualizersInUse.Clear();
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ClearVisualizers();
	}

	protected override string GetConfirmSound()
	{
		return "OutletDisconnected";
	}

	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
	}
}
