using System;
using System.Collections.Generic;
using UnityEngine;

public static class DiscreteShadowCaster
{
	public enum Octant
	{
		N_NW,
		N_NE,
		E_NE,
		E_SE,
		S_SE,
		S_SW,
		W_SW,
		W_NW
	}

	public enum Direction
	{
		North,
		East,
		South,
		West
	}

	public static Direction OctantToDirection(Octant octant)
	{
		switch (octant)
		{
		case Octant.N_NW:
		case Octant.N_NE:
			return Direction.North;
		case Octant.E_NE:
		case Octant.E_SE:
			return Direction.East;
		case Octant.S_SE:
		case Octant.S_SW:
			return Direction.South;
		case Octant.W_SW:
		case Octant.W_NW:
			return Direction.West;
		default:
			return Direction.South;
		}
	}

	public static Vector2I DirectionToVector(Direction dir)
	{
		return dir switch
		{
			Direction.North => new Vector2I(0, 1), 
			Direction.East => new Vector2I(1, 0), 
			Direction.South => new Vector2I(0, -1), 
			Direction.West => new Vector2I(-1, 0), 
			_ => default(Vector2I), 
		};
	}

	public static Vector2I TravelDirectionToOrtogonalDiractionVector(Direction dir)
	{
		switch (dir)
		{
		case Direction.North:
		case Direction.South:
			return new Vector2I(1, 0);
		case Direction.East:
		case Direction.West:
			return new Vector2I(0, 1);
		default:
			return default(Vector2I);
		}
	}

