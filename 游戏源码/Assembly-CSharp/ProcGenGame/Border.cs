using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace ProcGenGame
{
	// Token: 0x0200209B RID: 8347
	public class Border : Path
	{
		// Token: 0x0600B154 RID: 45396 RVA: 0x0042BE50 File Offset: 0x0042A050
		public Border(Neighbors neighbors, Vector2 e0, Vector2 e1)
		{
			this.neighbors = neighbors;
			Vector2 vector = e1 - e0;
			Vector2 normalized = new Vector2(-vector.y, vector.x).normalized;
			Vector2 point = e0 + vector / 2f + normalized;
			if (neighbors.n0.poly.Contains(point))
			{
				base.AddSegment(e0, e1);
				return;
			}
			base.AddSegment(e1, e0);
		}

		// Token: 0x0600B155 RID: 45397 RVA: 0x0042BECC File Offset: 0x0042A0CC
		private void SetCell(int gridCell, float defaultTemperature, TerrainCell.SetValuesFunction SetValues, SeededRandom rnd)
		{
			WeightedSimHash weightedSimHash = WeightedRandom.Choose<WeightedSimHash>(this.element, rnd);
			TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(weightedSimHash.element, weightedSimHash.overrides);
			if (!elementOverride.overrideTemperature)
			{
				elementOverride.pdelement.temperature = defaultTemperature;
			}
			SetValues(gridCell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
		}

		// Token: 0x0600B156 RID: 45398 RVA: 0x0042BF28 File Offset: 0x0042A128
		public void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float neighbour0Temperature, float neighbour1Temperature, float midTemp, SeededRandom rnd, int snapLastCells)
		{
			for (int i = 0; i < this.pathElements.Count; i++)
			{
				Vector2 vector = this.pathElements[i].e1 - this.pathElements[i].e0;
				Vector2 normalized = new Vector2(-vector.y, vector.x).normalized;
				List<Vector2I> line = ProcGen.Util.GetLine(this.pathElements[i].e0, this.pathElements[i].e1);
				for (int j = 0; j < line.Count; j++)
				{
					int num = Grid.XYToCell(line[j].x, line[j].y);
					if (Grid.IsValidCell(num))
					{
						this.SetCell(num, midTemp, SetValues, rnd);
					}
					for (float num2 = 0.5f; num2 <= this.width; num2 += 1f)
					{
						float num3 = Mathf.Clamp01((num2 - 0.5f) / (this.width - 0.5f));
						if (num2 + (float)snapLastCells > this.width)
						{
							num3 = 1f;
						}
						Vector2 vector2 = line[j] + normalized * num2;
						float defaultTemperature = midTemp + (neighbour0Temperature - midTemp) * num3;
						num = Grid.XYToCell((int)vector2.x, (int)vector2.y);
						if (Grid.IsValidCell(num))
						{
							this.SetCell(num, defaultTemperature, SetValues, rnd);
						}
						Vector2 vector3 = line[j] - normalized * num2;
						float defaultTemperature2 = midTemp + (neighbour1Temperature - midTemp) * num3;
						num = Grid.XYToCell((int)vector3.x, (int)vector3.y);
						if (Grid.IsValidCell(num))
						{
							this.SetCell(num, defaultTemperature2, SetValues, rnd);
						}
					}
				}
			}
		}

		// Token: 0x04008C2C RID: 35884
		public Neighbors neighbors;

		// Token: 0x04008C2D RID: 35885
		public List<WeightedSimHash> element;

		// Token: 0x04008C2E RID: 35886
		public float width;
	}
}
