using System;
using UnityEngine;

// Token: 0x02001438 RID: 5176
public class PrebuildTool : InterfaceTool
{
	// Token: 0x06006B3D RID: 27453 RVA: 0x000E6716 File Offset: 0x000E4916
	public static void DestroyInstance()
	{
		PrebuildTool.Instance = null;
	}

	// Token: 0x06006B3E RID: 27454 RVA: 0x000E671E File Offset: 0x000E491E
	protected override void OnPrefabInit()
	{
		PrebuildTool.Instance = this;
	}

	// Token: 0x06006B3F RID: 27455 RVA: 0x000E6726 File Offset: 0x000E4926
	protected override void OnActivateTool()
	{
		this.viewMode = this.def.ViewMode;
		base.OnActivateTool();
	}

	// Token: 0x06006B40 RID: 27456 RVA: 0x000E673F File Offset: 0x000E493F
	public void Activate(BuildingDef def, string errorMessage)
	{
		this.def = def;
		PlayerController.Instance.ActivateTool(this);
		PrebuildToolHoverTextCard component = base.GetComponent<PrebuildToolHoverTextCard>();
		component.errorMessage = errorMessage;
		component.currentDef = def;
	}

	// Token: 0x06006B41 RID: 27457 RVA: 0x000E6766 File Offset: 0x000E4966
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		UISounds.PlaySound(UISounds.Sound.Negative);
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x040050B8 RID: 20664
	public static PrebuildTool Instance;

	// Token: 0x040050B9 RID: 20665
	private BuildingDef def;
}
