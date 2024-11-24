using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	[MyCmpReq]
	private Pickupable pickupable;

	[Serialize]
	private bool isMarkedForMove;

	[Serialize]
	private Ref<Storage> storageProxy;

	private int storageReachableChangedHandle = -1;

	private int reachableChangedHandle = -1;

	private int cancelHandle = -1;

	private Guid pendingMoveGuid;

	private Guid storageUnreachableGuid;

	public bool IsMarkedForMove => isMarkedForMove;

	public Storage StorageProxy
	{
		get
		{
			if (storageProxy == null)
			{
				return null;
			}
			return storageProxy.Get();
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(493375141, OnRefreshUserMenu);
		Subscribe(1335436905, OnSplitFromChunk);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (isMarkedForMove)
		{
			if (StorageProxy != null)
			{
				if (reachableChangedHandle < 0)
				{
					reachableChangedHandle = Subscribe(-1432940121, OnReachableChanged);
				}
				if (storageReachableChangedHandle < 0)
				{
					storageReachableChangedHandle = StorageProxy.Subscribe(-1432940121, OnReachableChanged);
				}
				if (cancelHandle < 0)
				{
					cancelHandle = Subscribe(2127324410, CleanupMove);
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
			}
			else
			{
				isMarkedForMove = false;
			}
		}
		if (IsCritter())
		{
			skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, UpdateStatusItem);
			shouldShowSkillPerkStatusItem = isMarkedForMove;
			requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
			UpdateStatusItem();
		}
	}

	private void OnReachableChanged(object data)
	{
		if (!isMarkedForMove)
		{
			return;
		}
		if (StorageProxy != null)
		{
			int num = Grid.PosToCell(pickupable);
			int num2 = Grid.PosToCell(StorageProxy);
			if (num != num2)
			{
				bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
				if (pickupable.KPrefabID.HasTag(GameTags.Creatures.Confined))
				{
					flag = false;
				}
				KSelectable component = GetComponent<KSelectable>();
				pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, pendingMoveGuid, flag, this);
				storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, storageUnreachableGuid, !flag, this);
			}
		}
		else
		{
			ClearMove();
		}
	}

	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				storageProxy = new Ref<Storage>(component.StorageProxy);
				MarkForMove();
			}
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (isMarkedForMove && StorageProxy != null)
		{
			StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			ClearStorageProxy();
		}
	}

	private void CleanupMove(object data)
	{
		if (StorageProxy != null)
		{
			StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	public void ClearMove()
	{
		if (isMarkedForMove)
		{
			isMarkedForMove = false;
			KSelectable component = GetComponent<KSelectable>();
			pendingMoveGuid = component.RemoveStatusItem(pendingMoveGuid);
			storageUnreachableGuid = component.RemoveStatusItem(storageUnreachableGuid);
			ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			if (reachableChangedHandle != -1)
			{
				Unsubscribe(-1432940121, OnReachableChanged);
				reachableChangedHandle = -1;
			}
			if (cancelHandle != -1)
			{
				Unsubscribe(2127324410, CleanupMove);
				cancelHandle = -1;
			}
		}
		UpdateStatusItem();
	}

	private void ClearStorageProxy()
	{
		if (storageReachableChangedHandle != -1)
		{
			StorageProxy.Unsubscribe(-1432940121, OnReachableChanged);
			storageReachableChangedHandle = -1;
		}
		storageProxy = null;
	}

	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	private void OnClickCancel()
	{
		if (StorageProxy != null)
		{
			StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	private void OnRefreshUserMenu(object data)
	{
		if (!pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			KIconButtonMenu.ButtonInfo button = (isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, OnClickCancel, Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, OnClickMove, Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP));
			Game.Instance.userMenu.AddButton(base.gameObject, button);
		}
	}

	public void MoveToLocation(int cell)
	{
		CreateStorageProxy(cell);
		MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	private void MarkForMove()
	{
		Trigger(2127324410);
		isMarkedForMove = true;
		OnReachableChanged(null);
		storageReachableChangedHandle = StorageProxy.Subscribe(-1432940121, OnReachableChanged);
		reachableChangedHandle = Subscribe(-1432940121, OnReachableChanged);
		StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		cancelHandle = Subscribe(2127324410, CleanupMove);
		UpdateStatusItem();
	}

	private void UpdateStatusItem()
	{
		if (IsCritter())
		{
			shouldShowSkillPerkStatusItem = isMarkedForMove;
			base.UpdateStatusItem();
		}
	}

	private bool IsCritter()
	{
		return GetComponent<Capturable>() != null;
	}

	public bool CanMoveTo(int cell)
	{
		if (!Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell))
		{
			return base.gameObject.IsMyParentWorld(cell);
		}
		return false;
	}

	private void CreateStorageProxy(int cell)
	{
		if (storageProxy == null || storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject obj = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = obj.GetComponent<Storage>();
			obj.SetActive(value: true);
			storageProxy = new Ref<Storage>(component2);
		}
	}
}
