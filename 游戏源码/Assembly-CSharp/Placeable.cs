using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
	public enum PlacementRules
	{
		OnFoundation,
		VisibleToSpace,
		RestrictToWorld
	}

	[MyCmpReq]
	private OccupyArea occupyArea;

	public string kAnimName;

	public string animName;

	public List<PlacementRules> placementRules = new List<PlacementRules>();

	[NonSerialized]
	public int restrictWorldId;

	public bool checkRootCellOnly;

	public bool IsValidPlaceLocation(int cell, out string reason)
	{
		if (placementRules.Contains(PlacementRules.RestrictToWorld) && Grid.WorldIdx[cell] != restrictWorldId)
		{
			reason = UI.TOOLS.PLACE.REASONS.RESTRICT_TO_WORLD;
			return false;
		}
		if (!occupyArea.CanOccupyArea(cell, occupyArea.objectLayers[0]))
		{
			reason = UI.TOOLS.PLACE.REASONS.CAN_OCCUPY_AREA;
			return false;
		}
		if (placementRules.Contains(PlacementRules.OnFoundation))
		{
			bool flag = occupyArea.TestAreaBelow(cell, null, FoundationTest);
			if (checkRootCellOnly)
			{
				flag = FoundationTest(Grid.CellBelow(cell), null);
			}
			if (!flag)
			{
				reason = UI.TOOLS.PLACE.REASONS.ON_FOUNDATION;
				return false;
			}
		}
		if (placementRules.Contains(PlacementRules.VisibleToSpace))
		{
			bool flag2 = occupyArea.TestArea(cell, null, SunnySpaceTest);
			if (checkRootCellOnly)
			{
				flag2 = SunnySpaceTest(cell, null);
			}
			if (!flag2)
			{
				reason = UI.TOOLS.PLACE.REASONS.VISIBLE_TO_SPACE;
				return false;
			}
		}
		reason = "ok!";
		return true;
	}

	private bool SunnySpaceTest(int cell, object data)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		Grid.CellToXY(cell, out var x, out var y);
		int num = Grid.WorldIdx[cell];
		if (num == 255)
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(num);
		int top = world.WorldOffset.y + world.WorldSize.y;
		if (!Grid.Solid[cell] && !Grid.Foundation[cell])
		{
			if (Grid.ExposedToSunlight[cell] < 253)
			{
				return ClearPathToSky(x, y, top);
			}
			return true;
		}
		return false;
	}

	private bool ClearPathToSky(int x, int startY, int top)
	{
		for (int i = startY; i < top; i++)
		{
			int i2 = Grid.XYToCell(x, i);
			if (Grid.Solid[i2] || Grid.Foundation[i2])
			{
				return false;
			}
		}
		return true;
	}

	private bool FoundationTest(int cell, object data)
	{
		if (Grid.IsValidBuildingCell(cell))
		{
			if (!Grid.Solid[cell])
			{
				return Grid.Foundation[cell];
			}
			return true;
		}
		return false;
	}
}
