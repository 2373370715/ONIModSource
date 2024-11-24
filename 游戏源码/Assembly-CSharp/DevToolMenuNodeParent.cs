using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000BBA RID: 3002
public class DevToolMenuNodeParent : IMenuNode
{
	// Token: 0x0600397F RID: 14719 RVA: 0x000C52F4 File Offset: 0x000C34F4
	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		this.children = new List<IMenuNode>();
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x000C530E File Offset: 0x000C350E
	public void AddChild(IMenuNode menuNode)
	{
		this.children.Add(menuNode);
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x000C531C File Offset: 0x000C351C
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x002211A0 File Offset: 0x0021F3A0
	public void Draw()
	{
		if (ImGui.BeginMenu(this.name))
		{
			foreach (IMenuNode menuNode in this.children)
			{
				menuNode.Draw();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x0400272A RID: 10026
	public string name;

	// Token: 0x0400272B RID: 10027
	public List<IMenuNode> children;
}
