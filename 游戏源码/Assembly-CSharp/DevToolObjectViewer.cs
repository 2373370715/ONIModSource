using System;

// Token: 0x02000BBE RID: 3006
public class DevToolObjectViewer<T> : DevTool
{
	// Token: 0x06003993 RID: 14739 RVA: 0x000C53B8 File Offset: 0x000C35B8
	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		this.Name = typeof(T).Name;
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x002219F0 File Offset: 0x0021FBF0
	protected override void RenderTo(DevPanel panel)
	{
		T t = this.getValue();
		this.Name = t.GetType().Name;
		ImGuiEx.DrawObject(t, null);
	}

	// Token: 0x0400273C RID: 10044
	private Func<T> getValue;
}
