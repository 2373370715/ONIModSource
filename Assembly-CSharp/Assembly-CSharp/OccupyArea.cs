using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour {
    public  CellOffset[] _RotatedOccupiedCellsOffsets;
    public  CellOffset[] _UnrotatedOccupiedCellsOffsets;
    private CellOffset[] AboveOccupiedCellOffsets;
    private Orientation  appliedOrientation;

    [SerializeField]
    private bool applyToCells = true;

    private CellOffset[]  BelowOccupiedCellOffsets;
    public  ObjectLayer[] objectLayers = new ObjectLayer[0];
    private int[]         occupiedGridCells;

    [MyCmpGet]
    private Rotatable rotatable;

    public CellOffset[] OccupiedCellsOffsets {
        get {
            UpdateRotatedCells();
            return _RotatedOccupiedCellsOffsets;
        }
    }

    public bool ApplyToCells {
        get => applyToCells;
        set {
            if (value != applyToCells) {
                if (value)
                    UpdateOccupiedArea();
                else
                    ClearOccupiedArea();

                applyToCells = value;
            }
        }
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (applyToCells) UpdateOccupiedArea();
    }

    private void ValidatePosition() {
        if (!Grid.IsValidCell(Grid.PosToCell(this))) {
            Debug.LogWarning(name + " is outside the grid! DELETING!");
            Util.KDestroyGameObject(gameObject);
        }
    }

    [OnSerializing]
    private void OnSerializing() { ValidatePosition(); }

    [OnDeserialized]
    private void OnDeserialized() { ValidatePosition(); }

    public int GetOffsetCellWithRotation(CellOffset cellOffset) {
        var offset                    = cellOffset;
        if (rotatable != null) offset = rotatable.GetRotatedCellOffset(cellOffset);
        return Grid.OffsetCell(Grid.PosToCell(gameObject), offset);
    }

    public void SetCellOffsets(CellOffset[] cells) {
        _UnrotatedOccupiedCellsOffsets = cells;
        _RotatedOccupiedCellsOffsets   = cells;
        UpdateRotatedCells();
    }

    private void UpdateRotatedCells() {
        if (rotatable != null && appliedOrientation != rotatable.Orientation) {
            _RotatedOccupiedCellsOffsets = new CellOffset[_UnrotatedOccupiedCellsOffsets.Length];
            for (var i = 0; i < _UnrotatedOccupiedCellsOffsets.Length; i++) {
                var offset = _UnrotatedOccupiedCellsOffsets[i];
                _RotatedOccupiedCellsOffsets[i] = rotatable.GetRotatedCellOffset(offset);
            }

            appliedOrientation = rotatable.Orientation;
        }
    }

    public bool CheckIsOccupying(int checkCell) {
        var num = Grid.PosToCell(gameObject);
        if (checkCell == num) return true;

        foreach (var offset in OccupiedCellsOffsets)
            if (Grid.OffsetCell(num, offset) == checkCell)
                return true;

        return false;
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        ClearOccupiedArea();
    }

    private void ClearOccupiedArea() {
        if (occupiedGridCells == null) return;

        foreach (var objectLayer in objectLayers)
            if (objectLayer != ObjectLayer.NumLayers)
                foreach (var cell in occupiedGridCells)
                    if (Grid.Objects[cell, (int)objectLayer] == gameObject)
                        Grid.Objects[cell, (int)objectLayer] = null;
    }

    public void UpdateOccupiedArea() {
        if (objectLayers.Length == 0) return;

        if (occupiedGridCells == null) occupiedGridCells = new int[OccupiedCellsOffsets.Length];
        ClearOccupiedArea();
        var cell = Grid.PosToCell(gameObject);
        foreach (var objectLayer in objectLayers)
            if (objectLayer != ObjectLayer.NumLayers)
                for (var j = 0; j < OccupiedCellsOffsets.Length; j++) {
                    var offset = OccupiedCellsOffsets[j];
                    var num    = Grid.OffsetCell(cell, offset);
                    Grid.Objects[num, (int)objectLayer] = gameObject;
                    occupiedGridCells[j]                = num;
                }
    }

    public int GetWidthInCells() {
        var num  = int.MaxValue;
        var num2 = int.MinValue;
        foreach (var cellOffset in OccupiedCellsOffsets) {
            num  = Math.Min(num, cellOffset.x);
            num2 = Math.Max(num2, cellOffset.x);
        }

        return num2 - num + 1;
    }

    public int GetHeightInCells() {
        var num  = int.MaxValue;
        var num2 = int.MinValue;
        foreach (var cellOffset in OccupiedCellsOffsets) {
            num  = Math.Min(num, cellOffset.y);
            num2 = Math.Max(num2, cellOffset.y);
        }

        return num2 - num + 1;
    }

    public Extents GetExtents() { return new Extents(Grid.PosToCell(gameObject), OccupiedCellsOffsets); }

    public Extents GetExtents(Orientation orientation) {
        return new Extents(Grid.PosToCell(gameObject), OccupiedCellsOffsets, orientation);
    }

    private void OnDrawGizmosSelected() {
        var cell = Grid.PosToCell(gameObject);
        if (OccupiedCellsOffsets != null)
            foreach (var offset in OccupiedCellsOffsets) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) +
                                    Vector3.right / 2f                            +
                                    Vector3.up    / 2f,
                                    Vector3.one);
            }

        if (AboveOccupiedCellOffsets != null)
            foreach (var offset2 in AboveOccupiedCellOffsets) {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) +
                                    Vector3.right / 2f                             +
                                    Vector3.up    / 2f,
                                    Vector3.one * 0.9f);
            }

        if (BelowOccupiedCellOffsets != null)
            foreach (var offset3 in BelowOccupiedCellOffsets) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) +
                                    Vector3.right / 2f                             +
                                    Vector3.up    / 2f,
                                    Vector3.one * 0.9f);
            }
    }

    public bool CanOccupyArea(int rootCell, ObjectLayer layer) {
        for (var i = 0; i < OccupiedCellsOffsets.Length; i++) {
            var offset = OccupiedCellsOffsets[i];
            var cell   = Grid.OffsetCell(rootCell, offset);
            if (Grid.Objects[cell, (int)layer] != null) return false;
        }

        return true;
    }

    public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate) {
        for (var i = 0; i < OccupiedCellsOffsets.Length; i++) {
            var offset = OccupiedCellsOffsets[i];
            var arg    = Grid.OffsetCell(rootCell, offset);
            if (!testDelegate(arg, data)) return false;
        }

        return true;
    }

    public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate) {
        if (AboveOccupiedCellOffsets == null) {
            var list = new List<CellOffset>();
            for (var i = 0; i < OccupiedCellsOffsets.Length; i++) {
                var cellOffset = new CellOffset(OccupiedCellsOffsets[i].x, OccupiedCellsOffsets[i].y + 1);
                if (Array.IndexOf(OccupiedCellsOffsets, cellOffset) == -1) list.Add(cellOffset);
            }

            AboveOccupiedCellOffsets = list.ToArray();
        }

        for (var j = 0; j < AboveOccupiedCellOffsets.Length; j++) {
            var arg = Grid.OffsetCell(rootCell, AboveOccupiedCellOffsets[j]);
            if (!testDelegate(arg, data)) return false;
        }

        return true;
    }

    public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate) {
        if (BelowOccupiedCellOffsets == null) {
            var list = new List<CellOffset>();
            for (var i = 0; i < OccupiedCellsOffsets.Length; i++) {
                var cellOffset = new CellOffset(OccupiedCellsOffsets[i].x, OccupiedCellsOffsets[i].y - 1);
                if (Array.IndexOf(OccupiedCellsOffsets, cellOffset) == -1) list.Add(cellOffset);
            }

            BelowOccupiedCellOffsets = list.ToArray();
        }

        for (var j = 0; j < BelowOccupiedCellOffsets.Length; j++) {
            var arg = Grid.OffsetCell(rootCell, BelowOccupiedCellOffsets[j]);
            if (!testDelegate(arg, data)) return false;
        }

        return true;
    }
}