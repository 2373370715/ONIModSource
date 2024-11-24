using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using ImGuiObjectDrawer;
using UnityEngine;

public class DevToolCavity : DevTool
{
	private Option<DevToolEntityTarget.ForSimCell> targetOpt;

	private bool shouldDrawBoundingBox = true;

	public DevToolCavity()
		: this(Option.None)
	{
	}

	public DevToolCavity(Option<DevToolEntityTarget.ForSimCell> target)
	{
		targetOpt = target;
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					targetOpt = (DevToolEntityTarget.ForSimCell)target;
				}, GetErrorForCandidateTarget));
			}
			ImGui.EndMenuBar();
		}
		Name = "Cavity Info";
		if (targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		if (Game.Instance.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No Game instance");
			return;
		}
		if (Game.Instance.roomProber.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No RoomProber instance");
			return;
		}
		DevToolEntityTarget.ForSimCell forSimCell = targetOpt.Unwrap();
		Option<string> errorForCandidateTarget = GetErrorForCandidateTarget(forSimCell);
		if (errorForCandidateTarget.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget.Unwrap());
			return;
		}
		Name = $"Cavity Info for: Cell {forSimCell.cellIndex}";
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(forSimCell.cellIndex);
		if (cavityForCell.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No Cavity found");
			return;
		}
		ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
		ImGuiEx.SimpleField("Room Type", cavityForCell.room.IsNullOrDestroyed() ? "<None>" : cavityForCell.room.GetProperName());
		ImGuiEx.SimpleField("Cell Count", cavityForCell.numCells);
		DrawKPrefabIdCollection("Creatures", cavityForCell.creatures);
		DrawKPrefabIdCollection("Buildings", cavityForCell.buildings);
		DrawKPrefabIdCollection("Plants", cavityForCell.plants);
		DrawKPrefabIdCollection("Eggs", cavityForCell.eggs);
		if (ImGui.CollapsingHeader("Full CavityInfo Object"))
		{
			ImGuiEx.DrawObject("CavityInfo", cavityForCell, new MemberDrawContext(hide_default_values: false, default_open: true));
		}
		if (!shouldDrawBoundingBox)
		{
			return;
		}
		Option<(Vector2, Vector2)> screenRect = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.minX, cavityForCell.minY)).GetScreenRect();
		Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.maxX, cavityForCell.maxY)).GetScreenRect();
		if (screenRect.IsSome() && screenRect2.IsSome())
		{
			DevToolEntity.DrawBoundingBox((cornerA: Vector2.Min(screenRect.Unwrap().Item1, Vector2.Min(screenRect.Unwrap().Item2, Vector2.Min(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2))), cornerB: Vector2.Max(screenRect.Unwrap().Item1, Vector2.Max(screenRect.Unwrap().Item2, Vector2.Max(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2)))), cavityForCell.room.IsNullOrDestroyed() ? "<Room is null>" : cavityForCell.room.GetProperName(), ImGui.IsWindowFocused());
			Option<(Vector2, Vector2)> screenRect3 = forSimCell.GetScreenRect();
			if (screenRect3.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect3.Unwrap(), forSimCell.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	public static void DrawKPrefabIdCollection(string name, IEnumerable<KPrefabID> kprefabIds)
	{
		name += (kprefabIds.IsNullOrDestroyed() ? " (0)" : $" ({kprefabIds.Count()})");
		if (!ImGui.CollapsingHeader(name))
		{
			return;
		}
		if (kprefabIds.IsNullOrDestroyed())
		{
			ImGui.Text("List is null");
			return;
		}
		if (kprefabIds.Count() == 0)
		{
			ImGui.Text("List is empty");
			return;
		}
		foreach (KPrefabID kprefabId in kprefabIds)
		{
			ImGui.Text(kprefabId.ToString());
			ImGui.SameLine();
			if (ImGui.Button($"DevTool Inspect###ID_Inspect_{kprefabId.GetInstanceID()}"))
			{
				DevToolSceneInspector.Inspect(kprefabId);
			}
		}
	}

	public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
	{
		if (!(uncastTarget is DevToolEntityTarget.ForSimCell))
		{
			return "Target must be a sim cell";
		}
		DevToolEntityTarget.ForSimCell forSimCell = (DevToolEntityTarget.ForSimCell)uncastTarget;
		if (Game.Instance.IsNullOrDestroyed())
		{
			return "No Game instance found.";
		}
		if (forSimCell.cellIndex < 0 || Grid.CellCount <= forSimCell.cellIndex)
		{
			return $"Found cell index {forSimCell.cellIndex} is out of range {forSimCell.cellIndex}..{Grid.CellCount}";
		}
		if (!Grid.IsValidCell(forSimCell.cellIndex))
		{
			return $"Cell index {forSimCell.cellIndex} is invalid";
		}
		return Option.None;
	}
}
