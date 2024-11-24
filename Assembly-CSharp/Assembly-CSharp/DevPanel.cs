﻿using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevPanel
{
			public bool isRequestingToClose { get; private set; }

			public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

			public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

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

	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

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

	private void DrawMenuBarContents()
	{
	}

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

	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	public void Close()
	{
		this.isRequestingToClose = true;
	}

	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	public readonly string uniquePanelId;

	public readonly DevPanelList manager;

	public readonly Type initialDevToolType;

	public readonly uint idPostfixNumber;

	private List<DevTool> devTools;

	private int currentDevToolIndex;
}
