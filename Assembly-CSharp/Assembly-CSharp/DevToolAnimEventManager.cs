using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ImGuiNET;
using STRINGS;
using UnityEngine;

public class DevToolAnimEventManager : DevTool {
    protected override void RenderTo(DevPanel panel) {
        var                                 animEventManagerDebugInfo = GetAnimEventManagerDebugInfo();
        var                                 item                      = animEventManagerDebugInfo.Item1;
        bool                                flag;
        AnimEventManager.DevTools_DebugInfo devTools_DebugInfo;
        item.Deconstruct(out flag, out devTools_DebugInfo);
        var flag2               = flag;
        var devTools_DebugInfo2 = devTools_DebugInfo;
        var item2               = animEventManagerDebugInfo.Item2;
        if (!flag2) {
            ImGui.Text(item2);
            return;
        }

        if (ImGui.CollapsingHeader("World space animations", ImGuiTreeNodeFlags.DefaultOpen))
            DrawFor("ID_world_space_anims",
                    devTools_DebugInfo2.eventData.GetDataList(),
                    devTools_DebugInfo2.animData.GetDataList());

        if (ImGui.CollapsingHeader("UI space animations", ImGuiTreeNodeFlags.DefaultOpen))
            DrawFor("ID_ui_space_anims",
                    devTools_DebugInfo2.uiEventData.GetDataList(),
                    devTools_DebugInfo2.uiAnimData.GetDataList());

        if (ImGui.CollapsingHeader("Raw AnimEventManger", ImGuiTreeNodeFlags.DefaultOpen))
            ImGuiEx.DrawObject("Anim Event Manager", devTools_DebugInfo2.eventManager);
    }

    public void DrawFor(string                                 uniqueTableId,
                        List<AnimEventManager.EventPlayerData> eventDataList,
                        List<AnimEventManager.AnimData>        animDataList) {
        if (eventDataList == null) {
            ImGui.Text("Can't draw table: eventData is null");
            return;
        }

        if (animDataList == null) {
            ImGui.Text("Can't draw table: animData is null");
            return;
        }

        if (eventDataList.Count != animDataList.Count) {
            ImGui.Text(string.Format("Can't draw table: eventData.Count ({0}) != animData.Count ({1})",
                                     eventDataList.Count,
                                     animDataList.Count));

            return;
        }

        var count = eventDataList.Count;
        ImGui.PushID(uniqueTableId);
        var stateStorage = ImGui.GetStateStorage();
        var id           = ImGui.GetID("ID_should_expand_full_height");
        var flag         = stateStorage.GetBool(id);
        if (ImGui.Button(flag ? "Unexpand Height" : "Expand Height")) {
            flag = !flag;
            stateStorage.SetBool(id, flag);
        }

        var flags = ImGuiTableFlags.Resizable      |
                    ImGuiTableFlags.RowBg          |
                    ImGuiTableFlags.BordersInnerH  |
                    ImGuiTableFlags.BordersOuterH  |
                    ImGuiTableFlags.BordersInnerV  |
                    ImGuiTableFlags.BordersOuterV  |
                    ImGuiTableFlags.SizingFixedFit |
                    ImGuiTableFlags.ScrollY;

        if (ImGui.BeginTable("ID_table_contents", 4, flags, new Vector2(-1f, flag ? -1 : 400))) {
            ImGui.TableSetupScrollFreeze(4, 1);
            ImGui.TableSetupColumn("Game Object Name");
            ImGui.TableSetupColumn("Event Frame");
            ImGui.TableSetupColumn("Animation Frame");
            ImGui.TableSetupColumn("Event - Animation Frame Diff");
            ImGui.TableHeadersRow();
            for (var i = 0; i < count; i++) {
                var eventPlayerData = eventDataList[i];
                var animData        = animDataList[i];
                ImGui.TableNextRow();
                ImGui.PushID(string.Format("ID_row_{0}", i++));
                ImGui.TableNextColumn();
                if (ImGuiEx.Button("Focus", DevToolUtil.CanRevealAndFocus(eventPlayerData.controller.gameObject)))
                    DevToolUtil.RevealAndFocus(eventPlayerData.controller.gameObject);

                ImGuiEx.TooltipForPrevious("Will move the in-game camera to this gameobject");
                ImGui.SameLine();
                ImGui.Text(UI.StripLinkFormatting(eventPlayerData.controller.gameObject.name));
                ImGui.TableNextColumn();
                ImGui.Text(eventPlayerData.currentFrame.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(eventPlayerData.controller.currentFrame.ToString());
                ImGui.TableNextColumn();
                ImGui.Text((eventPlayerData.currentFrame - eventPlayerData.controller.currentFrame).ToString());
                ImGui.PopID();
            }

            ImGui.EndTable();
        }

        ImGui.PopID();
    }

    [return: TupleElementNames(new[] { "value", "error" })]
    public ValueTuple<Option<AnimEventManager.DevTools_DebugInfo>, string> GetAnimEventManagerDebugInfo() {
        if (Singleton<AnimEventManager>.Instance == null)
            return new ValueTuple<Option<AnimEventManager.DevTools_DebugInfo>, string>(Option.None,
             "AnimEventManager is null");

        return new
            ValueTuple<Option<AnimEventManager.DevTools_DebugInfo>, string>(Option.Some(Singleton<AnimEventManager>
                                                                                .Instance.DevTools_GetDebugInfo()),
                                                                            null);
    }
}