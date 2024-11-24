using System;
using UnityEngine;

// Token: 0x02000BDF RID: 3039
public static class DevToolUtil
{
	// Token: 0x06003A27 RID: 14887 RVA: 0x000C58FA File Offset: 0x000C3AFA
	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x000C590C File Offset: 0x000C3B0C
	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x000C591D File Offset: 0x000C3B1D
	public static DevPanel DebugObject<T>(T obj)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(() => obj));
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x000C5940 File Offset: 0x000C3B40
	public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(get_obj_fn));
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x000C594D File Offset: 0x000C3B4D
	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x000C5955 File Offset: 0x000C3B55
	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x000C595D File Offset: 0x000C3B5D
	public static string GenerateDevToolName(DevTool devTool)
	{
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x00226718 File Offset: 0x00224918
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

	// Token: 0x06003A2F RID: 14895 RVA: 0x00226788 File Offset: 0x00224988
	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		int num;
		return DevToolUtil.TryGetCellIndexFor(gameObject, out num);
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x002267A0 File Offset: 0x002249A0
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

	// Token: 0x06003A31 RID: 14897 RVA: 0x002267EC File Offset: 0x002249EC
	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x000C596A File Offset: 0x000C3B6A
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

	// Token: 0x06003A33 RID: 14899 RVA: 0x0022680C File Offset: 0x00224A0C
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

	// Token: 0x06003A34 RID: 14900 RVA: 0x0022685C File Offset: 0x00224A5C
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

	// Token: 0x02000BE0 RID: 3040
	public enum TextAlignment
	{
		// Token: 0x040027AB RID: 10155
		Center,
		// Token: 0x040027AC RID: 10156
		Left,
		// Token: 0x040027AD RID: 10157
		Right
	}
}
