using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
	[Serialize]
	protected bool isMarkedForCarve;

	protected Chore chore;

	private string buttonLabel;

	private string buttonTooltip;

	private string cancelButtonLabel;

	private string cancelButtonTooltip;

	private StatusItem pendingStatusItem;

	public bool showUserMenuButtons = true;

	public string dropItemPrefabId;

	public HandleVector<int>.Handle partitionerEntry;

	private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnCancel(data);
	});

	private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	public bool IsMarkedForCarve => isMarkedForCarve;

	protected Carvable()
	{
		buttonLabel = UI.USERMENUACTIONS.CARVE.NAME;
		buttonTooltip = UI.USERMENUACTIONS.CARVE.TOOLTIP;
		cancelButtonLabel = UI.USERMENUACTIONS.CANCELCARVE.NAME;
		cancelButtonTooltip = UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		workerStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Workable workable = (Workable)data;
			if (workable != null && workable.GetComponent<KSelectable>() != null)
			{
				str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
			}
			return str;
		};
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_sculpture_kanim") };
		synchronizeAnims = false;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetWorkTime(10f);
		Subscribe(2127324410, OnCancelDelegate);
		Subscribe(493375141, OnRefreshUserMenuDelegate);
		faceTargetWhenWorking = true;
		Prioritizable.AddRef(base.gameObject);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (isMarkedForCarve)
		{
			MarkForCarve();
		}
	}

	public void Carve()
	{
		isMarkedForCarve = false;
		chore = null;
		GetComponent<KSelectable>().RemoveStatusItem(pendingStatusItem);
		GetComponent<KSelectable>().RemoveStatusItem(workerStatusItem);
		Game.Instance.userMenu.Refresh(base.gameObject);
		ProducePickupable(dropItemPrefabId);
		Object.Destroy(base.gameObject);
	}

	public void MarkForCarve(bool instantOnDebug = true)
	{
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			Carve();
		}
		else if (chore == null)
		{
			isMarkedForCarve = true;
			chore = new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, this);
			chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
			GetComponent<KSelectable>().AddStatusItem(pendingStatusItem, this);
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		Carve();
	}

	private void OnCancel(object data)
	{
		if (chore != null)
		{
			chore.Cancel("Cancel uproot");
			chore = null;
			GetComponent<KSelectable>().RemoveStatusItem(pendingStatusItem);
		}
		isMarkedForCarve = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	private void OnClickCarve()
	{
		MarkForCarve();
	}

	protected void OnClickCancelCarve()
	{
		OnCancel(null);
	}

	private void OnRefreshUserMenu(object data)
	{
		if (showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = ((chore != null) ? new KIconButtonMenu.ButtonInfo("action_carve", cancelButtonLabel, OnClickCancelCarve, Action.NumActions, null, null, null, cancelButtonTooltip) : new KIconButtonMenu.ButtonInfo("action_carve", buttonLabel, OnClickCarve, Action.NumActions, null, null, null, buttonTooltip));
			Game.Instance.userMenu.AddButton(base.gameObject, button);
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		GetComponent<KSelectable>().RemoveStatusItem(pendingStatusItem);
	}

	private GameObject ProducePickupable(string pickupablePrefabId)
	{
		if (pickupablePrefabId != null)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore);
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			gameObject.GetComponent<PrimaryElement>().Temperature = component.Temperature;
			gameObject.SetActive(value: true);
			string properName = gameObject.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, gameObject.transform);
			return gameObject;
		}
		return null;
	}

	public void Dig()
	{
		Carve();
	}

	public void MarkForDig(bool instantOnDebug = true)
	{
		MarkForCarve(instantOnDebug);
	}
}
