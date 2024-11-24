using System;
using ImGuiNET;

// Token: 0x02000BB1 RID: 2993
public class DevToolEntity_SearchGameObjects : DevTool
{
	// Token: 0x06003952 RID: 14674 RVA: 0x000C51C0 File Offset: 0x000C33C0
	public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x000C51CF File Offset: 0x000C33CF
	protected override void RenderTo(DevPanel panel)
	{
		ImGui.Text("Not implemented yet");
	}

	// Token: 0x040026F0 RID: 9968
	private Action<DevToolEntityTarget> onSelectionMadeFn;
}
