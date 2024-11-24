using ImGuiNET;
using STRINGS;
using UnityEngine;

public class DevToolWarning
{
	private bool showAgain;

	public string Name;

	public bool ShouldDrawWindow;

	public DevToolWarning()
	{
		Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(Name, ref ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (!ImGui.Begin(Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			return;
		}
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
		ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref showAgain);
		if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
		{
			if (showAgain)
			{
				KPlayerPrefs.SetInt("ShowDevtools", 1);
			}
			DevToolManager.Instance.UserAcceptedWarning = true;
			isOpen = false;
		}
		ImGui.End();
	}
}
