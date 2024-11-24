using System;
using ImGuiNET;

public abstract class DevTool
{
	public string Name;

	public bool RequiresGameRunning;

	public bool isRequestingToClosePanel;

	public ImGuiWindowFlags drawFlags;

	private bool didInit;

	public event System.Action OnInit;

	public event System.Action OnUpdate;

	public event System.Action OnUninit;

	public DevTool()
	{
		Name = DevToolUtil.GenerateDevToolName(this);
	}

	public void DoImGui(DevPanel panel)
	{
		if (RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
		}
		else
		{
			RenderTo(panel);
		}
	}

	public void ClosePanel()
	{
		isRequestingToClosePanel = true;
	}

	protected abstract void RenderTo(DevPanel panel);

	public void Internal_TryInit()
	{
		if (!didInit)
		{
			didInit = true;
			if (this.OnInit != null)
			{
				this.OnInit();
			}
		}
	}

	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}
}
