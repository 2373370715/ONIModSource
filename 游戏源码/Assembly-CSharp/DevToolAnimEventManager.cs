using System.Collections.Generic;
using ImGuiNET;
using STRINGS;
using UnityEngine;

public class DevToolAnimEventManager : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		(Option<AnimEventManager.DevTools_DebugInfo>, string) animEventManagerDebugInfo = GetAnimEventManagerDebugInfo();
		var (option, _) = animEventManagerDebugInfo;
		option.Deconstruct(out var hasValue, out var value);
		bool num = hasValue;
		AnimEventManager.DevTools_DebugInfo devTools_DebugInfo = value;
		string item = animEventManagerDebugInfo.Item2;
		if (!num)
		{
			ImGui.Text(item);
			return;
		}
		if (ImGui.CollapsingHeader("World space animations", ImGuiTreeNodeFlags.DefaultOpen))
		{
			DrawFor("ID_world_space_anims", devTools_DebugInfo.eventData.GetDataList(), devTools_DebugInfo.animData.GetDataList());
		}
		if (ImGui.CollapsingHeader("UI space animations", ImGuiTreeNodeFlags.DefaultOpen))
		{
			DrawFor("ID_ui_space_anims", devTools_DebugInfo.uiEventData.GetDataList(), devTools_DebugInfo.uiAnimData.GetDataList());
		}
		if (ImGui.CollapsingHeader("Raw AnimEventManger", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGuiEx.DrawObject("Anim Event Manager", devTools_DebugInfo.eventManager);
		}
	}

	public void DrawFor(string uniqueTableId, List<AnimEventManager.EventPlayerData> eventDataList, List<AnimEventManager.AnimData> animDataList)
	{
		if (eventDataList == null)
		{
			ImGui.Text("Can't draw table: eventData is null");
			return;
		}
		if (animDataList == null)
		{
			ImGui.Text("Can't draw table: animData is null");
			return;
		}
		if (eventDataList.Count != animDataList.Count)
		{
			ImGui.Text($"Can't draw table: eventData.Count ({eventDataList.Count}) != animData.Count ({animDataList.Count})");
			return;
		}
		int count = eventDataList.Count;
		ImGui.PushID(uniqueTableId);
		ImGuiStoragePtr stateStorage = ImGui.GetStateStorage();
		uint iD = ImGui.GetID("ID_should_expand_full_height");
		bool flag = stateStorage.GetBool(iD);
		if (ImGui.Button(flag ? "Unexpand Height" : "Expand Height"))
		{
			flag = !flag;
			stateStorage.SetBool(iD, flag);
		}
		ImGuiTableFlags flags = ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollY;
		if (ImGui.BeginTable("ID_table_contents", 4, flags, new Vector2(-1f, flag ? (-1) : 400)))
		{
			ImGui.TableSetupScrollFreeze(4, 1);
			ImGui.TableSetupColumn("Game Object Name");
			ImGui.TableSetupColumn("Event Frame");
			ImGui.TableSetupColumn("Animation Frame");
			ImGui.TableSetupColumn("Event - Animation Frame Diff");
			ImGui.TableHeadersRow();
			for (int i = 0; i < count; i++)
			{
				AnimEventManager.EventPlayerData eventPlayerData = eventDataList[i];
				_ = animDataList[i];
				ImGui.TableNextRow();
				ImGui.PushID($"ID_row_{i++}");
				ImGui.TableNextColumn();
				if (ImGuiEx.Button("Focus", DevToolUtil.CanRevealAndFocus(eventPlayerData.controller.gameObject)))
				{
					DevToolUtil.RevealAndFocus(eventPlayerData.controller.gameObject);
				}
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

	public (Option<AnimEventManager.DevTools_DebugInfo> value, string error) GetAnimEventManagerDebugInfo()
	{
		if (Singleton<AnimEventManager>.Instance == null)
		{
			return (value: Option.None, error: "AnimEventManager is null");
		}
		return (value: Option.Some(Singleton<AnimEventManager>.Instance.DevTools_GetDebugInfo()), error: null);
	}
}
