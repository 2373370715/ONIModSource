using System;
using System.IO;
using ImGuiNET;

public class DevToolMenuNodeList
{
	private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");

	public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
	{
		string[] array = Path.GetDirectoryName(childPath).Split('/');
		string text = "";
		DevToolMenuNodeParent devToolMenuNodeParent = root;
		string[] array2 = array;
		foreach (string split in array2)
		{
			text += devToolMenuNodeParent.GetName();
			IMenuNode menuNode = devToolMenuNodeParent.children.Find((IMenuNode x) => x.GetName() == split);
			DevToolMenuNodeParent devToolMenuNodeParent2;
			if (menuNode != null)
			{
				devToolMenuNodeParent2 = (menuNode as DevToolMenuNodeParent) ?? throw new Exception("Conflict! Both a leaf and parent node exist at path: " + text);
			}
			else
			{
				devToolMenuNodeParent2 = new DevToolMenuNodeParent(split);
				devToolMenuNodeParent.AddChild(devToolMenuNodeParent2);
			}
			devToolMenuNodeParent = devToolMenuNodeParent2;
		}
		return devToolMenuNodeParent;
	}

	public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
	{
		DevToolMenuNodeAction devToolMenuNodeAction = new DevToolMenuNodeAction(Path.GetFileName(path), onClickFn);
		AddOrGetParentFor(path).AddChild(devToolMenuNodeAction);
		return devToolMenuNodeAction;
	}

	public void Draw()
	{
		foreach (IMenuNode child in root.children)
		{
			child.Draw();
		}
	}

	public void DrawFull()
	{
		if (ImGui.BeginMainMenuBar())
		{
			Draw();
			ImGui.EndMainMenuBar();
		}
	}
}
