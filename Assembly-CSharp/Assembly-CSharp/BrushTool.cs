using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : InterfaceTool {
    protected bool affectFoundation;

    [SerializeField]
    private readonly Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

    [SerializeField]
    private GameObject areaVisualizer;

    [SerializeField]
    private Texture2D brushCursor;

    protected         List<Vector2> brushOffsets  = new List<Vector2>();
    protected         int           brushRadius   = -1;
    protected         HashSet<int>  cellsInRadius = new HashSet<int>();
    protected         int           currentCell;
    protected         Vector3       downPos;
    private           DragAxis      dragAxis = DragAxis.Invalid;
    protected         bool          interceptNumberKeysForPriority;
    protected         int           lastCell;
    protected         Vector3       placementPivot;
    protected         Color         radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
    protected         List<int>     visitedCells         = new List<int>();
    public            bool          Dragging            { get; private set; }
    protected virtual void          PlaySound()         { }
    protected virtual void          clearVisitedCells() { visitedCells.Clear(); }

    protected override void OnActivateTool() {
        base.OnActivateTool();
        Dragging = false;
    }

    public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors) {
        colors = new HashSet<ToolMenu.CellColorData>();
        foreach (var cell in cellsInRadius) colors.Add(new ToolMenu.CellColorData(cell, radiusIndicatorColor));
    }

    public virtual void SetBrushSize(int radius) {
        if (radius == brushRadius) return;

        brushRadius = radius;
        brushOffsets.Clear();
        for (var i = 0; i < brushRadius * 2; i++) {
            for (var j = 0; j < brushRadius * 2; j++)
                if (Vector2.Distance(new Vector2(i, j), new Vector2(brushRadius, brushRadius)) < brushRadius - 0.8f)
                    brushOffsets.Add(new Vector2(i - brushRadius, j - brushRadius));
        }
    }

    protected override void OnDeactivateTool(InterfaceTool new_tool) {
        KScreenManager.Instance.SetEventSystemEnabled(true);
        if (KInputManager.currentControllerIsGamepad) SetCurrentVirtualInputModuleMousMovementMode(false);
        base.OnDeactivateTool(new_tool);
    }

    protected override void OnPrefabInit() {
        Game.Instance.Subscribe(1634669191, OnTutorialOpened);
        base.OnPrefabInit();
        if (visualizer != null) visualizer = Util.KInstantiate(visualizer);
        if (areaVisualizer != null) {
            areaVisualizer = Util.KInstantiate(areaVisualizer);
            areaVisualizer.SetActive(false);
            areaVisualizer.GetComponent<RectTransform>().SetParent(transform);
            areaVisualizer.GetComponent<Renderer>().material.color = areaColour;
        }
    }

    protected override void OnCmpEnable() { Dragging = false; }

    protected override void OnCmpDisable() {
        if (visualizer     != null) visualizer.SetActive(false);
        if (areaVisualizer != null) areaVisualizer.SetActive(false);
    }

    public override void OnLeftClickDown(Vector3 cursor_pos) {
        cursor_pos -= placementPivot;
        Dragging   =  true;
        downPos    =  cursor_pos;
        if (!KInputManager.currentControllerIsGamepad)
            KScreenManager.Instance.SetEventSystemEnabled(false);
        else
            SetCurrentVirtualInputModuleMousMovementMode(true);

        Paint();
    }

    public override void OnLeftClickUp(Vector3 cursor_pos) {
        cursor_pos -= placementPivot;
        KScreenManager.Instance.SetEventSystemEnabled(true);
        if (KInputManager.currentControllerIsGamepad) SetCurrentVirtualInputModuleMousMovementMode(false);
        if (!Dragging) return;

        Dragging = false;
        var dragAxis = this.dragAxis;
        if (dragAxis == DragAxis.Horizontal) {
            cursor_pos.y  = downPos.y;
            this.dragAxis = DragAxis.None;
            return;
        }

        if (dragAxis != DragAxis.Vertical) return;

        cursor_pos.x  = downPos.x;
        this.dragAxis = DragAxis.None;
    }

    protected virtual string GetConfirmSound()    { return "Tile_Confirm"; }
    protected virtual string GetDragSound()       { return "Tile_Drag"; }
    public override   string GetDeactivateSound() { return "Tile_Cancel"; }

    private static int GetGridDistance(int cell, int center_cell) {
        var u        = Grid.CellToXY(cell);
        var v        = Grid.CellToXY(center_cell);
        var vector2I = u - v;
        return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
    }

    private void Paint() {
        var count = visitedCells.Count;
        foreach (var num in cellsInRadius)
            if (Grid.IsValidCell(num)                                       &&
                Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId &&
                (!Grid.Foundation[num] || affectFoundation))
                OnPaintCell(num, Grid.GetCellDistance(currentCell, num));

        if (lastCell != currentCell) PlayDragSound();
        if (count    < visitedCells.Count) PlaySound();
    }

    protected virtual void PlayDragSound() {
        var dragSound = GetDragSound();
        if (!string.IsNullOrEmpty(dragSound)) {
            var sound = GlobalAssets.GetSound(dragSound);
            if (sound != null) {
                var pos = Grid.CellToPos(currentCell);
                pos.z = 0f;
                var cellDistance = Grid.GetCellDistance(Grid.PosToCell(downPos), currentCell);
                var instance     = SoundEvent.BeginOneShot(sound, pos);
                instance.setParameterByName("tileCount", cellDistance);
                SoundEvent.EndOneShot(instance);
            }
        }
    }

    public override void OnMouseMove(Vector3 cursorPos) {
        var num = Grid.PosToCell(cursorPos);
        currentCell = num;
        base.OnMouseMove(cursorPos);
        cellsInRadius.Clear();
        foreach (var vector in brushOffsets) {
            var num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
            if (Grid.IsValidCell(num2) && Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
                cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos),
                                                  new CellOffset((int)vector.x, (int)vector.y)));
        }

        if (!Dragging) return;

        Paint();
        lastCell = currentCell;
    }

    protected virtual void OnPaintCell(int cell, int distFromOrigin) {
        if (!visitedCells.Contains(cell)) visitedCells.Add(cell);
    }

    public override void OnKeyDown(KButtonEvent e) {
        if (e.TryConsume(Action.DragStraight))
            dragAxis = DragAxis.None;
        else if (interceptNumberKeysForPriority) HandlePriortyKeysDown(e);

        if (!e.Consumed) base.OnKeyDown(e);
    }

    public override void OnKeyUp(KButtonEvent e) {
        if (e.TryConsume(Action.DragStraight))
            dragAxis = DragAxis.Invalid;
        else if (interceptNumberKeysForPriority) HandlePriorityKeysUp(e);

        if (!e.Consumed) base.OnKeyUp(e);
    }

    private void HandlePriortyKeysDown(KButtonEvent e) {
        var action = e.GetAction();
        if (Action.Plan1 > action || action > Action.Plan10 || !e.TryConsume(action)) return;

        var num = action - Action.Plan1 + 1;
        if (num <= 9) {
            ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic,
                                                                num),
                                                               true);

            return;
        }

        ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority,
                                                                               1),
                                                           true);
    }

    private void HandlePriorityKeysUp(KButtonEvent e) {
        var action = e.GetAction();
        if (Action.Plan1 <= action && action <= Action.Plan10) e.TryConsume(action);
    }

    public override void OnFocus(bool focus) {
        if (visualizer != null) visualizer.SetActive(focus);
        hasFocus = focus;
        base.OnFocus(focus);
    }

    private         void OnTutorialOpened(object data) { Dragging = false; }
    public override bool ShowHoverUI()                 { return Dragging || base.ShowHoverUI(); }
    public override void LateUpdate()                  { base.LateUpdate(); }

    private enum DragAxis {
        Invalid = -1,
        None,
        Horizontal,
        Vertical
    }
}