using System;
using System.Collections.Generic;

public class DevPanelList
{
	private List<DevPanel> activePanels = new List<DevPanel>();

	private uint fallbackUniqueIdPostfixNumber = 300u;

	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return AddPanelFor(new T());
	}

	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		activePanels.Add(devPanel);
		return devPanel;
	}

	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel activePanel in activePanels)
		{
			if (activePanel.GetCurrentDevTool() is T val)
			{
				return val;
			}
		}
		return Option.None;
	}

	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		var (flag2, val2) = (Option<T>)(ref GetDevTool<T>());
		if (!flag2)
		{
			val2 = new T();
			AddPanelFor(val2);
		}
		return val2;
	}

	public void ClosePanel(DevPanel panel)
	{
		if (activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	public void Render()
	{
		if (activePanels.Count == 0)
		{
			return;
		}
		using ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate();
		for (int i = 0; i < activePanels.Count; i++)
		{
			DevPanel devPanel = activePanels[i];
			devPanel.RenderPanel();
			if (devPanel.isRequestingToClose)
			{
				pooledList.Add(devPanel);
			}
		}
		foreach (DevPanel item in pooledList)
		{
			ClosePanel(item);
		}
	}

	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber;
	}

	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		using HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate();
		foreach (DevPanel activePanel in activePanels)
		{
			if (!(activePanel.initialDevToolType != initialDevToolType))
			{
				pooledHashSet.Add(activePanel.idPostfixNumber);
			}
		}
		for (uint num = 0u; num < 100; num++)
		{
			if (!pooledHashSet.Contains(num))
			{
				return num;
			}
		}
		Debug.Assert(condition: false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
		return fallbackUniqueIdPostfixNumber++;
	}
}
