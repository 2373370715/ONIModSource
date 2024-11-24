using System;
using UnityEngine;

// Token: 0x020007FB RID: 2043
public class DrawNavGridQuery : PathFinderQuery
{
	// Token: 0x06002484 RID: 9348 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public DrawNavGridQuery Reset(MinionBrain brain)
	{
		return this;
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x001C9C68 File Offset: 0x001C7E68
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (parent_cell == Grid.InvalidCell || (int)Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			return false;
		}
		GL.Color(Color.white);
		GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
		GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
		return false;
	}
}
