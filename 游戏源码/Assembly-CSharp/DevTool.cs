using System;
using ImGuiNET;

// Token: 0x02000B95 RID: 2965
public abstract class DevTool
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060038CD RID: 14541 RVA: 0x0021B9E8 File Offset: 0x00219BE8
	// (remove) Token: 0x060038CE RID: 14542 RVA: 0x0021BA20 File Offset: 0x00219C20
	public event System.Action OnInit;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060038CF RID: 14543 RVA: 0x0021BA58 File Offset: 0x00219C58
	// (remove) Token: 0x060038D0 RID: 14544 RVA: 0x0021BA90 File Offset: 0x00219C90
	public event System.Action OnUpdate;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060038D1 RID: 14545 RVA: 0x0021BAC8 File Offset: 0x00219CC8
	// (remove) Token: 0x060038D2 RID: 14546 RVA: 0x0021BB00 File Offset: 0x00219D00
	public event System.Action OnUninit;

	// Token: 0x060038D3 RID: 14547 RVA: 0x000C4C03 File Offset: 0x000C2E03
	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x000C4C17 File Offset: 0x000C2E17
	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x000C4C40 File Offset: 0x000C2E40
	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	// Token: 0x060038D6 RID: 14550
	protected abstract void RenderTo(DevPanel panel);

	// Token: 0x060038D7 RID: 14551 RVA: 0x000C4C49 File Offset: 0x000C2E49
	public void Internal_TryInit()
	{
		if (this.didInit)
		{
			return;
		}
		this.didInit = true;
		if (this.OnInit != null)
		{
			this.OnInit();
		}
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x000C4C6E File Offset: 0x000C2E6E
	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x000C4C83 File Offset: 0x000C2E83
	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	// Token: 0x040026B2 RID: 9906
	public string Name;

	// Token: 0x040026B3 RID: 9907
	public bool RequiresGameRunning;

	// Token: 0x040026B4 RID: 9908
	public bool isRequestingToClosePanel;

	// Token: 0x040026B5 RID: 9909
	public ImGuiWindowFlags drawFlags;

	// Token: 0x040026B9 RID: 9913
	private bool didInit;
}
