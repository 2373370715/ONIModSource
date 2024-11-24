using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001736 RID: 5942
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06007A53 RID: 31315 RVA: 0x000F05A6 File Offset: 0x000EE7A6
	// (set) Token: 0x06007A54 RID: 31316 RVA: 0x000F05AE File Offset: 0x000EE7AE
	public AccessControl.Permission DefaultPermission
	{
		get
		{
			return this._defaultPermission;
		}
		set
		{
			this._defaultPermission = value;
			this.SetStatusItem();
			this.SetGridRestrictions(null, this._defaultPermission);
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06007A55 RID: 31317 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool Online
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06007A56 RID: 31318 RVA: 0x003182C0 File Offset: 0x003164C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (AccessControl.accessControlActive == null)
		{
			AccessControl.accessControlActive = new StatusItem("accessControlActive", BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		}
		base.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
		base.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
	}

	// Token: 0x06007A57 RID: 31319 RVA: 0x00318334 File Offset: 0x00316534
	private void CheckForBadData()
	{
		List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> list = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item in this.savedPermissions)
		{
			if (item.Key.Get() == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item2 in list)
		{
			this.savedPermissions.Remove(item2);
		}
	}

	// Token: 0x06007A58 RID: 31320 RVA: 0x003183E4 File Offset: 0x003165E4
	protected override void OnSpawn()
	{
		this.isTeleporter = (base.GetComponent<NavTeleporter>() != null);
		base.OnSpawn();
		if (this.savedPermissions.Count > 0)
		{
			this.CheckForBadData();
		}
		if (this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
		}
		ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
		for (int i = this.savedPermissions.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.savedPermissions[i].Key.Get();
			if (kprefabID != null)
			{
				MinionIdentity component = kprefabID.GetComponent<MinionIdentity>();
				if (component != null)
				{
					pooledList.Add(new global::Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[i].Value));
					this.savedPermissions.RemoveAt(i);
					this.ClearGridRestrictions(kprefabID);
				}
			}
		}
		foreach (global::Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in pooledList)
		{
			this.SetPermission(tuple.first, tuple.second);
		}
		pooledList.Recycle();
		this.SetStatusItem();
	}

	// Token: 0x06007A59 RID: 31321 RVA: 0x000F05CA File Offset: 0x000EE7CA
	protected override void OnCleanUp()
	{
		this.RegisterInGrid(false);
		base.OnCleanUp();
	}

	// Token: 0x06007A5A RID: 31322 RVA: 0x000F05D9 File Offset: 0x000EE7D9
	private void OnControlStateChanged(object data)
	{
		this.overrideAccess = (Door.ControlState)data;
	}

	// Token: 0x06007A5B RID: 31323 RVA: 0x00318520 File Offset: 0x00316720
	private void OnCopySettings(object data)
	{
		AccessControl component = ((GameObject)data).GetComponent<AccessControl>();
		if (component != null)
		{
			this.savedPermissions.Clear();
			foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in component.savedPermissions)
			{
				if (keyValuePair.Key.Get() != null)
				{
					this.SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);
				}
			}
			this._defaultPermission = component._defaultPermission;
			this.SetGridRestrictions(null, this.DefaultPermission);
		}
	}

	// Token: 0x06007A5C RID: 31324 RVA: 0x000F05E7 File Offset: 0x000EE7E7
	public void SetRegistered(bool newRegistered)
	{
		if (newRegistered && !this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
			return;
		}
		if (!newRegistered && this.registered)
		{
			this.RegisterInGrid(false);
		}
	}

	// Token: 0x06007A5D RID: 31325 RVA: 0x003185DC File Offset: 0x003167DC
	public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.savedPermissions.Count; i++)
		{
			if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
			{
				flag = true;
				KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair = this.savedPermissions[i];
				this.savedPermissions[i] = new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(keyValuePair.Key, permission);
				break;
			}
		}
		if (!flag)
		{
			this.savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(new Ref<KPrefabID>(component), permission));
		}
		this.SetStatusItem();
		this.SetGridRestrictions(component, permission);
	}

	// Token: 0x06007A5E RID: 31326 RVA: 0x00318688 File Offset: 0x00316888
	private void RestorePermissions()
	{
		this.SetGridRestrictions(null, this.DefaultPermission);
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in this.savedPermissions)
		{
			KPrefabID x = keyValuePair.Key.Get();
			if (x == null)
			{
				DebugUtil.Assert(x == null, "Tried to set a duplicant-specific access restriction with a null key! This will result in an invisible default permission!");
			}
			this.SetGridRestrictions(keyValuePair.Key.Get(), keyValuePair.Value);
		}
	}

	// Token: 0x06007A5F RID: 31327 RVA: 0x00318724 File Offset: 0x00316924
	private void RegisterInGrid(bool register)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		if (register)
		{
			Rotatable component3 = base.GetComponent<Rotatable>();
			Grid.Restriction.Orientation orientation;
			if (!this.isTeleporter)
			{
				orientation = ((component3 == null || component3.GetOrientation() == Orientation.Neutral) ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
			}
			else
			{
				orientation = Grid.Restriction.Orientation.SingleCell;
			}
			if (component != null)
			{
				this.registeredBuildingCells = component.PlacementCells;
				int[] array = this.registeredBuildingCells;
				for (int i = 0; i < array.Length; i++)
				{
					Grid.RegisterRestriction(array[i], orientation);
				}
			}
			else
			{
				foreach (CellOffset offset in component2.OccupiedCellsOffsets)
				{
					Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);
				}
			}
			if (this.isTeleporter)
			{
				Grid.RegisterRestriction(base.GetComponent<NavTeleporter>().GetCell(), orientation);
			}
		}
		else
		{
			if (component != null)
			{
				if (component.GetMyWorldId() != 255 && this.registeredBuildingCells != null)
				{
					int[] array = this.registeredBuildingCells;
					for (int i = 0; i < array.Length; i++)
					{
						Grid.UnregisterRestriction(array[i]);
					}
					this.registeredBuildingCells = null;
				}
			}
			else
			{
				foreach (CellOffset offset2 in component2.OccupiedCellsOffsets)
				{
					Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));
				}
			}
			if (this.isTeleporter)
			{
				int cell = base.GetComponent<NavTeleporter>().GetCell();
				if (cell != Grid.InvalidCell)
				{
					Grid.UnregisterRestriction(cell);
				}
			}
		}
		this.registered = register;
	}

	// Token: 0x06007A60 RID: 31328 RVA: 0x003188C8 File Offset: 0x00316AC8
	private void SetGridRestrictions(KPrefabID kpid, AccessControl.Permission permission)
	{
		if (!this.registered || !base.isSpawned)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		switch (permission)
		{
		case AccessControl.Permission.Both:
			directions = (Grid.Restriction.Directions)0;
			break;
		case AccessControl.Permission.GoLeft:
			directions = Grid.Restriction.Directions.Right;
			break;
		case AccessControl.Permission.GoRight:
			directions = Grid.Restriction.Directions.Left;
			break;
		case AccessControl.Permission.Neither:
			directions = (Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right);
			break;
		}
		if (this.isTeleporter)
		{
			if (directions != (Grid.Restriction.Directions)0)
			{
				directions = Grid.Restriction.Directions.Teleport;
			}
			else
			{
				directions = (Grid.Restriction.Directions)0;
			}
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.SetRestriction(array[i], minionInstanceID, directions);
			}
		}
		else
		{
			foreach (CellOffset offset in component2.OccupiedCellsOffsets)
			{
				Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID, directions);
			}
		}
		if (this.isTeleporter)
		{
			Grid.SetRestriction(base.GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
		}
	}

	// Token: 0x06007A61 RID: 31329 RVA: 0x003189DC File Offset: 0x00316BDC
	private void ClearGridRestrictions(KPrefabID kpid)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], minionInstanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
		}
	}

	// Token: 0x06007A62 RID: 31330 RVA: 0x00318A84 File Offset: 0x00316C84
	public AccessControl.Permission GetPermission(Navigator minion)
	{
		Door.ControlState controlState = this.overrideAccess;
		if (controlState == Door.ControlState.Opened)
		{
			return AccessControl.Permission.Both;
		}
		if (controlState == Door.ControlState.Locked)
		{
			return AccessControl.Permission.Neither;
		}
		return this.GetSetPermission(this.GetKeyForNavigator(minion));
	}

	// Token: 0x06007A63 RID: 31331 RVA: 0x000F0614 File Offset: 0x000EE814
	private MinionAssignablesProxy GetKeyForNavigator(Navigator minion)
	{
		return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
	}

	// Token: 0x06007A64 RID: 31332 RVA: 0x000F0626 File Offset: 0x000EE826
	public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
	{
		return this.GetSetPermission(key.GetComponent<KPrefabID>());
	}

	// Token: 0x06007A65 RID: 31333 RVA: 0x00318AB4 File Offset: 0x00316CB4
	private AccessControl.Permission GetSetPermission(KPrefabID kpid)
	{
		AccessControl.Permission result = this.DefaultPermission;
		if (kpid != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == kpid.InstanceID)
				{
					result = this.savedPermissions[i].Value;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06007A66 RID: 31334 RVA: 0x00318B20 File Offset: 0x00316D20
	public void ClearPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					this.savedPermissions.RemoveAt(i);
					break;
				}
			}
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(component);
	}

	// Token: 0x06007A67 RID: 31335 RVA: 0x00318B90 File Offset: 0x00316D90
	public bool IsDefaultPermission(MinionAssignablesProxy key)
	{
		bool flag = false;
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}

	// Token: 0x06007A68 RID: 31336 RVA: 0x00318BF0 File Offset: 0x00316DF0
	private void SetStatusItem()
	{
		if (this._defaultPermission != AccessControl.Permission.Both || this.savedPermissions.Count > 0)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, null);
			return;
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
	}

	// Token: 0x06007A69 RID: 31337 RVA: 0x00318C54 File Offset: 0x00316E54
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04005BCE RID: 23502
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005BCF RID: 23503
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005BD0 RID: 23504
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04005BD1 RID: 23505
	private bool isTeleporter;

	// Token: 0x04005BD2 RID: 23506
	private int[] registeredBuildingCells;

	// Token: 0x04005BD3 RID: 23507
	[Serialize]
	private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();

	// Token: 0x04005BD4 RID: 23508
	[Serialize]
	private AccessControl.Permission _defaultPermission;

	// Token: 0x04005BD5 RID: 23509
	[Serialize]
	public bool registered = true;

	// Token: 0x04005BD6 RID: 23510
	[Serialize]
	public bool controlEnabled;

	// Token: 0x04005BD7 RID: 23511
	public Door.ControlState overrideAccess;

	// Token: 0x04005BD8 RID: 23512
	private static StatusItem accessControlActive;

	// Token: 0x04005BD9 RID: 23513
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnControlStateChanged(data);
	});

	// Token: 0x04005BDA RID: 23514
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001737 RID: 5943
	public enum Permission
	{
		// Token: 0x04005BDC RID: 23516
		Both,
		// Token: 0x04005BDD RID: 23517
		GoLeft,
		// Token: 0x04005BDE RID: 23518
		GoRight,
		// Token: 0x04005BDF RID: 23519
		Neither
	}
}
