using System;
using UnityEngine;

// Token: 0x02001426 RID: 5158
public class DeconstructTool : FilteredDragTool
{
	// Token: 0x06006A81 RID: 27265 RVA: 0x000E5ECC File Offset: 0x000E40CC
	public static void DestroyInstance()
	{
		DeconstructTool.Instance = null;
	}

	// Token: 0x06006A82 RID: 27266 RVA: 0x000E5ED4 File Offset: 0x000E40D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DeconstructTool.Instance = this;
	}

	// Token: 0x06006A83 RID: 27267 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006A84 RID: 27268 RVA: 0x000E5CF4 File Offset: 0x000E3EF4
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06006A85 RID: 27269 RVA: 0x000E5CFB File Offset: 0x000E3EFB
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06006A86 RID: 27270 RVA: 0x000E5EE2 File Offset: 0x000E40E2
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.DeconstructCell(cell);
	}

	// Token: 0x06006A87 RID: 27271 RVA: 0x002DF3A0 File Offset: 0x002DD5A0
	public void DeconstructCell(int cell)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(-790448070, null);
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06006A88 RID: 27272 RVA: 0x000E5EEB File Offset: 0x000E40EB
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006A89 RID: 27273 RVA: 0x000E5F03 File Offset: 0x000E4103
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04005056 RID: 20566
	public static DeconstructTool Instance;
}
