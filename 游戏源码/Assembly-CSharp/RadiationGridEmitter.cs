using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200178D RID: 6029
[Serializable]
public class RadiationGridEmitter
{
	// Token: 0x06007C07 RID: 31751 RVA: 0x0031EC38 File Offset: 0x0031CE38
	public RadiationGridEmitter(int originCell, int intensity)
	{
		this.originCell = originCell;
		this.intensity = intensity;
	}

	// Token: 0x06007C08 RID: 31752 RVA: 0x0031EC88 File Offset: 0x0031CE88
	public void Emit()
	{
		this.scanCells.Clear();
		Vector2 a = Grid.CellToPosCCC(this.originCell, Grid.SceneLayer.Building);
		for (float num = (float)this.direction - (float)this.angle / 2f; num < (float)this.direction + (float)this.angle / 2f; num += (float)(this.angle / this.projectionCount))
		{
			float num2 = UnityEngine.Random.Range((float)(-(float)this.angle / this.projectionCount) / 2f, (float)(this.angle / this.projectionCount) / 2f);
			Vector2 vector = new Vector2(Mathf.Cos((num + num2) * 3.1415927f / 180f), Mathf.Sin((num + num2) * 3.1415927f / 180f));
			int num3 = 3;
			float num4 = (float)(this.intensity / 4);
			Vector2 a2 = vector;
			float num5 = 0f;
			while ((double)num4 > 0.01 && num5 < (float)RadiationGridEmitter.MAX_EMIT_DISTANCE)
			{
				num5 += 1f / (float)num3;
				int num6 = Grid.PosToCell(a + a2 * num5);
				if (!Grid.IsValidCell(num6))
				{
					break;
				}
				if (!this.scanCells.Contains(num6))
				{
					SimMessages.ModifyRadiationOnCell(num6, (float)Mathf.RoundToInt(num4), -1);
					this.scanCells.Add(num6);
				}
				num4 *= Mathf.Max(0f, 1f - Mathf.Pow(Grid.Mass[num6], 1.25f) * Grid.Element[num6].molarMass / 1000000f);
				num4 *= UnityEngine.Random.Range(0.96f, 0.98f);
			}
		}
	}

	// Token: 0x06007C09 RID: 31753 RVA: 0x000F1B2B File Offset: 0x000EFD2B
	private int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x04005DBF RID: 23999
	private static int MAX_EMIT_DISTANCE = 128;

	// Token: 0x04005DC0 RID: 24000
	public int originCell = -1;

	// Token: 0x04005DC1 RID: 24001
	public int intensity = 1;

	// Token: 0x04005DC2 RID: 24002
	public int projectionCount = 20;

	// Token: 0x04005DC3 RID: 24003
	public int direction;

	// Token: 0x04005DC4 RID: 24004
	public int angle = 360;

	// Token: 0x04005DC5 RID: 24005
	public bool enabled;

	// Token: 0x04005DC6 RID: 24006
	private HashSet<int> scanCells = new HashSet<int>();
}
