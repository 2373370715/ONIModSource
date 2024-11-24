using System;
using UnityEngine;

// Token: 0x02001422 RID: 5154
public class CopySettingsTool : DragTool
{
	// Token: 0x06006A6B RID: 27243 RVA: 0x000E5E16 File Offset: 0x000E4016
	public static void DestroyInstance()
	{
		CopySettingsTool.Instance = null;
	}

	// Token: 0x06006A6C RID: 27244 RVA: 0x000E5E1E File Offset: 0x000E401E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CopySettingsTool.Instance = this;
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006A6E RID: 27246 RVA: 0x000E5E2C File Offset: 0x000E402C
	public void SetSourceObject(GameObject sourceGameObject)
	{
		this.sourceGameObject = sourceGameObject;
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x000E5E35 File Offset: 0x000E4035
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.sourceGameObject == null)
		{
			return;
		}
		if (Grid.IsValidCell(cell))
		{
			CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
		}
	}

	// Token: 0x06006A70 RID: 27248 RVA: 0x000E5E5B File Offset: 0x000E405B
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
	}

	// Token: 0x06006A71 RID: 27249 RVA: 0x000E5E63 File Offset: 0x000E4063
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.sourceGameObject = null;
	}

	// Token: 0x0400503F RID: 20543
	public static CopySettingsTool Instance;

	// Token: 0x04005040 RID: 20544
	public GameObject Placer;

	// Token: 0x04005041 RID: 20545
	private GameObject sourceGameObject;
}
