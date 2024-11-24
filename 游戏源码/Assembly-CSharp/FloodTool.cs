using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001430 RID: 5168
public class FloodTool : InterfaceTool
{
	// Token: 0x06006AE2 RID: 27362 RVA: 0x002E0B1C File Offset: 0x002DED1C
	public HashSet<int> Flood(int startCell)
	{
		HashSet<int> visited_cells = new HashSet<int>();
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.FloodFillConditional(startCell, this.floodCriteria, visited_cells, hashSet);
		return hashSet;
	}

	// Token: 0x06006AE3 RID: 27363 RVA: 0x000E6332 File Offset: 0x000E4532
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x000E6352 File Offset: 0x000E4552
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.mouseCell = Grid.PosToCell(cursor_pos);
	}

	// Token: 0x04005082 RID: 20610
	public Func<int, bool> floodCriteria;

	// Token: 0x04005083 RID: 20611
	public Action<HashSet<int>> paintArea;

	// Token: 0x04005084 RID: 20612
	protected Color32 areaColour = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04005085 RID: 20613
	protected int mouseCell = -1;
}
