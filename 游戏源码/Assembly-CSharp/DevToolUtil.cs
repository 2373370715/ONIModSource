using System;
using UnityEngine;

public static class DevToolUtil
{
	public enum TextAlignment
	{
		Center,
		Left,
		Right
	}

	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	public static DevPanel DebugObject<T>(T obj)
	{
		return Open(new DevToolObjectViewer<T>(() => obj));
	}

	public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return Open(new DevToolObjectViewer<T>(get_obj_fn));
	}

	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	public static string GenerateDevToolName(DevTool devTool)
	{
		return GenerateDevToolName(devTool.GetType());
	}

	public static string GenerateDevToolName(Type devToolType)
	{
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out var value))
		{
			return value;
		}
		string text = devToolType.Name;
		if (text.StartsWith("DevTool_"))
		{
			text = text.Substring("DevTool_".Length);
		}
		else if (text.StartsWith("DevTool"))
		{
			text = text.Substring("DevTool".Length);
		}
		return text;
	}

	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		int cellIndex;
		return TryGetCellIndexFor(gameObject, out cellIndex);
	}

	public static void RevealAndFocus(GameObject gameObject)
	{
		if (!TryGetCellIndexFor(gameObject, out var cellIndex))
		{
			RevealAndFocusAt(cellIndex);
			if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
			}
			else
			{
				SelectTool.Instance.Select(null);
			}
		}
	}

	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	public static bool TryGetCellIndexFor(GameObject gameObject, out int cellIndex)
	{
		cellIndex = -1;
		if (gameObject.IsNullOrDestroyed())
		{
			return false;
		}
		if (!gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
		{
			return false;
		}
		cellIndex = Grid.PosToCell(gameObject);
		return true;
	}

	public static bool TryGetCellIndexForUniqueBuilding(string prefabId, out int index)
	{
		index = -1;
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(includeInactive: true);
		if (array == null)
		{
			return false;
		}
		BuildingComplete[] array2 = array;
		foreach (BuildingComplete buildingComplete in array2)
		{
			if (prefabId == buildingComplete.Def.PrefabID)
			{
				index = buildingComplete.GetCell();
				return true;
			}
		}
		return false;
	}

	public static void RevealAndFocusAt(int cellIndex)
	{
		Grid.CellToXY(cellIndex, out var x, out var y);
		GridVisibility.Reveal(x + 2, y + 2, 10, 10f);
		FocusCameraOnCell(cellIndex);
		if (TryGetCellIndexForUniqueBuilding("Headquarters", out var index))
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(index);
			float num = 2f / Vector3.Distance(a, b);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				Grid.PosToXY(Vector3.Lerp(a, b, num2), out var x2, out var y2);
				GridVisibility.Reveal(x2 + 2, y2 + 2, 4, 4f);
			}
		}
	}
}
