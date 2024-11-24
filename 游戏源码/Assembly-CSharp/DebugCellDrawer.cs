using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001208 RID: 4616
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
	// Token: 0x06005E3A RID: 24122 RVA: 0x002A1DB8 File Offset: 0x0029FFB8
	private void Update()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i] != PathFinder.InvalidCell)
			{
				DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[i], Grid.SceneLayer.Background), 1f, 0f, true);
			}
		}
	}

	// Token: 0x040042B3 RID: 17075
	public List<int> cells;
}