	public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, LightShape shape, bool canSeeThroughTransparent = true)
	{
		GetVisibleCells(cell, visiblePoints, range, 0, Direction.South, shape, canSeeThroughTransparent);
	}

	public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, int width, Direction direction, LightShape shape, bool canSeeThroughTransparent = true)
	{
		visiblePoints.Add(cell);
		Vector2I cellPos = Grid.CellToXY(cell);
		switch (shape)
		{
		case LightShape.Circle:
			ScanOctant(cellPos, range, 1, Octant.N_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.N_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.E_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.E_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.W_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.W_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			break;
		case LightShape.Cone:
			ScanOctant(cellPos, range, 1, Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			ScanOctant(cellPos, range, 1, Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			break;
		case LightShape.Quad:
			ScanQuad(cellPos, direction, width, range, visiblePoints, canSeeThroughTransparent);
			break;
		}
	}

	public static void ScanQuad(Vector2I cellPos, Direction direction, int width, int range, List<int> visiblePoints, bool canSeeThroughTransparent)
	{
		if (width <= 0 || range <= 0)
		{
			return;
		}
		Vector2I[] array = new Vector2I[width];
		int num = ((width % 2 == 0) ? (width / 2 - 1) : Mathf.FloorToInt((float)(width - 1) * 0.5f));
		Vector2I vector2I = DirectionToVector(direction);
		Vector2I vector2I2 = TravelDirectionToOrtogonalDiractionVector(direction);
		Vector2I vector2I3 = cellPos - vector2I2 * num;
		Vector2I vector2I4 = new Vector2I(-1, -1);
		for (int i = 0; i < width; i++)
		{
			Vector2I vector2I5 = vector2I3 + vector2I2 * i;
			bool flag = DoesOcclude(vector2I5.x, vector2I5.y, canSeeThroughTransparent);
			array[i] = (flag ? vector2I4 : vector2I5);
		}
		Vector2I[] array2 = array;
		foreach (Vector2I vector2I6 in array2)
		{
			if (vector2I6 == vector2I4)
			{
				continue;
			}
			bool flag2 = false;
			int num2 = 0;
			while (!flag2 && num2 < range)
			{
				Vector2I vector2I7 = vector2I6 + vector2I * num2;
				flag2 = flag2 || DoesOcclude(vector2I7.x, vector2I7.y, canSeeThroughTransparent);
				if (!flag2)
				{
					int item = Grid.XYToCell(vector2I7.x, vector2I7.y);
					if (!visiblePoints.Contains(item))
					{
						visiblePoints.Add(item);
					}
				}
				num2++;
			}
		}
	}

	private static bool DoesOcclude(int x, int y, bool canSeeThroughTransparent = false)
	{
		int num = Grid.XYToCell(x, y);
		if (Grid.IsValidCell(num) && (!canSeeThroughTransparent || !Grid.Transparent[num]))
		{
			return Grid.Solid[num];
		}
		return false;
	}

	private static void ScanOctant(Vector2I cellPos, int range, int depth, Octant octant, double startSlope, double endSlope, List<int> visiblePoints, bool canSeeThroughTransparent = true)
	{
		int num = range * range;
		int num2 = 0;
		int num3 = 0;
		switch (octant)
		{
		case Octant.S_SW:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope; num2++)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > num)
				{
					continue;
				}
				if (DoesOcclude(num2, num3, canSeeThroughTransparent))
				{
					if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
					{
						double slope = GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
						ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope, visiblePoints, canSeeThroughTransparent);
					}
					continue;
				}
				if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
				{
					startSlope = GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
				}
				if (!DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
				{
					visiblePoints.Add(Grid.XYToCell(num2, num3));
				}
			}
			num2--;
			break;
		case Octant.S_SE:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.WidthInCells && !DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							double slope3 = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
							ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope3, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.WidthInCells && DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = 0.0 - GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
						}
						if (!DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case Octant.E_SE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope; num3++)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > num)
				{
					continue;
				}
				if (DoesOcclude(num2, num3, canSeeThroughTransparent))
				{
					if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
					{
						ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, canSeeThroughTransparent);
					}
					continue;
				}
				if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
				{
					startSlope = 0.0 - GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
				}
				if (!DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
				{
					visiblePoints.Add(Grid.XYToCell(num2, num3));
				}
			}
			num3--;
			break;
		case Octant.E_NE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
						{
							ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
						}
						if (!DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		case Octant.N_NE:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) >= endSlope)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.HeightInCells && !DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
						{
							double slope2 = GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false);
							ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope2, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.HeightInCells && DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
						}
						if (!DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case Octant.N_NW:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: false) <= endSlope; num2++)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > num)
				{
					continue;
				}
				if (DoesOcclude(num2, num3, canSeeThroughTransparent))
				{
					if (num2 - 1 >= 0 && !DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
					{
						ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: false), visiblePoints, canSeeThroughTransparent);
					}
					continue;
				}
				if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
				{
					startSlope = 0.0 - GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: false);
				}
				if (!DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
				{
					visiblePoints.Add(Grid.XYToCell(num2, num3));
				}
			}
			num2--;
			break;
		case Octant.W_NW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) <= endSlope)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 + 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = 0.0 - GetSlope((double)num2 - 0.5, (double)num3 + 0.5, cellPos.x, cellPos.y, pInvert: true);
						}
						if (!DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		case Octant.W_SW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			for (; GetSlope(num2, num3, cellPos.x, cellPos.y, pInvert: true) >= endSlope; num3++)
			{
				if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) > num)
				{
					continue;
				}
				if (DoesOcclude(num2, num3, canSeeThroughTransparent))
				{
					if (num3 - 1 >= 0 && !DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
					{
						ScanOctant(cellPos, range, depth + 1, octant, startSlope, GetSlope((double)num2 + 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true), visiblePoints, canSeeThroughTransparent);
					}
					continue;
				}
				if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
				{
					startSlope = GetSlope((double)num2 - 0.5, (double)num3 - 0.5, cellPos.x, cellPos.y, pInvert: true);
				}
				if (!DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
				{
					visiblePoints.Add(Grid.XYToCell(num2, num3));
				}
			}
			num3--;
			break;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		else if (num2 >= Grid.WidthInCells)
		{
			num2 = Grid.WidthInCells - 1;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		else if (num3 >= Grid.HeightInCells)
		{
			num3 = Grid.HeightInCells - 1;
		}
		if ((depth < range) & !DoesOcclude(num2, num3, canSeeThroughTransparent))
		{
			ScanOctant(cellPos, range, depth + 1, octant, startSlope, endSlope, visiblePoints, canSeeThroughTransparent);
		}
	}

	private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
	{
		if (pInvert)
		{
			return (pY1 - pY2) / (pX1 - pX2);
		}
		return (pX1 - pX2) / (pY1 - pY2);
	}

	private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
	{
		return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
	}
}
