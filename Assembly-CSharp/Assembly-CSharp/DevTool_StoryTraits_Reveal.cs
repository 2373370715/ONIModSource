using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevTool_StoryTraits_Reveal : DevTool {
    protected override void RenderTo(DevPanel panel) {
        int cellIndex;
        var flag = DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cellIndex);
        if (ImGuiEx.Button("Focus on headquaters", flag)) DevToolUtil.FocusCameraOnCell(cellIndex);
        if (!flag) ImGuiEx.TooltipForPrevious("Couldn't find headquaters");
        if (ImGui.CollapsingHeader("Search world for entity", ImGuiTreeNodeFlags.DefaultOpen)) {
            var allSpawnables = GetAllSpawnables();
            if (allSpawnables == null) {
                ImGui.Text("Couldn't find a list of spawnables");
                return;
            }

            foreach (var text in GetPrefabIDsToSearchFor()) {
                int cellIndex2;
                var cellIndexForSpawnable = GetCellIndexForSpawnable(text, allSpawnables, out cellIndex2);
                var str                   = "\"" + text + "\"";
                var flag2                 = cellIndexForSpawnable;
                if (ImGuiEx.Button("Reveal and focus on " + str, flag2)) DevToolUtil.RevealAndFocusAt(cellIndex2);
                if (!flag2)
                    ImGuiEx.TooltipForPrevious("Couldn't find a cell that contained a spawnable with component " + str);
            }
        }
    }

    public IEnumerable<string> GetPrefabIDsToSearchFor() {
        yield return "MegaBrainTank";
        yield return "GravitasCreatureManipulator";
        yield return "LonelyMinionHouse";
        yield return "FossilDig";
    }

    private bool GetCellIndexForSpawnable(string                                   prefabId,
                                          IReadOnlyList<WorldGenSpawner.Spawnable> spawnablesToSearch,
                                          out int                                  cellIndex) {
        foreach (var spawnable in spawnablesToSearch)
            if (prefabId == spawnable.spawnInfo.id) {
                cellIndex = spawnable.cell;
                return true;
            }

        cellIndex = -1;
        return false;
    }

    private IReadOnlyList<WorldGenSpawner.Spawnable> GetAllSpawnables() {
        var worldGenSpawner = Object.FindObjectOfType<WorldGenSpawner>(true);
        if (worldGenSpawner == null) return null;

        var spawnables = worldGenSpawner.GetSpawnables();
        if (spawnables == null) return null;

        return spawnables;
    }
}