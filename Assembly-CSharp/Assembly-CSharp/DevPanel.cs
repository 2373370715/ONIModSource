using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevPanel {
    public readonly  uint          idPostfixNumber;
    public readonly  Type          initialDevToolType;
    public readonly  DevPanelList  manager;
    public readonly  string        uniquePanelId;
    private          int           currentDevToolIndex;
    private readonly List<DevTool> devTools;

    public DevPanel(DevTool devTool, DevPanelList manager) {
        this.manager = manager;
        devTools     = new List<DevTool>();
        devTools.Add(devTool);
        currentDevToolIndex = 0;
        initialDevToolType  = devTool.GetType();
        manager.Internal_InitPanelId(initialDevToolType, out uniquePanelId, out idPostfixNumber);
    }

    public bool isRequestingToClose { get; private set; }
    public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }
    public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }
    public void PushValue<T>(T value) where T : class { PushDevTool(new DevToolObjectViewer<T>(() => value)); }
    public void PushValue<T>(Func<T> value) { PushDevTool(new DevToolObjectViewer<T>(value)); }
    public void PushDevTool<T>() where T : DevTool, new() { PushDevTool(Activator.CreateInstance<T>()); }

    public void PushDevTool(DevTool devTool) {
        if (Input.GetKey(KeyCode.LeftShift)) {
            manager.AddPanelFor(devTool);
            return;
        }

        for (var i = devTools.Count - 1; i > currentDevToolIndex; i--) {
            devTools[i].Internal_Uninit();
            devTools.RemoveAt(i);
        }

        devTools.Add(devTool);
        currentDevToolIndex = devTools.Count - 1;
    }

    public bool NavGoBack() {
        var option = TryGetDevToolIndexByOffset(-1);
        if (option.IsNone()) return false;

        currentDevToolIndex = option.Unwrap();
        return true;
    }

    public bool NavGoForward() {
        var option = TryGetDevToolIndexByOffset(1);
        if (option.IsNone()) return false;

        currentDevToolIndex = option.Unwrap();
        return true;
    }

    public DevTool GetCurrentDevTool() { return devTools[currentDevToolIndex]; }

    public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex) {
        var num = currentDevToolIndex + offsetFromCurrentIndex;
        if (num < 0) return Option.None;

        if (num >= devTools.Count) return Option.None;

        return num;
    }

    public void RenderPanel() {
        var currentDevTool = GetCurrentDevTool();
        currentDevTool.Internal_TryInit();
        if (currentDevTool.isRequestingToClosePanel) {
            isRequestingToClose = true;
            return;
        }

        ImGuiWindowFlags flags;
        ConfigureImGuiWindowFor(currentDevTool, out flags);
        currentDevTool.Internal_Update();
        var flag = true;
        if (ImGui.Begin(currentDevTool.Name + "###ID_" + uniquePanelId, ref flag, flags)) {
            if (!flag) {
                isRequestingToClose = true;
                ImGui.End();
                return;
            }

            if (ImGui.BeginMenuBar()) {
                DrawNavigation();
                ImGui.SameLine(0f, 20f);
                DrawMenuBarContents();
                ImGui.EndMenuBar();
            }

            currentDevTool.DoImGui(this);
            if (GetCurrentDevTool() != currentDevTool) ImGui.SetScrollY(0f);
        }

        ImGui.End();
        if (GetCurrentDevTool().isRequestingToClosePanel) isRequestingToClose = true;
    }

    private void DrawNavigation() {
        var option                                                      = TryGetDevToolIndexByOffset(-1);
        if (ImGuiEx.Button(" < ", option.IsSome())) currentDevToolIndex = option.Unwrap();
        if (option.IsSome())
            ImGuiEx.TooltipForPrevious("Go back to " + devTools[option.Unwrap()].Name);
        else
            ImGuiEx.TooltipForPrevious("Go back");

        ImGui.SameLine(0f, 5f);
        var option2                                                      = TryGetDevToolIndexByOffset(1);
        if (ImGuiEx.Button(" > ", option2.IsSome())) currentDevToolIndex = option2.Unwrap();
        if (option2.IsSome()) {
            ImGuiEx.TooltipForPrevious("Go forward to " + devTools[option2.Unwrap()].Name);
            return;
        }

        ImGuiEx.TooltipForPrevious("Go forward");
    }

    private void DrawMenuBarContents() { }

    private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags) {
        drawFlags = ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags;
        if (nextImGuiWindowPosition.HasValue) {
            var value = nextImGuiWindowPosition.Value;
            var item  = value.Item1;
            var item2 = value.Item2;
            ImGui.SetNextWindowPos(item, item2);
            nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
        }

        if (nextImGuiWindowSize.HasValue) {
            var item3 = nextImGuiWindowSize.Value.Item1;
            ImGui.SetNextWindowSize(item3);
            nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
        }
    }

    public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None) {
        nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
    }

    public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None) {
        nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
    }

    public void Close() { isRequestingToClose = true; }

    public void Internal_Uninit() {
        foreach (var devTool in devTools) devTool.Internal_Uninit();
    }
}