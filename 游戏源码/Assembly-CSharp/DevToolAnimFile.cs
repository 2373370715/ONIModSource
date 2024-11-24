using System;

// Token: 0x02000B97 RID: 2967
public class DevToolAnimFile : DevTool
{
	// Token: 0x060038DE RID: 14558 RVA: 0x000C4CD3 File Offset: 0x000C2ED3
	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		this.Name = "Anim File: \"" + animFile.name + "\"";
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x0021BE24 File Offset: 0x0021A024
	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(this.animFile, null);
		ImGuiEx.DrawObject(this.animFile.GetData(), null);
	}

	// Token: 0x040026BA RID: 9914
	private KAnimFile animFile;
}
