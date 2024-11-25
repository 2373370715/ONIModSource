using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour {
    private KAnimSynchronizedController bottom;
    public  string                      bottomName = "#cap_bottom";
    private Extents                     extents;
    private KAnimSynchronizedController left;
    public  string                      leftName    = "#cap_left";
    public  ObjectLayer                 objectLayer = ObjectLayer.Building;
    private HandleVector<int>.Handle    partitionerEntry;
    private KAnimSynchronizedController right;
    public  string                      rightName = "#cap_right";
    public  Tag[]                       tags;
    private KAnimSynchronizedController top;
    public  string                      topName = "#cap_top";

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        if (tags == null || tags.Length == 0) tags = new[] { GetComponent<KPrefabID>().PrefabTag };
    }

    protected override void OnSpawn() {
        var component = GetComponent<OccupyArea>();
        if (component != null)
            this.extents = component.GetExtents();
        else {
            var component2 = GetComponent<Building>();
            this.extents = component2.GetExtents();
        }

        var extents = new Extents(this.extents.x      - 1,
                                  this.extents.y      - 1,
                                  this.extents.width  + 2,
                                  this.extents.height + 2);

        partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn",
                                                             gameObject,
                                                             extents,
                                                             GameScenePartitioner.Instance.objectLayers
                                                                 [(int)objectLayer],
                                                             OnNeighbourCellsUpdated);

        var component3 = GetComponent<KBatchedAnimController>();
        left   = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), leftName);
        right  = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), rightName);
        top    = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), topName);
        bottom = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), bottomName);
        UpdateEndCaps();
    }

    protected override void OnCleanUp() {
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        base.OnCleanUp();
    }

    private void UpdateEndCaps() {
        var cell    = Grid.PosToCell(this);
        var enable  = true;
        var enable2 = true;
        var enable3 = true;
        var enable4 = true;
        int num;
        int num2;
        Grid.CellToXY(cell, out num, out num2);
        var rotatedCellOffset  = new CellOffset(extents.x       - num - 1,       0);
        var rotatedCellOffset2 = new CellOffset(extents.x - num + extents.width, 0);
        var rotatedCellOffset3 = new CellOffset(0,                               extents.y - num2 + extents.height);
        var rotatedCellOffset4 = new CellOffset(0,                               extents.y        - num2 - 1);
        var component          = GetComponent<Rotatable>();
        if (component) {
            rotatedCellOffset  = component.GetRotatedCellOffset(rotatedCellOffset);
            rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
            rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
            rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
        }

        var num3                            = Grid.OffsetCell(cell, rotatedCellOffset);
        var num4                            = Grid.OffsetCell(cell, rotatedCellOffset2);
        var num5                            = Grid.OffsetCell(cell, rotatedCellOffset3);
        var num6                            = Grid.OffsetCell(cell, rotatedCellOffset4);
        if (Grid.IsValidCell(num3)) enable  = !HasTileableNeighbour(num3);
        if (Grid.IsValidCell(num4)) enable2 = !HasTileableNeighbour(num4);
        if (Grid.IsValidCell(num5)) enable3 = !HasTileableNeighbour(num5);
        if (Grid.IsValidCell(num6)) enable4 = !HasTileableNeighbour(num6);
        left.Enable(enable);
        right.Enable(enable2);
        top.Enable(enable3);
        bottom.Enable(enable4);
    }

    private bool HasTileableNeighbour(int neighbour_cell) {
        var result     = false;
        var gameObject = Grid.Objects[neighbour_cell, (int)objectLayer];
        if (gameObject != null) {
            var component                                               = gameObject.GetComponent<KPrefabID>();
            if (component != null && component.HasAnyTags(tags)) result = true;
        }

        return result;
    }

    private void OnNeighbourCellsUpdated(object data) {
        if (this == null || gameObject == null) return;

        if (partitionerEntry.IsValid()) UpdateEndCaps();
    }
}