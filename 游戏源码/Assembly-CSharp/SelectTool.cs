using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001453 RID: 5203
public class SelectTool : InterfaceTool
{
	// Token: 0x06006BFC RID: 27644 RVA: 0x000E6F1A File Offset: 0x000E511A
	public static void DestroyInstance()
	{
		SelectTool.Instance = null;
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x002E4D18 File Offset: 0x002E2F18
	protected override void OnPrefabInit()
	{
		this.defaultLayerMask = (1 | LayerMask.GetMask(new string[]
		{
			"World",
			"Pickupable",
			"Place",
			"PlaceWithDepth",
			"BlockSelection",
			"Construction",
			"Selection"
		}));
		this.layerMask = this.defaultLayerMask;
		this.selectMarker = global::Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
		this.selectMarker.gameObject.SetActive(false);
		this.populateHitsList = true;
		SelectTool.Instance = this;
	}

	// Token: 0x06006BFE RID: 27646 RVA: 0x000E6F22 File Offset: 0x000E5122
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06006BFF RID: 27647 RVA: 0x000E6F46 File Offset: 0x000E5146
	public void SetLayerMask(int mask)
	{
		this.layerMask = mask;
		base.ClearHover();
		this.LateUpdate();
	}

	// Token: 0x06006C00 RID: 27648 RVA: 0x000E6F5B File Offset: 0x000E515B
	public void ClearLayerMask()
	{
		this.layerMask = this.defaultLayerMask;
	}

	// Token: 0x06006C01 RID: 27649 RVA: 0x000E6F69 File Offset: 0x000E5169
	public int GetDefaultLayerMask()
	{
		return this.defaultLayerMask;
	}

	// Token: 0x06006C02 RID: 27650 RVA: 0x000E6F71 File Offset: 0x000E5171
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x002E4DBC File Offset: 0x002E2FBC
	public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		if (selectable != null)
		{
			pos = selectable.transform.GetPosition();
		}
		pos.z = -40f;
		pos += offset;
		WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
		if (worldFromPosition != null)
		{
			CameraController.Instance.ActiveWorldStarWipe(worldFromPosition.id, pos, 10f, null);
			return;
		}
		DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x000E6F88 File Offset: 0x000E5188
	public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		this.Focus(pos, selectable, offset);
		this.Select(selectable, false);
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x000E6F9B File Offset: 0x000E519B
	public void SelectAndFocus(Vector3 pos, KSelectable selectable)
	{
		this.SelectAndFocus(pos, selectable, Vector3.zero);
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x000E6FAA File Offset: 0x000E51AA
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06006C07 RID: 27655 RVA: 0x000E6FD8 File Offset: 0x000E51D8
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x002E4E2C File Offset: 0x002E302C
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.previousSelection)
		{
			return;
		}
		this.previousSelection = new_selected;
		if (this.selected != null)
		{
			this.selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			SelectToolHoverTextCard component = base.GetComponent<SelectToolHoverTextCard>();
			if (component != null)
			{
				int num = component.currentSelectedSelectableIndex;
				int recentNumberOfDisplayedSelectables = component.recentNumberOfDisplayedSelectables;
				if (recentNumberOfDisplayedSelectables != 0)
				{
					num = (num + 1) % recentNumberOfDisplayedSelectables;
					if (!skipSound)
					{
						if (recentNumberOfDisplayedSelectables == 1)
						{
							KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty", false));
						}
						else
						{
							EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero, 1f);
							instance.setParameterByName("selection", (float)num, false);
							SoundEvent.EndOneShot(instance);
						}
						this.playedSoundThisFrame = true;
					}
				}
			}
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
			this.selectMarker.SetTargetTransform(gameObject.transform);
			this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
		}
		else if (this.selectMarker != null)
		{
			this.selectMarker.gameObject.SetActive(false);
		}
		this.selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x002E4F98 File Offset: 0x002E3198
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		KSelectable objectUnderCursor = base.GetObjectUnderCursor<KSelectable>(true, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, this.selected);
		this.selectedCell = Grid.PosToCell(cursor_pos);
		this.Select(objectUnderCursor, false);
		if (DevToolSimDebug.Instance != null)
		{
			DevToolSimDebug.Instance.SetCell(this.selectedCell);
		}
		if (DevToolNavGrid.Instance != null)
		{
			DevToolNavGrid.Instance.SetCell(this.selectedCell);
		}
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x000E6FF3 File Offset: 0x000E51F3
	public int GetSelectedCell()
	{
		return this.selectedCell;
	}

	// Token: 0x04005103 RID: 20739
	public KSelectable selected;

	// Token: 0x04005104 RID: 20740
	protected int cell_new;

	// Token: 0x04005105 RID: 20741
	private int selectedCell;

	// Token: 0x04005106 RID: 20742
	protected int defaultLayerMask;

	// Token: 0x04005107 RID: 20743
	public static SelectTool Instance;

	// Token: 0x04005108 RID: 20744
	private KSelectable delayedNextSelection;

	// Token: 0x04005109 RID: 20745
	private bool delayedSkipSound;

	// Token: 0x0400510A RID: 20746
	private KSelectable previousSelection;
}
