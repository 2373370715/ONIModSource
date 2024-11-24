using ImGuiNET;

public class DevToolPOITechUnlocks : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		if (Research.Instance == null)
		{
			return;
		}
		foreach (TechItem resource in Db.Get().TechItems.resources)
		{
			if (resource.isPOIUnlock)
			{
				ImGui.Text(resource.Id);
				ImGui.SameLine();
				bool v = resource.IsComplete();
				if (ImGui.Checkbox("Unlocked ", ref v))
				{
					resource.POIUnlocked();
				}
			}
		}
	}
}
