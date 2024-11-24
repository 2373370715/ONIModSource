using System;
using UnityEngine;

// Token: 0x02001A30 RID: 6704
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
	// Token: 0x06008BD1 RID: 35793 RVA: 0x000FB66C File Offset: 0x000F986C
	public static void DestroyInstance()
	{
		VisibilityTester.Instance = null;
	}

	// Token: 0x06008BD2 RID: 35794 RVA: 0x000FB674 File Offset: 0x000F9874
	protected override void OnPrefabInit()
	{
		VisibilityTester.Instance = this;
	}

	// Token: 0x06008BD3 RID: 35795 RVA: 0x00360E38 File Offset: 0x0035F038
	private void Update()
	{
		if (SelectTool.Instance == null || SelectTool.Instance.selected == null || !this.enableTesting)
		{
			return;
		}
		int cell = Grid.PosToCell(SelectTool.Instance.selected);
		int mouseCell = DebugHandler.GetMouseCell();
		string text = "";
		text = text + "Source Cell: " + cell.ToString() + "\n";
		text = text + "Target Cell: " + mouseCell.ToString() + "\n";
		text = text + "Visible: " + Grid.VisibilityTest(cell, mouseCell, false).ToString();
		for (int i = 0; i < 10000; i++)
		{
			Grid.VisibilityTest(cell, mouseCell, false);
		}
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x04006935 RID: 26933
	public static VisibilityTester Instance;

	// Token: 0x04006936 RID: 26934
	public bool enableTesting;
}
