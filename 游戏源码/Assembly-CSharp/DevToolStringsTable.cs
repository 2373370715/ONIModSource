using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevToolStringsTable : DevTool
{
	private List<(string id, string value)> m_cached_entries;

	private const int MAX_ENTRIES_TO_DRAW = 3000;

	private string m_search_filter = "";

	protected override void RenderTo(DevPanel panel)
	{
		if (m_cached_entries == null)
		{
			m_cached_entries = new List<(string, string)>();
			RegenerateCacheWithFilter(m_cached_entries, m_search_filter);
		}
		if (!ImGui.CollapsingHeader($"Entries ({m_cached_entries.Count})###ID_LocStringEntries", ImGuiTreeNodeFlags.DefaultOpen))
		{
			return;
		}
		if (ImGuiEx.InputFilter("Filter", ref m_search_filter))
		{
			RegenerateCacheWithFilter(m_cached_entries, m_search_filter);
		}
		ImGui.Columns(2, "LocStrings");
		ImGui.Text("Key");
		ImGui.NextColumn();
		ImGui.Text("Value");
		ImGui.NextColumn();
		ImGui.Separator();
		int num = Mathf.Min(3000, m_cached_entries.Count);
		for (int i = 0; i < num; i++)
		{
			var (text, text2) = m_cached_entries[i];
			if (ImGui.Selectable($"{text}###ID_{i}_key"))
			{
				m_search_filter = text;
				RegenerateCacheWithFilter(m_cached_entries, m_search_filter);
				break;
			}
			ImGuiEx.TooltipForPrevious(text ?? "");
			ImGui.NextColumn();
			if (ImGui.Selectable($"{text2}###ID_{i}_value"))
			{
				m_search_filter = text2;
				RegenerateCacheWithFilter(m_cached_entries, m_search_filter);
				break;
			}
			ImGuiEx.TooltipForPrevious(text2 ?? "");
			ImGui.NextColumn();
		}
		ImGui.Columns(1);
		if (m_cached_entries.Count > 3000)
		{
			ImGui.Separator();
			ImGui.Text($"* Stopped drawing entries because there are too many to draw (limit: {3000}, current: {m_cached_entries.Count}) *");
		}
	}

	public static void RegenerateCacheWithFilter(List<(string id, string value)> cached_entries, string filter)
	{
		cached_entries.Clear();
		if (!string.IsNullOrWhiteSpace(filter))
		{
			string normalized_filter = filter.ToLowerInvariant().Trim();
			Strings.VisitEntries(delegate(string id, string value)
			{
				if (id.ToLowerInvariant().Contains(normalized_filter) || value.ToLowerInvariant().Contains(normalized_filter))
				{
					cached_entries.Add((id, value));
				}
			});
		}
		else
		{
			Strings.VisitEntries(delegate(string id, string value)
			{
				cached_entries.Add((id, value));
			});
		}
	}
}
