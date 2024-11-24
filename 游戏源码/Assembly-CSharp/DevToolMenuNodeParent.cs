using System.Collections.Generic;
using ImGuiNET;

public class DevToolMenuNodeParent : IMenuNode
{
	public string name;

	public List<IMenuNode> children;

	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		children = new List<IMenuNode>();
	}

	public void AddChild(IMenuNode menuNode)
	{
		children.Add(menuNode);
	}

	public string GetName()
	{
		return name;
	}

	public void Draw()
	{
		if (!ImGui.BeginMenu(name))
		{
			return;
		}
		foreach (IMenuNode child in children)
		{
			child.Draw();
		}
		ImGui.EndMenu();
	}
}
