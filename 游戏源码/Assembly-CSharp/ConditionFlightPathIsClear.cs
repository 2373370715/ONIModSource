using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class ConditionFlightPathIsClear : ProcessCondition
{
	private CraftModuleInterface moduleInterface;

	private RocketModule module;

	private int bufferWidth;

	private bool hasClearSky;

	private int obstructedTile = -1;

	public const int MAXIMUM_ROCKET_HEIGHT = 35;

	public const float FIRE_FX_HEIGHT = 10f;

	public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
	{
		this.module = module.GetComponent<RocketModule>();
		if (this.module is RocketModuleCluster)
		{
			moduleInterface = (this.module as RocketModuleCluster).CraftInterface;
		}
		this.bufferWidth = bufferWidth;
	}

	public override Status EvaluateCondition()
	{
		Update();
		if (!hasClearSky)
		{
			return Status.Failure;
		}
		return Status.Ready;
	}

	public override StatusItem GetStatusItem(Status status)
	{
		if (status == Status.Failure)
		{
			return Db.Get().BuildingStatusItems.PathNotClear;
		}
		return null;
	}

	public override string GetStatusMessage(Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.FAILURE;
		}
		if (status != Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationText;
		}
		Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	public override string GetStatusTooltip(Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.FAILURE;
		}
		if (status != Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationTooltipText;
		}
		Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	public override bool ShowInUI()
	{
		return DlcManager.FeatureClusterSpaceEnabled();
	}

	public void Update()
	{
		List<Building> list = new List<Building>();
		if (moduleInterface != null)
		{
			foreach (Ref<RocketModuleCluster> item in new List<Ref<RocketModuleCluster>>(moduleInterface.ClusterModules))
			{
				list.Add(item.Get().GetComponent<Building>());
			}
		}
		else
		{
			foreach (RocketModule rocketModule in module.FindLaunchConditionManager().rocketModules)
			{
				list.Add(rocketModule.GetComponent<Building>());
			}
		}
		list.Sort(delegate(Building a, Building b)
		{
			int y = Grid.PosToXY(a.transform.GetPosition()).y;
			int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
			return y.CompareTo(y2);
		});
		if (moduleInterface != null && moduleInterface.CurrentPad == null)
		{
			hasClearSky = false;
			return;
		}
		hasClearSky = true;
		int obstructionCell = -1;
		int num = 0;
		while (hasClearSky && num < list.Count)
		{
			Building building = list[num];
			hasClearSky = HasModuleAccessToSpace(building, out obstructionCell);
			num++;
		}
	}

	public static bool HasModuleAccessToSpace(Building module, out int obstructionCell)
	{
		WorldContainer myWorld = module.GetMyWorld();
		obstructionCell = -1;
		if (myWorld.id == 255)
		{
			return false;
		}
		int num = (int)myWorld.maximumBounds.y;
		Extents extents = module.GetExtents();
		int cell = Grid.XYToCell(extents.x, extents.y);
		bool result = true;
		for (int i = 0; i < extents.width; i++)
		{
			int num2 = Grid.OffsetCell(cell, new CellOffset(i, 0));
			while (!Grid.IsSolidCell(num2) && Grid.CellToXY(num2).y < num)
			{
				num2 = Grid.CellAbove(num2);
			}
			if (Grid.IsSolidCell(num2) || Grid.CellToXY(num2).y != num)
			{
				obstructionCell = num2;
				result = false;
				break;
			}
		}
		return result;
	}

	public static int PadTopEdgeDistanceToOutOfScreenEdge(GameObject launchpad)
	{
		WorldContainer myWorld = launchpad.GetMyWorld();
		_ = myWorld.maximumBounds;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return (int)CameraController.GetHighestVisibleCell_Height((byte)myWorld.ParentWorldId) - y + 10;
	}

	public static int PadTopEdgeDistanceToCeilingEdge(GameObject launchpad)
	{
		_ = launchpad.GetMyWorld().maximumBounds;
		int num = (int)launchpad.GetMyWorld().maximumBounds.y;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return num - Grid.TopBorderHeight - y + 1;
	}

	public static bool CheckFlightPathClear(CraftModuleInterface craft, GameObject launchpad, out int obstruction)
	{
		Vector2I vector2I = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition);
		int num = PadTopEdgeDistanceToCeilingEdge(launchpad);
		foreach (Ref<RocketModuleCluster> clusterModule in craft.ClusterModules)
		{
			Building component = clusterModule.Get().GetComponent<Building>();
			int widthInCells = component.Def.WidthInCells;
			int moduleRelativeVerticalPosition = craft.GetModuleRelativeVerticalPosition(clusterModule.Get().gameObject);
			if (moduleRelativeVerticalPosition + component.Def.HeightInCells > num)
			{
				int num2 = Grid.XYToCell(vector2I.x, moduleRelativeVerticalPosition + vector2I.y);
				obstruction = num2;
				return false;
			}
			for (int i = moduleRelativeVerticalPosition; i < num; i++)
			{
				for (int j = 0; j < widthInCells; j++)
				{
					int num3 = Grid.XYToCell(j + (vector2I.x - widthInCells / 2), i + vector2I.y);
					bool flag = Grid.Solid[num3];
					if (!Grid.IsValidCell(num3) || Grid.WorldIdx[num3] != Grid.WorldIdx[launchpad.GetComponent<LaunchPad>().RocketBottomPosition] || flag)
					{
						obstruction = num3;
						return false;
					}
				}
			}
		}
		obstruction = -1;
		return true;
	}

	private static bool CanReachSpace(int startCell, out int obstruction, out int highestCellInSky)
	{
		WorldContainer worldContainer = ((startCell >= 0) ? ClusterManager.Instance.GetWorld(Grid.WorldIdx[startCell]) : null);
		int num = (highestCellInSky = ((worldContainer == null) ? Grid.HeightInCells : ((int)worldContainer.maximumBounds.y)));
		obstruction = -1;
		int num2 = startCell;
		while (Grid.CellRow(num2) < num)
		{
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
			{
				obstruction = num2;
				return false;
			}
			num2 = Grid.CellAbove(num2);
		}
		return true;
	}

	public string GetObstruction()
	{
		if (obstructedTile == -1)
		{
			return null;
		}
		if (Grid.Objects[obstructedTile, 1] != null)
		{
			return Grid.Objects[obstructedTile, 1].GetComponent<Building>().Def.Name;
		}
		return string.Format(BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, Grid.Element[obstructedTile].tag.ProperName());
	}
}
