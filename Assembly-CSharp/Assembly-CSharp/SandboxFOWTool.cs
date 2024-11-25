using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class SandboxFOWTool : BrushTool {
    public static SandboxFOWTool  instance;
    private       EventInstance   ev;
    protected     Color           recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
    protected     HashSet<int>    recentlyAffectedCells     = new HashSet<int>();
    private       SandboxSettings settings          => SandboxToolParameterMenu.instance.settings;
    public static void            DestroyInstance() { instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        instance = this;
    }

    protected override string GetDragSound() { return ""; }
    public             void   Activate()     { PlayerController.Instance.ActivateTool(this); }

    protected override void OnActivateTool() {
        base.OnActivateTool();
        SandboxToolParameterMenu.instance.gameObject.SetActive(true);
        SandboxToolParameterMenu.instance.DisableParameters();
        SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    }

    protected override void OnDeactivateTool(InterfaceTool new_tool) {
        base.OnDeactivateTool(new_tool);
        SandboxToolParameterMenu.instance.gameObject.SetActive(false);
        ev.release();
    }

    public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors) {
        colors = new HashSet<ToolMenu.CellColorData>();
        foreach (var cell in recentlyAffectedCells)
            colors.Add(new ToolMenu.CellColorData(cell, recentlyAffectedCellColor));

        foreach (var cell2 in cellsInRadius) colors.Add(new ToolMenu.CellColorData(cell2, radiusIndicatorColor));
    }

    public override void OnMouseMove(Vector3 cursorPos) { base.OnMouseMove(cursorPos); }

    protected override void OnPaintCell(int cell, int distFromOrigin) {
        base.OnPaintCell(cell, distFromOrigin);
        Grid.Reveal(cell, byte.MaxValue, true);
    }

    public override void OnLeftClickDown(Vector3 cursor_pos) {
        base.OnLeftClickDown(cursor_pos);
        var intSetting = settings.GetIntSetting("SandboxTools.BrushSize");
        ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal"));
        ev.setParameterByName("BrushSize", intSetting);
        ev.start();
    }

    public override void OnLeftClickUp(Vector3 cursor_pos) {
        base.OnLeftClickUp(cursor_pos);
        ev.stop(STOP_MODE.ALLOWFADEOUT);
        ev.release();
    }
}