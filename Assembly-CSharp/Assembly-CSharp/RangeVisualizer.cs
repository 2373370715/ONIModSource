using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour {
    public bool            AllowLineOfSightInvalidCells;
    public Func<int, bool> BlockingCb = Grid.IsSolidCell;
    public bool            BlockingTileVisible;
    public Func<int, bool> BlockingVisibleCb;
    public Vector2I        OriginOffset;
    public Vector2I        RangeMax;
    public Vector2I        RangeMin;
    public bool            TestLineOfSight = true;
}