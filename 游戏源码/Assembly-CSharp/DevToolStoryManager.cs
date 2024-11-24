using System.Collections.Generic;
using ImGuiNET;

public class DevToolStoryManager : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Story Instance Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			DrawStoryInstanceData();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Story Telemetry Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			DrawTelemetryData();
		}
	}

	private void DrawStoryInstanceData()
	{
		if (StoryManager.Instance == null)
		{
			ImGui.Text("Couldn't find StoryManager instance");
			return;
		}
		ImGui.Text($"Stories (count: {StoryManager.Instance.GetStoryInstances().Count})");
		string text = ((StoryManager.Instance.GetHighestCoordinate() == -2) ? "Before stories" : StoryManager.Instance.GetHighestCoordinate().ToString());
		ImGui.Text("Highest generated: " + text);
		foreach (KeyValuePair<int, StoryInstance> storyInstance in StoryManager.Instance.GetStoryInstances())
		{
			ImGui.Text(" - " + storyInstance.Value.storyId + ": " + storyInstance.Value.CurrentState);
		}
		if (StoryManager.Instance.GetStoryInstances().Count == 0)
		{
			ImGui.Text(" - No stories");
		}
	}

	private void DrawTelemetryData()
	{
		ImGuiEx.DrawObjectTable("ID_telemetry", StoryManager.GetTelemetry());
	}
}
