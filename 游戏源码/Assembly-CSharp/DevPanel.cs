using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
public class DevPanel
{
	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060038AB RID: 14507 RVA: 0x000C4B00 File Offset: 0x000C2D00
	// (set) Token: 0x060038AC RID: 14508 RVA: 0x000C4B08 File Offset: 0x000C2D08
	public bool isRequestingToClose { get; private set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060038AD RID: 14509 RVA: 0x000C4B11 File Offset: 0x000C2D11
	// (set) Token: 0x060038AE RID: 14510 RVA: 0x000C4B19 File Offset: 0x000C2D19
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060038AF RID: 14511 RVA: 0x000C4B22 File Offset: 0x000C2D22
	// (set) Token: 0x060038B0 RID: 14512 RVA: 0x000C4B2A File Offset: 0x000C2D2A
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	// Token: 0x060038B1 RID: 14513 RVA: 0x0021B354 File Offset: 0x00219554
	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x0021B3B0 File Offset: 0x002195B0
	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x000C4B33 File Offset: 0x000C2D33
	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x000C4B41 File Offset: 0x000C2D41
	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x0021B3E4 File Offset: 0x002195E4
	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.manager.AddPanelFor(devTool);
			return;
		}
		for (int i = this.devTools.Count - 1; i > this.currentDevToolIndex; i--)
		{
			this.devTools[i].Internal_Uninit();
			this.devTools.RemoveAt(i);
		}
		this.devTools.Add(devTool);
		this.currentDevToolIndex = this.devTools.Count - 1;
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x0021B464 File Offset: 0x00219664
	public bool NavGoBack()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x0021B494 File Offset: 0x00219694
	public bool NavGoForward()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x000C4B53 File Offset: 0x000C2D53
	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x0021B4C4 File Offset: 0x002196C4
	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = this.currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= this.devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x0021B508 File Offset: 0x00219708
	public void RenderPanel()
	{
		DevTool currentDevTool = this.GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
			return;
		}
		ImGuiWindowFlags flags;
		this.ConfigureImGuiWindowFor(currentDevTool, out flags);
		currentDevTool.Internal_Update();
		bool flag = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + this.uniquePanelId, ref flag, flags))
		{
			if (!flag)
			{
				this.isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				this.DrawNavigation();
				ImGui.SameLine(0f, 20f);
				this.DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (this.GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (this.GetCurrentDevTool().isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x0021B5D0 File Offset: 0x002197D0
	private void DrawNavigation()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			this.currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + this.devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = this.TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			this.currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + this.devTools[option2.Unwrap()].Name);
			return;
		}
		ImGuiEx.TooltipForPrevious("Go forward");
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void DrawMenuBarContents()
	{
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x0021B6B4 File Offset: 0x002198B4
	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = (ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags);
		if (this.nextImGuiWindowPosition.HasValue)
		{
			ValueTuple<Vector2, ImGuiCond> value = this.nextImGuiWindowPosition.Value;
			Vector2 item = value.Item1;
			ImGuiCond item2 = value.Item2;
			ImGui.SetNextWindowPos(item, item2);
			this.nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
		if (this.nextImGuiWindowSize.HasValue)
		{
			Vector2 item3 = this.nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item3);
			this.nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x000C4B66 File Offset: 0x000C2D66
	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x000C4B7A File Offset: 0x000C2D7A
	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x000C4B8E File Offset: 0x000C2D8E
	public void Close()
	{
		this.isRequestingToClose = true;
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x0021B74C File Offset: 0x0021994C
	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	// Token: 0x040026A9 RID: 9897
	public readonly string uniquePanelId;

	// Token: 0x040026AA RID: 9898
	public readonly DevPanelList manager;

	// Token: 0x040026AB RID: 9899
	public readonly Type initialDevToolType;

	// Token: 0x040026AC RID: 9900
	public readonly uint idPostfixNumber;

	// Token: 0x040026AD RID: 9901
	private List<DevTool> devTools;

	// Token: 0x040026AE RID: 9902
	private int currentDevToolIndex;
}
