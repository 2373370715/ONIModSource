using System;
using UnityEngine;

public static class DevToolUtil
{
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
		return DevToolUtil.Open(new DevToolObjectViewer<T>(() => obj));
	}

		public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(get_obj_fn));
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
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

		public static string GenerateDevToolName(Type devToolType)
	{
		string result;
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out result))
		{
			return result;
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
		int num;
		return DevToolUtil.TryGetCellIndexFor(gameObject, out num);
	}

		public static void RevealAndFocus(GameObject gameObject)
	{
		int cellIndex;
		if (DevToolUtil.TryGetCellIndexFor(gameObject, out cellIndex))
		{
			return;
		}
		DevToolUtil.RevealAndFocusAt(cellIndex);
		if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			return;
		}
		SelectTool.Instance.Select(null, false);
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
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(true);
		if (array == null)
		{
			return false;
		}
		foreach (BuildingComplete buildingComplete in array)
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
		int num;
		int num2;
		Grid.CellToXY(cellIndex, out num, out num2);
		GridVisibility.Reveal(num + 2, num2 + 2, 10, 10f);
		DevToolUtil.FocusCameraOnCell(cellIndex);
		int cell;
		if (DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cell))
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(cell);
			float num3 = 2f / Vector3.Distance(a, b);
			for (float num4 = 0f; num4 < 1f; num4 += num3)
			{
				int num5;
				int num6;
				Grid.PosToXY(Vector3.Lerp(a, b, num4), out num5, out num6);
				GridVisibility.Reveal(num5 + 2, num6 + 2, 4, 4f);
			}
		}
	}

		public enum TextAlignment
	{
				Center,
				Left,
				Right
	}
}
