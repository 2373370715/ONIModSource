using System;
using UnityEngine;

// Token: 0x02001437 RID: 5175
public class PlaceTool : DragTool
{
	// Token: 0x06006B30 RID: 27440 RVA: 0x000E66BB File Offset: 0x000E48BB
	public static void DestroyInstance()
	{
		PlaceTool.Instance = null;
	}

	// Token: 0x06006B31 RID: 27441 RVA: 0x000E66C3 File Offset: 0x000E48C3
	protected override void OnPrefabInit()
	{
		PlaceTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
	}

	// Token: 0x06006B32 RID: 27442 RVA: 0x002E1CB0 File Offset: 0x002DFEB0
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.visualizer = new GameObject("PlaceToolVisualizer");
		this.visualizer.SetActive(false);
		this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.source.kAnimName)
		};
		kbatchedAnimController.initialAnim = this.source.animName;
		this.visualizer.SetActive(true);
		this.ShowToolTip();
		base.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06006B33 RID: 27443 RVA: 0x002E1D98 File Offset: 0x002DFF98
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		UnityEngine.Object.Destroy(this.visualizer);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
		this.source = null;
		this.onPlacedCallback = null;
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006B34 RID: 27444 RVA: 0x000E66D7 File Offset: 0x000E48D7
	public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
	{
		this.source = source;
		this.onPlacedCallback = onPlacedCallback;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B35 RID: 27445 RVA: 0x002E1DF8 File Offset: 0x002DFFF8
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.visualizer == null)
		{
			return;
		}
		bool flag = false;
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			this.onPlacedCallback(this.source, cell);
			flag = true;
		}
		if (flag)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06006B36 RID: 27446 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06006B37 RID: 27447 RVA: 0x000E66F2 File Offset: 0x000E48F2
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06006B38 RID: 27448 RVA: 0x000E6704 File Offset: 0x000E4904
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06006B39 RID: 27449 RVA: 0x002E1E44 File Offset: 0x002E0044
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			component.TintColour = Color.white;
		}
		else
		{
			component.TintColour = Color.red;
		}
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006B3A RID: 27450 RVA: 0x002E1EB0 File Offset: 0x002E00B0
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06006B3B RID: 27451 RVA: 0x000E5C8E File Offset: 0x000E3E8E
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x040050B2 RID: 20658
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x040050B3 RID: 20659
	private Action<Placeable, int> onPlacedCallback;

	// Token: 0x040050B4 RID: 20660
	private Placeable source;

	// Token: 0x040050B5 RID: 20661
	private ToolTip tooltip;

	// Token: 0x040050B6 RID: 20662
	public static PlaceTool Instance;

	// Token: 0x040050B7 RID: 20663
	private bool active;
}
