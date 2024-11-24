using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001B3D RID: 6973
public class RootMenu : KScreen
{
	// Token: 0x0600924C RID: 37452 RVA: 0x000FF8CA File Offset: 0x000FDACA
	public static void DestroyInstance()
	{
		RootMenu.Instance = null;
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x0600924D RID: 37453 RVA: 0x000FF8D2 File Offset: 0x000FDAD2
	// (set) Token: 0x0600924E RID: 37454 RVA: 0x000FF8D9 File Offset: 0x000FDAD9
	public static RootMenu Instance { get; private set; }

	// Token: 0x0600924F RID: 37455 RVA: 0x000FF8E1 File Offset: 0x000FDAE1
	public override float GetSortKey()
	{
		return -1f;
	}

	// Token: 0x06009250 RID: 37456 RVA: 0x003866CC File Offset: 0x003848CC
	protected override void OnPrefabInit()
	{
		RootMenu.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		base.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
		base.OnPrefabInit();
	}

	// Token: 0x06009251 RID: 37457 RVA: 0x0038674C File Offset: 0x0038494C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, base.gameObject, true).GetComponent<DetailsScreen>();
		this.detailsScreen.gameObject.SetActive(true);
		this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
		this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
		this.detailsScreen.gameObject.SetActive(false);
		this.userMenu.gameObject.SetActive(false);
	}

	// Token: 0x06009252 RID: 37458 RVA: 0x000FF8E8 File Offset: 0x000FDAE8
	private void OnClickCommon()
	{
		this.CloseSubMenus();
	}

	// Token: 0x06009253 RID: 37459 RVA: 0x000FF8F0 File Offset: 0x000FDAF0
	public void AddSubMenu(KScreen sub_menu)
	{
		if (sub_menu.activateOnSpawn)
		{
			sub_menu.Show(true);
		}
		this.subMenus.Add(sub_menu);
	}

	// Token: 0x06009254 RID: 37460 RVA: 0x000FF90D File Offset: 0x000FDB0D
	public void RemoveSubMenu(KScreen sub_menu)
	{
		this.subMenus.Remove(sub_menu);
	}

	// Token: 0x06009255 RID: 37461 RVA: 0x003867E8 File Offset: 0x003849E8
	private void CloseSubMenus()
	{
		foreach (KScreen kscreen in this.subMenus)
		{
			if (kscreen != null)
			{
				if (kscreen.activateOnSpawn)
				{
					kscreen.gameObject.SetActive(false);
				}
				else
				{
					kscreen.Deactivate();
				}
			}
		}
		this.subMenus.Clear();
	}

