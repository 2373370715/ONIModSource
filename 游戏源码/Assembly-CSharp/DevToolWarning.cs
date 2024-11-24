using System;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000BE3 RID: 3043
public class DevToolWarning
{
	// Token: 0x06003A3D RID: 14909 RVA: 0x000C59AC File Offset: 0x000C3BAC
	public DevToolWarning()
	{
		this.Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x000C59C4 File Offset: 0x000C3BC4
	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00226A7C File Offset: 0x00224C7C
	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			if (!isOpen)
			{
				ImGui.End();
				return;
			}
			ImGui.SetWindowSize(new Vector2(500f, 250f));
			ImGui.TextWrapped(UI.FRONTEND.DEVTOOLS.WARNING);
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
			if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
			{
				if (this.showAgain)
				{
					KPlayerPrefs.SetInt("ShowDevtools", 1);
				}
				DevToolManager.Instance.UserAcceptedWarning = true;
				isOpen = false;
			}
			ImGui.End();
		}
	}

	// Token: 0x040027B2 RID: 10162
	private bool showAgain;

	// Token: 0x040027B3 RID: 10163
	public string Name;

	// Token: 0x040027B4 RID: 10164
	public bool ShouldDrawWindow;
}
