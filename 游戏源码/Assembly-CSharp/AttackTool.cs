using System;
using UnityEngine;

// Token: 0x02001414 RID: 5140
public class AttackTool : DragTool
{
	// Token: 0x060069E6 RID: 27110 RVA: 0x002DC2C8 File Offset: 0x002DA4C8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x060069E7 RID: 27111 RVA: 0x002DC310 File Offset: 0x002DA510
	public static void MarkForAttack(Vector2 min, Vector2 max, bool mark)
	{
		foreach (FactionAlignment factionAlignment in Components.FactionAlignments.Items)
		{
			if (!factionAlignment.IsNullOrDestroyed())
			{
				Vector2 vector = Grid.PosToXY(factionAlignment.transform.GetPosition());
				if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
				{
					if (mark)
					{
						if (FactionManager.Instance.GetDisposition(FactionManager.FactionID.Duplicant, factionAlignment.Alignment) != FactionManager.Disposition.Assist)
						{
							factionAlignment.SetPlayerTargeted(true);
							Prioritizable component = factionAlignment.GetComponent<Prioritizable>();
							if (component != null)
							{
								component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
					else
					{
						factionAlignment.gameObject.Trigger(2127324410, null);
					}
				}
			}
		}
	}

	// Token: 0x060069E8 RID: 27112 RVA: 0x000E57E0 File Offset: 0x000E39E0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x060069E9 RID: 27113 RVA: 0x000E57F8 File Offset: 0x000E39F8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
