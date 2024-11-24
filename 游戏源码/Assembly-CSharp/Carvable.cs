using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001060 RID: 4192
[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06005576 RID: 21878 RVA: 0x000D7C7F File Offset: 0x000D5E7F
	public bool IsMarkedForCarve
	{
		get
		{
			return this.isMarkedForCarve;
		}
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x0027E45C File Offset: 0x0027C65C
	protected Carvable()
	{
		this.buttonLabel = UI.USERMENUACTIONS.CARVE.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.CARVE.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELCARVE.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x0027E4B8 File Offset: 0x0027C6B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Workable workable = (Workable)data;
			if (workable != null && workable.GetComponent<KSelectable>() != null)
			{
				str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
			}
			return str;
		};
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x0027E56C File Offset: 0x0027C76C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		base.Subscribe<Carvable>(2127324410, Carvable.OnCancelDelegate);
		base.Subscribe<Carvable>(493375141, Carvable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Prioritizable.AddRef(base.gameObject);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForCarve)
		{
			this.MarkForCarve(true);
		}
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x0027E624 File Offset: 0x0027C824
	public void Carve()
	{
		this.isMarkedForCarve = false;
		this.chore = null;
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
		this.ProducePickupable(this.dropItemPrefabId);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x0027E694 File Offset: 0x0027C894
	public void MarkForCarve(bool instantOnDebug = true)
	{
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Carve();
			return;
		}
		if (this.chore == null)
		{
			this.isMarkedForCarve = true;
			this.chore = new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x000D7C87 File Offset: 0x000D5E87
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Carve();
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x0027E718 File Offset: 0x0027C918
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		}
		this.isMarkedForCarve = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x000D7C8F File Offset: 0x000D5E8F
	private void OnClickCarve()
	{
		this.MarkForCarve(true);
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x000D7C98 File Offset: 0x000D5E98
	protected void OnClickCancelCarve()
	{
		this.OnCancel(null);
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x0027E774 File Offset: 0x0027C974
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_carve", this.cancelButtonLabel, new System.Action(this.OnClickCancelCarve), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_carve", this.buttonLabel, new System.Action(this.OnClickCarve), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005581 RID: 21889 RVA: 0x000D7CA1 File Offset: 0x000D5EA1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x000D7CB9 File Offset: 0x000D5EB9
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x0027E808 File Offset: 0x0027CA08
	private GameObject ProducePickupable(string pickupablePrefabId)
	{
		if (pickupablePrefabId != null)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			gameObject.GetComponent<PrimaryElement>().Temperature = component.Temperature;
			gameObject.SetActive(true);
			string properName = gameObject.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x000D7C87 File Offset: 0x000D5E87
	public void Dig()
	{
		this.Carve();
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x000D7CD5 File Offset: 0x000D5ED5
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForCarve(instantOnDebug);
	}

	// Token: 0x04003BF3 RID: 15347
	[Serialize]
	protected bool isMarkedForCarve;

	// Token: 0x04003BF4 RID: 15348
	protected Chore chore;

	// Token: 0x04003BF5 RID: 15349
	private string buttonLabel;

	// Token: 0x04003BF6 RID: 15350
	private string buttonTooltip;

	// Token: 0x04003BF7 RID: 15351
	private string cancelButtonLabel;

	// Token: 0x04003BF8 RID: 15352
	private string cancelButtonTooltip;

	// Token: 0x04003BF9 RID: 15353
	private StatusItem pendingStatusItem;

	// Token: 0x04003BFA RID: 15354
	public bool showUserMenuButtons = true;

	// Token: 0x04003BFB RID: 15355
	public string dropItemPrefabId;

	// Token: 0x04003BFC RID: 15356
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003BFD RID: 15357
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04003BFE RID: 15358
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
