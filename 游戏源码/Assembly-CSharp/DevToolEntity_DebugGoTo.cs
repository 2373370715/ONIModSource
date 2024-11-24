using ImGuiNET;
using UnityEngine;

public class DevToolEntity_DebugGoTo : DevTool
{
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	private Option<DevToolEntityTarget.ForSimCell> destinationSimCellTarget;

	private bool shouldDrawBoundingBox = true;

	private bool shouldContinouslyRequest;

	public DevToolEntity_DebugGoTo()
		: this(Option.None)
	{
	}

	public DevToolEntity_DebugGoTo(Option<DevToolEntityTarget.ForWorldGameObject> target)
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
		Name = "Debug Go To";
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
		Name = "Debug Go To for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		if (forWorldGameObject.gameObject.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("Target GameObject is null");
			return;
		}
		ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
		ImGuiEx.SimpleField("Target GameObject", DevToolEntity.GetNameFor(forWorldGameObject.gameObject));
		ImGuiEx.SimpleField("Destination Cell Index", GetCellName(destinationSimCellTarget));
		if (ImGui.Button("Select New Destination Cell"))
		{
			panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
			{
				destinationSimCellTarget = (DevToolEntityTarget.ForSimCell)target;
			}, (DevToolEntityTarget uncastTarget) => (!(uncastTarget is DevToolEntityTarget.ForSimCell)) ? ((Option<string>)"Target is not a sim cell") : ((Option<string>)Option.None)));
		}
		ImGui.Separator();
		ImGui.Checkbox("Should Continously Request", ref shouldContinouslyRequest);
		string error = (shouldContinouslyRequest ? "Disable continous requests" : (destinationSimCellTarget.IsNone() ? "No destination target." : null));
		if (ImGuiEx.Button("Request Target go to Destination", error) || (shouldContinouslyRequest && destinationSimCellTarget.IsSome()))
		{
			DebugGoToMonitor.Instance sMI = forWorldGameObject.gameObject.GetSMI<DebugGoToMonitor.Instance>();
			CreatureDebugGoToMonitor.Instance sMI2 = forWorldGameObject.gameObject.GetSMI<CreatureDebugGoToMonitor.Instance>();
			if (!sMI.IsNullOrDestroyed())
			{
				sMI.GoToCell(destinationSimCellTarget.Unwrap().cellIndex);
			}
			else if (!sMI2.IsNullOrDestroyed())
			{
				sMI2.GoToCell(destinationSimCellTarget.Unwrap().cellIndex);
			}
			else
			{
				DebugUtil.DevLogError("No debug goto SMI found");
			}
		}
		if (!shouldDrawBoundingBox)
		{
			return;
		}
		Option<(Vector2, Vector2)> screenRect = forWorldGameObject.GetScreenRect();
		if (screenRect.IsSome())
		{
			DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), "[Target]", ImGui.IsWindowFocused());
		}
		if (destinationSimCellTarget.IsSome())
		{
			Option<(Vector2, Vector2)> screenRect2 = destinationSimCellTarget.Unwrap().GetScreenRect();
			if (screenRect2.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect2.Unwrap(), "[Destination]", ImGui.IsWindowFocused());
			}
		}
		static string GetCellName(Option<DevToolEntityTarget.ForSimCell> target)
		{
			if (!target.IsNone())
			{
				return target.Unwrap().cellIndex.ToString();
			}
			return "<None>";
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
		if (forWorldGameObject.gameObject.GetSMI<DebugGoToMonitor.Instance>().IsNullOrDestroyed() && forWorldGameObject.gameObject.GetSMI<CreatureDebugGoToMonitor.Instance>().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have either a DebugGoToMonitor or CreatureDebugGoToMonitor";
		}
		return Option.None;
	}
}
