using ImGuiNET;

public class DevToolSaveGameInfo : DevTool
{
	private string clSearch = "";

	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed);
		ImGui.Text("Content Enabled: ");
		foreach (string dlcId in SaveLoader.Instance.GameInfo.dlcIds)
		{
			string text = ((dlcId == "") ? "VANILLA_ID" : dlcId);
			ImGui.Text(" - " + text);
		}
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref clSearch, 10u);
		ImGui.PopItemWidth();
		foreach (uint item in Game.Instance.changelistsPlayedOn)
		{
			if (clSearch.IsNullOrWhiteSpace() || item.ToString().Contains(clSearch))
			{
				ImGui.Text(item.ToString());
			}
		}
		ImGui.NewLine();
	}
}
