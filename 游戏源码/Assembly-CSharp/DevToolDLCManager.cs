using ImGuiNET;

public class DevToolDLCManager : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		string name = DistributionPlatform.Inst.Name;
		if (!DistributionPlatform.Initialized)
		{
			ImGui.Text("Failed to initialize " + name);
			return;
		}
		ImGui.Text("Active content letters: " + DlcManager.GetActiveContentLetters());
		ImGui.Separator();
		foreach (string rELEASED_VERSION in DlcManager.RELEASED_VERSIONS)
		{
			if (!rELEASED_VERSION.IsNullOrWhiteSpace())
			{
				ImGui.Text(rELEASED_VERSION);
				ImGui.SameLine();
				bool v = DlcManager.IsContentSubscribed(rELEASED_VERSION);
				if (ImGui.Checkbox("Enabled ", ref v))
				{
					DlcManager.ToggleDLC(rELEASED_VERSION);
				}
				ImGui.SameLine();
				bool v2 = DistributionPlatform.Inst.IsDLCSubscribed(rELEASED_VERSION);
				if (ImGui.Checkbox("Subscribed ", ref v2))
				{
					DistributionPlatform.Inst.ToggleDLCSubscription(rELEASED_VERSION);
				}
			}
		}
	}
}
