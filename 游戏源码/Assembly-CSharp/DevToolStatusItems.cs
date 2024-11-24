using ImGuiNET;
using UnityEngine;

public class DevToolStatusItems : DevTool
{
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	private ImGuiObjectTableDrawer<StatusItemGroup.Entry> tableDrawer;

	private StatusItemStackTraceWatcher statusItemStackTraceWatcher = new StatusItemStackTraceWatcher();

	private bool shouldDrawBoundingBox = true;

	public DevToolStatusItems()
		: this(Option.None)
	{
	}

	public DevToolStatusItems(Option<DevToolEntityTarget.ForWorldGameObject> target)
	{
		targetOpt = target;
		tableDrawer = ImGuiObjectTableDrawer<StatusItemGroup.Entry>.New().RemoveFlags(ImGuiTableFlags.SizingFixedFit).AddFlags(ImGuiTableFlags.Resizable)
			.Column("Text", (StatusItemGroup.Entry entry) => entry.GetName())
			.Column("Id Name", (StatusItemGroup.Entry entry) => entry.item.Id)
			.Column("Notification Type", (StatusItemGroup.Entry entry) => entry.item.notificationType)
			.Column("Category", (StatusItemGroup.Entry entry) => entry.category?.Name ?? "<no category>")
			.Column("OnAdded Callstack", delegate(StatusItemGroup.Entry entry)
			{
				if (statusItemStackTraceWatcher.GetStackTraceForEntry(entry, out var stackTrace))
				{
					if (ImGui.Selectable("copy callstack"))
					{
						ImGui.SetClipboardText(stackTrace.ToString());
					}
					ImGuiEx.TooltipForPrevious(stackTrace.ToString());
				}
				else
				{
					ImGui.Text("<None>");
				}
			})
			.Build();
		base.OnUninit += delegate
		{
			statusItemStackTraceWatcher.Dispose();
		};
	}

	protected override void RenderTo(DevPanel panel)
	{
		statusItemStackTraceWatcher.SetTarget(targetOpt.AndThen((DevToolEntityTarget.ForWorldGameObject t) => t.gameObject).AndThen((GameObject go) => go.GetComponent<KSelectable>()).AndThen((KSelectable s) => s.GetStatusItemGroup()));
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					targetOpt = (DevToolEntityTarget.ForWorldGameObject)target;
				}, GetErrorForCandidateTarget));
			}
			string error = null;
			if (targetOpt.IsNone())
			{
				error = "No target selected.";
			}
			else
			{
				Option<string> errorForCandidateTarget = GetErrorForCandidateTarget(targetOpt.Unwrap());
				if (errorForCandidateTarget.IsSome())
				{
					error = errorForCandidateTarget.Unwrap();
				}
			}
			if (ImGuiEx.MenuItem("Debug Target", error))
			{
				panel.PushValue(targetOpt.Unwrap());
			}
			ImGui.EndMenuBar();
		}
		Name = "Status Items";
		if (targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = targetOpt.Unwrap();
		Option<string> errorForCandidateTarget2 = GetErrorForCandidateTarget(forWorldGameObject);
		if (errorForCandidateTarget2.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget2.Unwrap());
			return;
		}
		Name = "Status Items for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		bool v = statusItemStackTraceWatcher.GetShouldWatch();
		if (ImGui.Checkbox("Should Track OnAdded Callstacks", ref v))
		{
			statusItemStackTraceWatcher.SetShouldWatch(v);
		}
		ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
		tableDrawer.Draw(forWorldGameObject.gameObject.GetComponent<KSelectable>().GetStatusItemGroup().GetEnumerator());
		if (shouldDrawBoundingBox)
		{
			Option<(Vector2, Vector2)> screenRect = forWorldGameObject.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), forWorldGameObject.GetDebugName(), ImGui.IsWindowFocused());
			}
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
		KSelectable component = forWorldGameObject.gameObject.GetComponent<KSelectable>();
		if (component.IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a KSelectable";
		}
		if (component.GetStatusItemGroup().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a StatusItemGroup";
		}
		return Option.None;
	}
}
