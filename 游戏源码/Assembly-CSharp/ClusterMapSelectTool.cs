using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001420 RID: 5152
public class ClusterMapSelectTool : InterfaceTool
{
	// Token: 0x06006A5B RID: 27227 RVA: 0x000E5D34 File Offset: 0x000E3F34
	public static void DestroyInstance()
	{
		ClusterMapSelectTool.Instance = null;
	}

	// Token: 0x06006A5C RID: 27228 RVA: 0x000E5D3C File Offset: 0x000E3F3C
	protected override void OnPrefabInit()
	{
		ClusterMapSelectTool.Instance = this;
	}

	// Token: 0x06006A5D RID: 27229 RVA: 0x000E5D44 File Offset: 0x000E3F44
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06006A5E RID: 27230 RVA: 0x000E5D68 File Offset: 0x000E3F68
	public KSelectable GetSelected()
	{
		return this.m_selected;
	}

	// Token: 0x06006A5F RID: 27231 RVA: 0x000E5D70 File Offset: 0x000E3F70
	public override bool ShowHoverUI()
	{
		return ClusterMapScreen.Instance.HasCurrentHover();
	}

	// Token: 0x06006A60 RID: 27232 RVA: 0x000E5D7C File Offset: 0x000E3F7C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06006A61 RID: 27233 RVA: 0x002DE9AC File Offset: 0x002DCBAC
	private void UpdateHoveredSelectables()
	{
		this.m_hoveredSelectables.Clear();
		if (ClusterMapScreen.Instance.HasCurrentHover())
		{
			AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
			List<KSelectable> collection = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation)
			select entity.GetComponent<KSelectable>() into selectable
			where selectable != null && selectable.IsSelectable
			select selectable).ToList<KSelectable>();
			this.m_hoveredSelectables.AddRange(collection);
		}
	}

	// Token: 0x06006A62 RID: 27234 RVA: 0x002DEA40 File Offset: 0x002DCC40
	public override void LateUpdate()
	{
		this.UpdateHoveredSelectables();
		KSelectable kselectable = (this.m_hoveredSelectables.Count > 0) ? this.m_hoveredSelectables[0] : null;
		base.UpdateHoverElements(this.m_hoveredSelectables);
		if (!this.hasFocus)
		{
			base.ClearHover();
		}
		else if (kselectable != this.hover)
		{
			base.ClearHover();
			this.hover = kselectable;
			if (kselectable != null)
			{
				Game.Instance.Trigger(2095258329, kselectable.gameObject);
				kselectable.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x000E5D93 File Offset: 0x000E3F93
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x000E5DC1 File Offset: 0x000E3FC1
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x002DEAE4 File Offset: 0x002DCCE4
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.m_selected)
		{
			return;
		}
		if (this.m_selected != null)
		{
			this.m_selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == -1)
		{
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
		}
		this.m_selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x04005037 RID: 20535
	private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();

	// Token: 0x04005038 RID: 20536
	private KSelectable m_selected;

	// Token: 0x04005039 RID: 20537
	public static ClusterMapSelectTool Instance;

	// Token: 0x0400503A RID: 20538
	private KSelectable delayedNextSelection;

	// Token: 0x0400503B RID: 20539
	private bool delayedSkipSound;
}
