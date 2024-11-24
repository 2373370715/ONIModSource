using System;
using ImGuiNET;

// Token: 0x02000BC3 RID: 3011
public class DevToolSaveGameInfo : DevTool
{
	// Token: 0x0600399F RID: 14751 RVA: 0x002221C0 File Offset: 0x002203C0
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
		ImGui.Text("Content Enabled: ");
		foreach (string text in SaveLoader.Instance.GameInfo.dlcIds)
		{
			string str = (text == "") ? "VANILLA_ID" : text;
			ImGui.Text(" - " + str);
		}
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref this.clSearch, 10U);
		ImGui.PopItemWidth();
		foreach (uint num in Game.Instance.changelistsPlayedOn)
		{
			if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
			{
				ImGui.Text(num.ToString());
			}
		}
		ImGui.NewLine();
	}

	// Token: 0x0400273E RID: 10046
	private string clSearch = "";
}
