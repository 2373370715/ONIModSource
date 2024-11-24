using System;
using UnityEngine;

// Token: 0x0200141F RID: 5151
public class ClearTool : DragTool
{
	// Token: 0x06006A54 RID: 27220 RVA: 0x000E5D0A File Offset: 0x000E3F0A
	public static void DestroyInstance()
	{
		ClearTool.Instance = null;
	}

	// Token: 0x06006A55 RID: 27221 RVA: 0x000E5D12 File Offset: 0x000E3F12
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClearTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
	}

	// Token: 0x06006A56 RID: 27222 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006A57 RID: 27223 RVA: 0x002DE910 File Offset: 0x002DCB10
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject == null)
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && gameObject2.GetComponent<Clearable>().isClearable)
			{
				gameObject2.GetComponent<Clearable>().MarkForClear(false, false);
				Prioritizable component = gameObject2.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}
	}

	// Token: 0x06006A58 RID: 27224 RVA: 0x000E57E0 File Offset: 0x000E39E0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006A59 RID: 27225 RVA: 0x000E57F8 File Offset: 0x000E39F8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04005036 RID: 20534
	public static ClearTool Instance;
}
