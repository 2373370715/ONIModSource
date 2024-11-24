using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200177F RID: 6015
public static class LightGridManager
{
	// Token: 0x06007BD0 RID: 31696 RVA: 0x0031D410 File Offset: 0x0031B610
	public static int ComputeFalloff(float fallOffRate, int cell, int originCell, global::LightShape lightShape, DiscreteShadowCaster.Direction lightDirection)
	{
		int num = originCell;
		if (lightShape == global::LightShape.Quad)
		{
			Vector2I vector2I = Grid.CellToXY(num);
			Vector2I vector2I2 = Grid.CellToXY(cell);
			switch (lightDirection)
			{
			case DiscreteShadowCaster.Direction.North:
			case DiscreteShadowCaster.Direction.South:
			{
				Vector2I vector2I3 = new Vector2I(vector2I2.x, vector2I.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			case DiscreteShadowCaster.Direction.East:
			case DiscreteShadowCaster.Direction.West:
			{
				Vector2I vector2I3 = new Vector2I(vector2I.x, vector2I2.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			}
		}
		return LightGridManager.CalculateFalloff(fallOffRate, cell, num);
	}

	// Token: 0x06007BD1 RID: 31697 RVA: 0x000F18D5 File Offset: 0x000EFAD5
	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06007BD2 RID: 31698 RVA: 0x000F18F2 File Offset: 0x000EFAF2
	public static void Initialise()
	{
		LightGridManager.previewLux = new int[Grid.CellCount];
	}

	// Token: 0x06007BD3 RID: 31699 RVA: 0x000F1903 File Offset: 0x000EFB03
	public static void Shutdown()
	{
		LightGridManager.previewLux = null;
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06007BD4 RID: 31700 RVA: 0x0031D4A0 File Offset: 0x0031B6A0
	public static void DestroyPreview()
	{
		foreach (global::Tuple<int, int> tuple in LightGridManager.previewLightCells)
		{
			LightGridManager.previewLux[tuple.first] = 0;
		}
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06007BD5 RID: 31701 RVA: 0x000F1915 File Offset: 0x000EFB15
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux)
	{
		LightGridManager.CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
	}

	// Token: 0x06007BD6 RID: 31702 RVA: 0x0031D504 File Offset: 0x0031B704
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
	{
		LightGridManager.previewLightCells.Clear();
		ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape, true);
		foreach (int num in pooledList)
		{
			if (Grid.IsValidCell(num))
			{
				int num2 = lux / LightGridManager.ComputeFalloff(0.5f, num, origin_cell, shape, direction);
				LightGridManager.previewLightCells.Add(new global::Tuple<int, int>(num, num2));
				LightGridManager.previewLux[num] = num2;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x04005CD8 RID: 23768
	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	// Token: 0x04005CD9 RID: 23769
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04005CDA RID: 23770
	public static int[] previewLux;

	// Token: 0x02001780 RID: 6016
	public class LightGridEmitter
	{
		// Token: 0x06007BD8 RID: 31704 RVA: 0x0031D5AC File Offset: 0x0031B7AC
		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int)this.state.radius, this.state.width, this.state.direction, this.state.shape, true);
		}

		// Token: 0x06007BD9 RID: 31705 RVA: 0x0031D600 File Offset: 0x0031B800
		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter", null);
			if (update_lit_cells)
			{
				this.UpdateLitCells();
			}
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					int num2 = Mathf.Max(0, Grid.LightCount[num] + this.ComputeLux(num));
					Grid.LightCount[num] = num2;
					LightGridManager.previewLux[num] = num2;
				}
			}
		}

		// Token: 0x06007BDA RID: 31706 RVA: 0x0031D6A4 File Offset: 0x0031B8A4
		public void RemoveFromGrid()
		{
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					Grid.LightCount[num] = Mathf.Max(0, Grid.LightCount[num] - this.ComputeLux(num));
					LightGridManager.previewLux[num] = 0;
				}
			}
			this.litCells.Clear();
		}

		// Token: 0x06007BDB RID: 31707 RVA: 0x000F192E File Offset: 0x000EFB2E
		public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
		{
			if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
			{
				return false;
			}
			this.RemoveFromGrid();
			this.state = state;
			this.AddToGrid(true);
			return true;
		}

		// Token: 0x06007BDC RID: 31708 RVA: 0x000F195D File Offset: 0x000EFB5D
		private int ComputeLux(int cell)
		{
			return this.state.intensity / this.ComputeFalloff(cell);
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x000F1972 File Offset: 0x000EFB72
		private int ComputeFalloff(int cell)
		{
			return LightGridManager.ComputeFalloff(this.state.falloffRate, cell, this.state.origin, this.state.shape, this.state.direction);
		}

		// Token: 0x04005CDB RID: 23771
		private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;

		// Token: 0x04005CDC RID: 23772
		private List<int> litCells = new List<int>();

		// Token: 0x02001781 RID: 6017
		[Serializable]
		public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
		{
			// Token: 0x06007BDF RID: 31711 RVA: 0x0031D728 File Offset: 0x0031B928
			public bool Equals(LightGridManager.LightGridEmitter.State rhs)
			{
				return this.origin == rhs.origin && this.shape == rhs.shape && this.radius == rhs.radius && this.intensity == rhs.intensity && this.falloffRate == rhs.falloffRate && this.colour == rhs.colour && this.width == rhs.width && this.direction == rhs.direction;
			}

			// Token: 0x04005CDD RID: 23773
			public int origin;

			// Token: 0x04005CDE RID: 23774
			public global::LightShape shape;

			// Token: 0x04005CDF RID: 23775
			public int width;

			// Token: 0x04005CE0 RID: 23776
			public DiscreteShadowCaster.Direction direction;

			// Token: 0x04005CE1 RID: 23777
			public float radius;

			// Token: 0x04005CE2 RID: 23778
			public int intensity;

			// Token: 0x04005CE3 RID: 23779
			public float falloffRate;

			// Token: 0x04005CE4 RID: 23780
			public Color colour;

			// Token: 0x04005CE5 RID: 23781
			public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State
			{
				origin = Grid.InvalidCell,
				shape = global::LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white,
				direction = DiscreteShadowCaster.Direction.South,
				width = 4
			};
		}
	}
}