	// Token: 0x06009256 RID: 37462 RVA: 0x00386864 File Offset: 0x00384A64
	private void OnSelectObject(object data)
	{
		GameObject gameObject = (GameObject)data;
		bool flag = false;
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && !component.IsInitialized())
			{
				return;
			}
			flag = (component != null || CellSelectionObject.IsSelectionObject(gameObject));
		}
		if (gameObject != this.selectedGO)
		{
			if (this.selectedGO != null)
			{
				this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
			}
			this.selectedGO = null;
			this.CloseSubMenus();
			if (flag)
			{
				this.selectedGO = gameObject;
				this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
				this.AddSubMenu(this.detailsScreen);
				this.AddSubMenu(this.userMenu);
			}
			this.userMenu.SetSelected(this.selectedGO);
		}
		this.Refresh();
	}

	// Token: 0x06009257 RID: 37463 RVA: 0x000FF91C File Offset: 0x000FDB1C
	public void TriggerRefresh(object obj)
	{
		this.Refresh();
	}

	// Token: 0x06009258 RID: 37464 RVA: 0x000FF924 File Offset: 0x000FDB24
	public void Refresh()
	{
		if (this.selectedGO == null)
		{
			return;
		}
		this.detailsScreen.Refresh(this.selectedGO);
		this.userMenu.Refresh(this.selectedGO);
	}

	// Token: 0x06009259 RID: 37465 RVA: 0x00386950 File Offset: 0x00384B50
	private void OnBuildingStatechanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == this.selectedGO)
		{
			this.OnSelectObject(gameObject);
		}
	}

	// Token: 0x0600925A RID: 37466 RVA: 0x0038697C File Offset: 0x00384B7C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && e.TryConsume(global::Action.Escape) && SelectTool.Instance.enabled)
		{
			if (!this.canTogglePauseScreen)
			{
				return;
			}
			if (this.AreSubMenusOpen())
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
				this.CloseSubMenus();
				SelectTool.Instance.Select(null, false);
			}
			else if (e.IsAction(global::Action.Escape))
			{
				if (!SelectTool.Instance.enabled)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
				}
				if (PlayerController.Instance.IsUsingDefaultTool())
				{
					if (SelectTool.Instance.selected != null)
					{
						SelectTool.Instance.Select(null, false);
					}
					else
					{
						CameraController.Instance.ForcePanningState(false);
						this.TogglePauseScreen();
					}
				}
				else
				{
					Game.Instance.Trigger(288942073, null);
				}
				ToolMenu.Instance.ClearSelection();
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600925B RID: 37467 RVA: 0x000FF957 File Offset: 0x000FDB57
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		if (!e.Consumed && e.TryConsume(global::Action.AlternateView) && this.tileScreenInst != null)
		{
			this.tileScreenInst.Deactivate();
			this.tileScreenInst = null;
		}
	}

	// Token: 0x0600925C RID: 37468 RVA: 0x000FF992 File Offset: 0x000FDB92
	public void TogglePauseScreen()
	{
		PauseScreen.Instance.Show(true);
	}

	// Token: 0x0600925D RID: 37469 RVA: 0x000FF99F File Offset: 0x000FDB9F
	public void ExternalClose()
	{
		this.OnClickCommon();
	}

	// Token: 0x0600925E RID: 37470 RVA: 0x000FF9A7 File Offset: 0x000FDBA7
	private void OnUIClear(object data)
	{
		this.CloseSubMenus();
		SelectTool.Instance.Select(null, true);
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		global::Debug.LogWarning("OnUIClear() Event system is null");
	}

	// Token: 0x0600925F RID: 37471 RVA: 0x000FF9DE File Offset: 0x000FDBDE
	protected override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x06009260 RID: 37472 RVA: 0x000FF9E6 File Offset: 0x000FDBE6
	private bool AreSubMenusOpen()
	{
		return this.subMenus.Count > 0;
	}

	// Token: 0x06009261 RID: 37473 RVA: 0x00386A80 File Offset: 0x00384C80
	private KToggleMenu.ToggleInfo[] GetFillers()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			KPrefabID kprefabID = pickupable.KPrefabID;
			if (kprefabID.HasTag(GameTags.Filler) && hashSet.Add(kprefabID.PrefabTag))
			{
				string text = kprefabID.GetComponent<PrimaryElement>().Element.id.ToString();
				list.Add(new KToggleMenu.ToggleInfo(text, null, global::Action.NumActions));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06009262 RID: 37474 RVA: 0x000FF9F6 File Offset: 0x000FDBF6
	public bool IsBuildingChorePanelActive()
	{
		return this.detailsScreen != null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
	}

	// Token: 0x04006EA9 RID: 28329
	private DetailsScreen detailsScreen;

	// Token: 0x04006EAA RID: 28330
	private UserMenuScreen userMenu;

	// Token: 0x04006EAB RID: 28331
	[SerializeField]
	private GameObject detailsScreenPrefab;

	// Token: 0x04006EAC RID: 28332
	[SerializeField]
	private UserMenuScreen userMenuPrefab;

	// Token: 0x04006EAD RID: 28333
	private GameObject userMenuParent;

	// Token: 0x04006EAE RID: 28334
	[SerializeField]
	private TileScreen tileScreen;

	// Token: 0x04006EB0 RID: 28336
	public KScreen buildMenu;

	// Token: 0x04006EB1 RID: 28337
	private List<KScreen> subMenus = new List<KScreen>();

	// Token: 0x04006EB2 RID: 28338
	private TileScreen tileScreenInst;

	// Token: 0x04006EB3 RID: 28339
	public bool canTogglePauseScreen = true;

	// Token: 0x04006EB4 RID: 28340
	public GameObject selectedGO;
}
