using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevTool_StoryTraits_Reveal : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		int index;
		bool flag = DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out index);
		if (ImGuiEx.Button("Focus on headquaters", flag))
		{
			DevToolUtil.FocusCameraOnCell(index);
		}
		if (!flag)
		{
			ImGuiEx.TooltipForPrevious("Couldn't find headquaters");
		}
		if (!ImGui.CollapsingHeader("Search world for entity", ImGuiTreeNodeFlags.DefaultOpen))
		{
			return;
		}
		IReadOnlyList<WorldGenSpawner.Spawnable> allSpawnables = GetAllSpawnables();
		if (allSpawnables == null)
		{
			ImGui.Text("Couldn't find a list of spawnables");
			return;
		}
		foreach (string item in GetPrefabIDsToSearchFor())
		{
			int cellIndex;
			bool cellIndexForSpawnable = GetCellIndexForSpawnable(item, allSpawnables, out cellIndex);
			string text = "\"" + item + "\"";
			bool flag2 = cellIndexForSpawnable;
			if (ImGuiEx.Button("Reveal and focus on " + text, flag2))
			{
				DevToolUtil.RevealAndFocusAt(cellIndex);
			}
			if (!flag2)
			{
				ImGuiEx.TooltipForPrevious("Couldn't find a cell that contained a spawnable with component " + text);
			}
		}
	}

	public IEnumerable<string> GetPrefabIDsToSearchFor()
	{
		yield return "MegaBrainTank";
		yield return "GravitasCreatureManipulator";
		yield return "LonelyMinionHouse";
		yield return "FossilDig";
	}

	private bool GetCellIndexForSpawnable(string prefabId, IReadOnlyList<WorldGenSpawner.Spawnable> spawnablesToSearch, out int cellIndex)
	{
		foreach (WorldGenSpawner.Spawnable item in spawnablesToSearch)
		{
			if (prefabId == item.spawnInfo.id)
			{
				cellIndex = item.cell;
				return true;
			}
		}
		cellIndex = -1;
		return false;
	}

	private IReadOnlyList<WorldGenSpawner.Spawnable> GetAllSpawnables()
	{
		WorldGenSpawner worldGenSpawner = Object.FindObjectOfType<WorldGenSpawner>(includeInactive: true);
		if (worldGenSpawner == null)
		{
			return null;
		}
		IReadOnlyList<WorldGenSpawner.Spawnable> spawnables = worldGenSpawner.GetSpawnables();
		if (spawnables == null)
		{
			return null;
		}
		return spawnables;
	}
}
