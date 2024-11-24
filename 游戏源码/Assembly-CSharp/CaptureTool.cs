using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200141E RID: 5150
public class CaptureTool : DragTool
{
	// Token: 0x06006A4F RID: 27215 RVA: 0x002DE7C8 File Offset: 0x002DC9C8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x002DE810 File Offset: 0x002DCA10
	public static void MarkForCapture(Vector2 min, Vector2 max, bool mark)
	{
		foreach (Capturable capturable in Components.Capturables.Items)
		{
			Vector2 vector = Grid.PosToXY(capturable.transform.GetPosition());
			if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
			{
				if (capturable.allowCapture)
				{
					PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
					capturable.MarkForCapture(mark, lastSelectedPriority, true);
				}
				else if (mark)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.TOOLS.CAPTURE.NOT_CAPTURABLE, null, capturable.transform.GetPosition(), 1.5f, false, false);
				}
			}
		}
	}

	// Token: 0x06006A51 RID: 27217 RVA: 0x000E57E0 File Offset: 0x000E39E0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006A52 RID: 27218 RVA: 0x000E57F8 File Offset: 0x000E39F8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
