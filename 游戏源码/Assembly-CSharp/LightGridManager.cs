using System;
using System.Collections.Generic;
using UnityEngine;

public static class LightGridManager
{
	public class LightGridEmitter
	{
		[Serializable]
		public struct State : IEquatable<State>
		{
			public int origin;

			public LightShape shape;

			public int width;

			public DiscreteShadowCaster.Direction direction;

			public float radius;

			public int intensity;

			public float falloffRate;

			public Color colour;

			public static readonly State DEFAULT = new State
			{
				origin = Grid.InvalidCell,
				shape = LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white,
				direction = DiscreteShadowCaster.Direction.South,
				width = 4
			};

			public bool Equals(State rhs)
			{
				if (origin == rhs.origin && shape == rhs.shape && radius == rhs.radius && intensity == rhs.intensity && falloffRate == rhs.falloffRate && colour == rhs.colour && width == rhs.width)
				{
					return direction == rhs.direction;
				}
				return false;
			}
		}

		private State state = State.DEFAULT;

		private List<int> litCells = new List<int>();

		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(state.origin, litCells, (int)state.radius, state.width, state.direction, state.shape);
		}

		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || litCells.Count == 0, "adding an already added emitter");
			if (update_lit_cells)
			{
				UpdateLitCells();
			}
			foreach (int litCell in litCells)
			{
				if (Grid.IsValidCell(litCell))
				{
					int num = Mathf.Max(0, Grid.LightCount[litCell] + ComputeLux(litCell));
					Grid.LightCount[litCell] = num;
					previewLux[litCell] = num;
				}
			}
		}

		public void RemoveFromGrid()
		{
			foreach (int litCell in litCells)
			{
				if (Grid.IsValidCell(litCell))
				{
					Grid.LightCount[litCell] = Mathf.Max(0, Grid.LightCount[litCell] - ComputeLux(litCell));
					previewLux[litCell] = 0;
				}
			}
			litCells.Clear();
		}

		public bool Refresh(State state, bool force = false)
		{
			if (!force && EqualityComparer<State>.Default.Equals(this.state, state))
			{
				return false;
			}
			RemoveFromGrid();
			this.state = state;
			AddToGrid(update_lit_cells: true);
			return true;
		}

		private int ComputeLux(int cell)
		{
			return state.intensity / ComputeFalloff(cell);
		}

		private int ComputeFalloff(int cell)
		{
			return LightGridManager.ComputeFalloff(state.falloffRate, cell, state.origin, state.shape, state.direction);
		}
	}

	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	public static List<Tuple<int, int>> previewLightCells = new List<Tuple<int, int>>();

	public static int[] previewLux;

	public static int ComputeFalloff(float fallOffRate, int cell, int originCell, LightShape lightShape, DiscreteShadowCaster.Direction lightDirection)
	{
		int num = originCell;
		if (lightShape == LightShape.Quad)
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
		return CalculateFalloff(fallOffRate, cell, num);
	}

	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	public static void Initialise()
	{
		previewLux = new int[Grid.CellCount];
	}

	public static void Shutdown()
	{
		previewLux = null;
		previewLightCells.Clear();
	}

	public static void DestroyPreview()
	{
		foreach (Tuple<int, int> previewLightCell in previewLightCells)
		{
			previewLux[previewLightCell.first] = 0;
		}
		previewLightCells.Clear();
	}

	public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux)
	{
		CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
	}

	public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
	{
		previewLightCells.Clear();
		ListPool<int, LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape);
		int num = 0;
		foreach (int item in pooledList)
		{
			if (Grid.IsValidCell(item))
			{
				num = lux / ComputeFalloff(0.5f, item, origin_cell, shape, direction);
				previewLightCells.Add(new Tuple<int, int>(item, num));
				previewLux[item] = num;
			}
		}
		pooledList.Recycle();
	}
}
