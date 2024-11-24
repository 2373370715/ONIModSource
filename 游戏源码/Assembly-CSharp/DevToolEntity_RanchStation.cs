using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using ImGuiObjectDrawer;
using UnityEngine;

public class DevToolEntity_RanchStation : DevTool
{
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	private bool shouldDrawBoundingBox = true;

	public DevToolEntity_RanchStation()
		: this(Option.None)
	{
	}

	public DevToolEntity_RanchStation(Option<DevToolEntityTarget.ForWorldGameObject> target)
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
					targetOpt = (DevToolEntityTarget.ForWorldGameObject)target;
				}, GetErrorForCandidateTarget));
			}
			ImGui.EndMenuBar();
		}
		Name = "RanchStation debug";
		if (targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = targetOpt.Unwrap();
		Option<string> errorForCandidateTarget = GetErrorForCandidateTarget(forWorldGameObject);
		if (errorForCandidateTarget.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget.Unwrap());
			return;
		}
		Name = "RanchStation debug for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		RanchStation.Instance sMI = forWorldGameObject.gameObject.GetSMI<RanchStation.Instance>();
		RanchStation.Def def = forWorldGameObject.gameObject.GetDef<RanchStation.Def>();
		StateMachine stateMachine = sMI.GetStateMachine();
		DrawRanchableCollection("Target Ranchables", sMI.DEBUG_GetTargetRanchables());
		if (ImGui.CollapsingHeader("Full Debug Info"))
		{
			ImGuiEx.DrawObject("State Machine Instance", sMI, new MemberDrawContext(hide_default_values: false, default_open: false));
			ImGuiEx.DrawObject("State Machine Def", def, new MemberDrawContext(hide_default_values: false, default_open: false));
			ImGuiEx.DrawObject("State Machine", stateMachine, new MemberDrawContext(hide_default_values: false, default_open: false));
		}
		if (!shouldDrawBoundingBox)
		{
			return;
		}
		Option<(Vector2, Vector2)> screenRect = forWorldGameObject.GetScreenRect();
		if (screenRect.IsSome())
		{
			DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), "[Ranching Station]", ImGui.IsWindowFocused());
		}
		List<RanchableMonitor.Instance> list = sMI.DEBUG_GetTargetRanchables();
		for (int i = 0; i < list.Count; i++)
		{
			RanchableMonitor.Instance instance = list[i];
			if (!instance.gameObject.IsNullOrDestroyed())
			{
				Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForWorldGameObject(instance.gameObject).GetScreenRect();
				if (screenRect2.IsSome())
				{
					DevToolEntity.DrawBoundingBox(screenRect2.Unwrap(), $"[Target Ranchable @ Index {i}]", ImGui.IsWindowFocused());
				}
			}
		}
	}

	public static void DrawRanchableCollection(string name, IEnumerable<RanchableMonitor.Instance> ranchables)
	{
		if (!ImGui.CollapsingHeader(name))
		{
			return;
		}
		if (ranchables.IsNullOrDestroyed())
		{
			ImGui.Text("List is null");
			return;
		}
		if (ranchables.Count() == 0)
		{
			ImGui.Text("List is empty");
			return;
		}
		int num = 0;
		foreach (RanchableMonitor.Instance ranchable in ranchables)
		{
			ImGui.Text(ranchable.IsNullOrDestroyed() ? "<null RanchableMonitor>" : DevToolEntity.GetNameFor(ranchable.gameObject));
			ImGui.SameLine();
			if (ImGui.Button($"DevTool Inspect###ID_Inspect_{num}"))
			{
				DevToolSceneInspector.Inspect(ranchable);
			}
			num++;
		}
	}

	public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
	{
		if (!(uncastTarget is DevToolEntityTarget.ForWorldGameObject))
		{
			return "Target must be a world GameObject";
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = (DevToolEntityTarget.ForWorldGameObject)uncastTarget;
		if (forWorldGameObject.gameObject.IsNullOrDestroyed())
		{
			return "Target GameObject is null or destroyed";
		}
		if (forWorldGameObject.gameObject.GetDef<RanchStation.Def>().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a RanchStation.Def";
		}
		return Option.None;
	}
}
