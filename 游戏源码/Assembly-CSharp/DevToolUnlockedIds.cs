using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

public class DevToolUnlockedIds : DevTool
{
	public readonly struct UnlocksWrapper
	{
		public readonly Unlocks unlocks;

		public int Count => unlocks.GetAllUnlockedIds().Count;

		public UnlocksWrapper(Unlocks unlocks)
		{
			this.unlocks = unlocks;
		}

		public void AddId(string unlockId)
		{
			unlocks.Unlock(unlockId);
		}

		public void RemoveId(string unlockId)
		{
			unlocks.Lock(unlockId);
		}

		public IEnumerable<string> GetAllIds()
		{
			return from s in unlocks.GetAllUnlockedIds()
				orderby s
				select s;
		}
	}

	private string filterForUnlockIds = "";

	private string unlockIdToAdd = "";

	public DevToolUnlockedIds()
	{
		RequiresGameRunning = true;
	}

	protected override void RenderTo(DevPanel panel)
	{
		var (flag2, unlocksWrapper2) = (Option<UnlocksWrapper>)(ref GetUnlocks());
		if (!flag2)
		{
			ImGui.Text("Couldn't access global unlocks");
			return;
		}
		if (ImGui.TreeNode("Help"))
		{
			ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
			ImGui.Spacing();
			ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
			ImGui.TreePop();
		}
		ImGui.Spacing();
		ImGuiEx.InputFilter("Filter", ref filterForUnlockIds);
		ImGuiTableFlags flags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY;
		if (!ImGui.BeginTable("ID_unlockIds", 2, flags))
		{
			return;
		}
		ImGui.TableSetupScrollFreeze(2, 2);
		ImGui.TableSetupColumn("Unlock ID");
		ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
		ImGui.TableHeadersRow();
		ImGui.PushID("ID_row_add_new");
		ImGui.TableNextRow();
		ImGui.TableSetColumnIndex(0);
		ImGui.InputText("", ref unlockIdToAdd, 50u);
		ImGui.TableSetColumnIndex(1);
		if (ImGui.Button("Add"))
		{
			unlocksWrapper2.AddId(unlockIdToAdd);
			Debug.Log("[Added unlock id] " + unlockIdToAdd);
			unlockIdToAdd = "";
		}
		ImGui.PopID();
		int num = 0;
		foreach (string allId in unlocksWrapper2.GetAllIds())
		{
			string text = ((allId == null) ? "<<null>>" : ("\"" + allId + "\""));
			if (text.ToLower().Contains(filterForUnlockIds.ToLower()))
			{
				ImGui.TableNextRow();
				ImGui.PushID($"ID_row_{num++}");
				ImGui.TableSetColumnIndex(0);
				ImGui.Text(text);
				ImGui.TableSetColumnIndex(1);
				if (ImGui.Button("Copy"))
				{
					GUIUtility.systemCopyBuffer = allId;
					Debug.Log("[Copied to clipboard] " + allId);
				}
				ImGui.SameLine();
				if (ImGui.Button("Remove"))
				{
					unlocksWrapper2.RemoveId(allId);
					Debug.Log("[Removed unlock id] " + allId);
				}
				ImGui.PopID();
			}
		}
		ImGui.EndTable();
	}

	private Option<UnlocksWrapper> GetUnlocks()
	{
		if (App.IsExiting)
		{
			return Option.None;
		}
		if (Game.Instance == null || !Game.Instance)
		{
			return Option.None;
		}
		if (Game.Instance.unlocks == null)
		{
			return Option.None;
		}
		return Option.Some(new UnlocksWrapper(Game.Instance.unlocks));
	}
}
