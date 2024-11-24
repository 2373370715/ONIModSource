using System;

// Token: 0x02000BBB RID: 3003
public class DevToolMenuNodeAction : IMenuNode
{
	// Token: 0x06003983 RID: 14723 RVA: 0x000C5324 File Offset: 0x000C3524
	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x000C533A File Offset: 0x000C353A
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x000C5342 File Offset: 0x000C3542
	public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

	// Token: 0x0400272C RID: 10028
	public string name;

	// Token: 0x0400272D RID: 10029
	public System.Action onClickFn;

	// Token: 0x0400272E RID: 10030
	public Func<bool> isEnabledFn;
}
