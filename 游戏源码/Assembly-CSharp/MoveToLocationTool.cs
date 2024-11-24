using System;
using UnityEngine;

// Token: 0x02001436 RID: 5174
public class MoveToLocationTool : InterfaceTool
{
	// Token: 0x06006B23 RID: 27427 RVA: 0x000E6619 File Offset: 0x000E4819
	public static void DestroyInstance()
	{
		MoveToLocationTool.Instance = null;
	}

	// Token: 0x06006B24 RID: 27428 RVA: 0x000E6621 File Offset: 0x000E4821
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MoveToLocationTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x000E6642 File Offset: 0x000E4842
	public void Activate(Navigator navigator)
	{
		this.targetNavigator = navigator;
		this.targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B26 RID: 27430 RVA: 0x000E665D File Offset: 0x000E485D
	public void Activate(Movable movable)
	{
		this.targetNavigator = null;
		this.targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x002E1B00 File Offset: 0x002DFD00
	public bool CanMoveTo(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			return this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell);
		}
		return this.targetMovable != null && this.targetMovable.CanMoveTo(target_cell);
	}

	// Token: 0x06006B28 RID: 27432 RVA: 0x002E1B54 File Offset: 0x002DFD54
	private void SetMoveToLocation(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
			if (smi != null)
			{
				smi.MoveToLocation(target_cell);
				return;
			}
		}
		else if (this.targetMovable != null)
		{
			this.targetMovable.MoveToLocation(target_cell);
		}
	}

	// Token: 0x06006B29 RID: 27433 RVA: 0x000E6678 File Offset: 0x000E4878
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.visualizer.gameObject.SetActive(true);
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x002E1BA0 File Offset: 0x002DFDA0
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (this.targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
		}
		this.visualizer.gameObject.SetActive(false);
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x002E1BF8 File Offset: 0x002DFDF8
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.targetNavigator != null || this.targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (this.CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
				this.SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
				return;
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x002E1C6C File Offset: 0x002DFE6C
	private void RefreshColor()
	{
		Color white = new Color(0.91f, 0.21f, 0.2f);
		if (this.CanMoveTo(DebugHandler.GetMouseCell()))
		{
			white = Color.white;
		}
		this.SetColor(this.visualizer, white);
	}

	// Token: 0x06006B2D RID: 27437 RVA: 0x000E6691 File Offset: 0x000E4891
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.RefreshColor();
	}

	// Token: 0x06006B2E RID: 27438 RVA: 0x000E66A0 File Offset: 0x000E48A0
	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}

	// Token: 0x040050AF RID: 20655
	public static MoveToLocationTool Instance;

	// Token: 0x040050B0 RID: 20656
	private Navigator targetNavigator;

	// Token: 0x040050B1 RID: 20657
	private Movable targetMovable;
}
