using System;
using ImGuiNET;

// Token: 0x02000BBF RID: 3007
public class DevToolPOITechUnlocks : DevTool
{
	// Token: 0x06003996 RID: 14742 RVA: 0x00221A38 File Offset: 0x0021FC38
	protected override void RenderTo(DevPanel panel)
	{
		if (Research.Instance == null)
		{
			return;
		}
		foreach (TechItem techItem in Db.Get().TechItems.resources)
		{
			if (techItem.isPOIUnlock)
			{
				ImGui.Text(techItem.Id);
				ImGui.SameLine();
				bool flag = techItem.IsComplete();
				if (ImGui.Checkbox("Unlocked ", ref flag))
				{
					techItem.POIUnlocked();
				}
			}
		}
	}
}
