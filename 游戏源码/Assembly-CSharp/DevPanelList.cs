using System;
using System.Collections.Generic;

// Token: 0x02000B94 RID: 2964
public class DevPanelList
{
	// Token: 0x060038C4 RID: 14532 RVA: 0x000C4B9F File Offset: 0x000C2D9F
	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return this.AddPanelFor(Activator.CreateInstance<T>());
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x0021B79C File Offset: 0x0021999C
	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		this.activePanels.Add(devPanel);
		return devPanel;
	}

	// Token: 0x060038C6 RID: 14534 RVA: 0x0021B7C0 File Offset: 0x002199C0
	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel devPanel in this.activePanels)
		{
			T t = devPanel.GetCurrentDevTool() as T;
			if (t != null)
			{
				return t;
			}
		}
		return Option.None;
	}

	// Token: 0x060038C7 RID: 14535 RVA: 0x0021B838 File Offset: 0x00219A38
	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		bool flag;
		T t;
		this.GetDevTool<T>().Deconstruct(out flag, out t);
		bool flag2 = flag;
		T t2 = t;
		if (!flag2)
		{
			t2 = Activator.CreateInstance<T>();
			this.AddPanelFor(t2);
		}
		return t2;
	}

	// Token: 0x060038C8 RID: 14536 RVA: 0x000C4BB1 File Offset: 0x000C2DB1
	public void ClosePanel(DevPanel panel)
	{
		if (this.activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x0021B870 File Offset: 0x00219A70
	public void Render()
	{
		if (this.activePanels.Count == 0)
		{
			return;
		}
		using (ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate())
		{
			for (int i = 0; i < this.activePanels.Count; i++)
			{
				DevPanel devPanel = this.activePanels[i];
				devPanel.RenderPanel();
				if (devPanel.isRequestingToClose)
				{
					pooledList.Add(devPanel);
				}
			}
			foreach (DevPanel panel in pooledList)
			{
				this.ClosePanel(panel);
			}
		}
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x000C4BC7 File Offset: 0x000C2DC7
	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = this.Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber.ToString();
	}

	// Token: 0x060038CB RID: 14539 RVA: 0x0021B924 File Offset: 0x00219B24
	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		uint result;
		using (HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate())
		{
			foreach (DevPanel devPanel in this.activePanels)
			{
				if (!(devPanel.initialDevToolType != initialDevToolType))
				{
					pooledHashSet.Add(devPanel.idPostfixNumber);
				}
			}
			for (uint num = 0U; num < 100U; num += 1U)
			{
				if (!pooledHashSet.Contains(num))
				{
					return num;
				}
			}
			Debug.Assert(false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
			uint num2 = this.fallbackUniqueIdPostfixNumber;
			this.fallbackUniqueIdPostfixNumber = num2 + 1U;
			result = num2;
		}
		return result;
	}

	// Token: 0x040026B0 RID: 9904
	private List<DevPanel> activePanels = new List<DevPanel>();

	// Token: 0x040026B1 RID: 9905
	private uint fallbackUniqueIdPostfixNumber = 300U;
}
