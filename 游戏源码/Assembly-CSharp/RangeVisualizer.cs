using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	public Vector2I OriginOffset;

	public Vector2I RangeMin;

	public Vector2I RangeMax;

	public bool TestLineOfSight = true;

	public bool BlockingTileVisible;

	public Func<int, bool> BlockingVisibleCb;

	public Func<int, bool> BlockingCb = Grid.IsSolidCell;

	public bool AllowLineOfSightInvalidCells;
}
