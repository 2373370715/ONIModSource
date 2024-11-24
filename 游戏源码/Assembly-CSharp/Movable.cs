using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06003253 RID: 12883 RVA: 0x000C0AAC File Offset: 0x000BECAC
	public bool IsMarkedForMove
	{
		get
		{
			return this.isMarkedForMove;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06003254 RID: 12884 RVA: 0x000C0AB4 File Offset: 0x000BECB4
	public Storage StorageProxy
	{
		get
		{
			if (this.storageProxy == null)
			{
				return null;
			}
			return this.storageProxy.Get();
		}
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x000C0ACB File Offset: 0x000BECCB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x002031E8 File Offset: 0x002013E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				if (this.reachableChangedHandle < 0)
				{
					this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.storageReachableChangedHandle < 0)
				{
					this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.cancelHandle < 0)
				{
					this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
			}
			else
			{
				this.isMarkedForMove = false;
			}
		}
		if (this.IsCritter())
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
			this.UpdateStatusItem();
		}
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x00203300 File Offset: 0x00201500
	private void OnReachableChanged(object data)
	{
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				int num = Grid.PosToCell(this.pickupable);
				int num2 = Grid.PosToCell(this.StorageProxy);
				if (num != num2)
				{
					bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
					if (this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Confined))
					{
						flag = false;
					}
					KSelectable component = base.GetComponent<KSelectable>();
					this.pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, this.pendingMoveGuid, flag, this);
					this.storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, this.storageUnreachableGuid, !flag, this);
					return;
				}
			}
			else
			{
				this.ClearMove();
			}
		}
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x002033E0 File Offset: 0x002015E0
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				this.storageProxy = new Ref<Storage>(component.StorageProxy);
				this.MarkForMove();
			}
		}
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x000C0B03 File Offset: 0x000BED03
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isMarkedForMove && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			this.ClearStorageProxy();
		}
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000C0B38 File Offset: 0x000BED38
	private void CleanupMove(object data)
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x00203424 File Offset: 0x00201624
	public void ClearMove()
	{
		if (this.isMarkedForMove)
		{
			this.isMarkedForMove = false;
			KSelectable component = base.GetComponent<KSelectable>();
			this.pendingMoveGuid = component.RemoveStatusItem(this.pendingMoveGuid, false);
			this.storageUnreachableGuid = component.RemoveStatusItem(this.storageUnreachableGuid, false);
			this.ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			if (this.reachableChangedHandle != -1)
			{
				base.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				this.reachableChangedHandle = -1;
			}
			if (this.cancelHandle != -1)
			{
				base.Unsubscribe(2127324410, new Action<object>(this.CleanupMove));
				this.cancelHandle = -1;
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x000C0B59 File Offset: 0x000BED59
	private void ClearStorageProxy()
	{
		if (this.storageReachableChangedHandle != -1)
		{
			this.StorageProxy.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
			this.storageReachableChangedHandle = -1;
		}
		this.storageProxy = null;
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x000C0B8E File Offset: 0x000BED8E
	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x000C0B38 File Offset: 0x000BED38
	private void OnClickCancel()
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x002034DC File Offset: 0x002016DC
	private void OnRefreshUserMenu(object data)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, new System.Action(this.OnClickMove), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x000C0B9B File Offset: 0x000BED9B
	public void MoveToLocation(int cell)
	{
		this.CreateStorageProxy(cell);
		this.MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x00203590 File Offset: 0x00201790
	private void MarkForMove()
	{
		base.Trigger(2127324410, null);
		this.isMarkedForMove = true;
		this.OnReachableChanged(null);
		this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
		this.UpdateStatusItem();
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x000C0BC0 File Offset: 0x000BEDC0
	private void UpdateStatusItem()
	{
		if (this.IsCritter())
		{
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			base.UpdateStatusItem(null);
		}
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x000C0BDD File Offset: 0x000BEDDD
	private bool IsCritter()
	{
		return base.GetComponent<Capturable>() != null;
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x000C0BEB File Offset: 0x000BEDEB
	public bool CanMoveTo(int cell)
	{
		return !Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell) && base.gameObject.IsMyParentWorld(cell);
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x0020363C File Offset: 0x0020183C
	private void CreateStorageProxy(int cell)
	{
		if (this.storageProxy == null || this.storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				this.storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = gameObject.GetComponent<Storage>();
			gameObject.SetActive(true);
			this.storageProxy = new Ref<Storage>(component2);
		}
	}

	// Token: 0x040021D4 RID: 8660
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x040021D5 RID: 8661
	[Serialize]
	private bool isMarkedForMove;

	// Token: 0x040021D6 RID: 8662
	[Serialize]
	private Ref<Storage> storageProxy;

	// Token: 0x040021D7 RID: 8663
	private int storageReachableChangedHandle = -1;

	// Token: 0x040021D8 RID: 8664
	private int reachableChangedHandle = -1;

	// Token: 0x040021D9 RID: 8665
	private int cancelHandle = -1;

	// Token: 0x040021DA RID: 8666
	private Guid pendingMoveGuid;

	// Token: 0x040021DB RID: 8667
	private Guid storageUnreachableGuid;
}
