using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevPanel
{
	public readonly string uniquePanelId;

	public readonly DevPanelList manager;

	public readonly Type initialDevToolType;

	public readonly uint idPostfixNumber;

	private List<DevTool> devTools;

	private int currentDevToolIndex;

	public bool isRequestingToClose { get; private set; }

	public Option<(Vector2, ImGuiCond)> nextImGuiWindowPosition { get; private set; }

	public Option<(Vector2, ImGuiCond)> nextImGuiWindowSize { get; private set; }

	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		devTools = new List<DevTool>();
		devTools.Add(devTool);
		currentDevToolIndex = 0;
		initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(initialDevToolType, out uniquePanelId, out idPostfixNumber);
	}

	public void PushValue<T>(T value) where T : class
	{
		PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	public void PushValue<T>(Func<T> value)
	{
		PushDevTool(new DevToolObjectViewer<T>(value));
	}

	public void PushDevTool<T>() where T : DevTool, new()
	{
		PushDevTool(new T());
	}

	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			manager.AddPanelFor(devTool);
			return;
		}
		for (int num = devTools.Count - 1; num > currentDevToolIndex; num--)
		{
			devTools[num].Internal_Uninit();
			devTools.RemoveAt(num);
		}
		devTools.Add(devTool);
		currentDevToolIndex = devTools.Count - 1;
	}

	public bool NavGoBack()
	{
		Option<int> option = TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		currentDevToolIndex = option.Unwrap();
		return true;
	}

	public bool NavGoForward()
	{
		Option<int> option = TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		currentDevToolIndex = option.Unwrap();
		return true;
	}

	public DevTool GetCurrentDevTool()
	{
		return devTools[currentDevToolIndex];
	}

	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	public void RenderPanel()
	{
		DevTool currentDevTool = GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			isRequestingToClose = true;
			return;
		}
		ConfigureImGuiWindowFor(currentDevTool, out var drawFlags);
		currentDevTool.Internal_Update();
		bool p_open = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + uniquePanelId, ref p_open, drawFlags))
		{
			if (!p_open)
			{
				isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				DrawNavigation();
				ImGui.SameLine(0f, 20f);
				DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (GetCurrentDevTool().isRequestingToClosePanel)
		{
			isRequestingToClose = true;
		}
	}

	private void DrawNavigation()
	{
		Option<int> option = TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + devTools[option2.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go forward");
		}
	}

	private void DrawMenuBarContents()
	{
	}

	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags;
		if (nextImGuiWindowPosition.HasValue)
		{
			var (pos, cond) = nextImGuiWindowPosition.Value;
			ImGui.SetNextWindowPos(pos, cond);
			nextImGuiWindowPosition = default(Option<(Vector2, ImGuiCond)>);
		}
		if (nextImGuiWindowSize.HasValue)
		{
			Vector2 item = nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item);
			nextImGuiWindowSize = default(Option<(Vector2, ImGuiCond)>);
		}
	}

	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		nextImGuiWindowPosition = (position, condition);
	}

	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		nextImGuiWindowSize = (size, condition);
	}

	public void Close()
	{
		isRequestingToClose = true;
	}

	public void Internal_Uninit()
	{
		foreach (DevTool devTool in devTools)
		{
			devTool.Internal_Uninit();
		}
	}
}
