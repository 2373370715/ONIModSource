using System;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020013DE RID: 5086
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x0600684A RID: 26698 RVA: 0x000E44AC File Offset: 0x000E26AC
	public bool InPlanterBox
	{
		get
		{
			return this.isInPlanterBox;
		}
	}

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x0600684B RID: 26699 RVA: 0x000E44B4 File Offset: 0x000E26B4
	// (set) Token: 0x0600684C RID: 26700 RVA: 0x000E44BC File Offset: 0x000E26BC
	public bool MarkedForHarvest
	{
		get
		{
			return this.isMarkedForHarvest;
		}
		set
		{
			this.isMarkedForHarvest = value;
		}
	}

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x0600684D RID: 26701 RVA: 0x000E44C5 File Offset: 0x000E26C5
	public bool HarvestWhenReady
	{
		get
		{
			return this.harvestWhenReady;
		}
	}

	// Token: 0x0600684E RID: 26702 RVA: 0x000E44CD File Offset: 0x000E26CD
	protected HarvestDesignatable()
	{
		this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
	}

	// Token: 0x0600684F RID: 26703 RVA: 0x000E4500 File Offset: 0x000E2700
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
	}

	// Token: 0x06006850 RID: 26704 RVA: 0x002D69B4 File Offset: 0x002D4BB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		Components.HarvestDesignatables.Add(this);
		base.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
		base.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
		Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
		this.area = base.GetComponent<OccupyArea>();
	}

	// Token: 0x06006851 RID: 26705 RVA: 0x002D6A74 File Offset: 0x002D4C74
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HarvestDesignatables.Remove(this);
		this.DestroyOverlayIcon();
		Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
	}

	// Token: 0x06006852 RID: 26706 RVA: 0x000E4519 File Offset: 0x000E2719
	private void DestroyOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			UnityEngine.Object.Destroy(this.HarvestWhenReadyOverlayIcon.gameObject);
			this.HarvestWhenReadyOverlayIcon = null;
		}
	}

	// Token: 0x06006853 RID: 26707 RVA: 0x002D6AF8 File Offset: 0x002D4CF8
	private void CreateOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			return;
		}
		if (base.GetComponent<AttackableBase>() == null)
		{
			this.HarvestWhenReadyOverlayIcon = Util.KInstantiate(Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas, null).GetComponent<RectTransform>();
			Extents extents = base.GetComponent<OccupyArea>().GetExtents();
			Vector3 position;
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Hanging))
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)(extents.y + extents.height)) + this.iconOffset;
			}
			else
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)extents.y) + this.iconOffset;
			}
			this.HarvestWhenReadyOverlayIcon.transform.SetPosition(position);
			this.RefreshOverlayIcon(null);
		}
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x000E4540 File Offset: 0x000E2740
	private void OnDisableOverlay(object data)
	{
		this.DestroyOverlayIcon();
	}

	// Token: 0x06006855 RID: 26709 RVA: 0x000E4548 File Offset: 0x000E2748
	private void OnEnableOverlay(object data)
	{
		if ((HashedString)data == OverlayModes.Harvest.ID)
		{
			this.CreateOverlayIcon();
			return;
		}
		this.DestroyOverlayIcon();
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x002D6BF0 File Offset: 0x002D4DF0
	private void RefreshOverlayIcon(object data = null)
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			if ((Grid.IsVisible(Grid.PosToCell(base.gameObject)) && base.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) || (CameraController.Instance != null && CameraController.Instance.FreeCameraEnabled))
			{
				if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
				{
					this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
				}
			}
			else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
			{
				this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
			}
			HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
			if (this.harvestWhenReady)
			{
				Image image = (Image)component.GetReference("On");
				image.gameObject.SetActive(true);
				image.color = GlobalAssets.Instance.colorSet.harvestEnabled;
				component.GetReference("Off").gameObject.SetActive(false);
				return;
			}
			component.GetReference("On").gameObject.SetActive(false);
			Image image2 = (Image)component.GetReference("Off");
			image2.gameObject.SetActive(true);
			image2.color = GlobalAssets.Instance.colorSet.harvestDisabled;
		}
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x002D6D44 File Offset: 0x002D4F44
	public bool CanBeHarvested()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		return !(component != null) || component.CanBeHarvested;
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x000E4569 File Offset: 0x000E2769
	public void SetInPlanterBox(bool state)
	{
		if (state)
		{
			if (!this.isInPlanterBox)
			{
				this.isInPlanterBox = true;
				this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
				return;
			}
		}
		else
		{
			this.isInPlanterBox = false;
		}
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x002D6D6C File Offset: 0x002D4F6C
	public void SetHarvestWhenReady(bool state)
	{
		this.harvestWhenReady = state;
		if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		if (this.isMarkedForHarvest && !this.harvestWhenReady)
		{
			this.OnCancel(null);
			if (this.CanBeHarvested() && this.isInPlanterBox)
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		base.Trigger(-266953818, null);
		this.RefreshOverlayIcon(null);
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnCancel(object data = null)
	{
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x002D6DF4 File Offset: 0x002D4FF4
	public virtual void MarkForHarvest()
	{
		if (!this.CanBeHarvested())
		{
			return;
		}
		this.isMarkedForHarvest = true;
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.OnMarkedForHarvest();
		}
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x000E4591 File Offset: 0x000E2791
	protected virtual void OnClickHarvestWhenReady()
	{
		this.SetHarvestWhenReady(true);
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x002D6E28 File Offset: 0x002D5028
	protected virtual void OnClickCancelHarvestWhenReady()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.Trigger(2127324410, null);
		}
		this.SetHarvestWhenReady(false);
	}

	// Token: 0x0600685E RID: 26718 RVA: 0x002D6E58 File Offset: 0x002D5058
	public virtual void OnRefreshUserMenu(object data)
	{
		if (this.showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickCancelHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x04004EA9 RID: 20137
	public Vector2 iconOffset = Vector2.zero;

	// Token: 0x04004EAA RID: 20138
	public bool defaultHarvestStateWhenPlanted = true;

	// Token: 0x04004EAB RID: 20139
	public OccupyArea area;

	// Token: 0x04004EAC RID: 20140
	[Serialize]
	protected bool isMarkedForHarvest;

	// Token: 0x04004EAD RID: 20141
	[Serialize]
	private bool isInPlanterBox;

	// Token: 0x04004EAE RID: 20142
	public bool showUserMenuButtons = true;

	// Token: 0x04004EAF RID: 20143
	[Serialize]
	protected bool harvestWhenReady;

	// Token: 0x04004EB0 RID: 20144
	public RectTransform HarvestWhenReadyOverlayIcon;

	// Token: 0x04004EB1 RID: 20145
	private Action<object> onEnableOverlayDelegate;

	// Token: 0x04004EB2 RID: 20146
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04004EB3 RID: 20147
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04004EB4 RID: 20148
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.SetInPlanterBox(true);
	});
}
