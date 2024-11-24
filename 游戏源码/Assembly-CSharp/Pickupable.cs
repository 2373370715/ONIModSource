using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AC2 RID: 2754
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
	// Token: 0x17000220 RID: 544
	// (get) Token: 0x0600334C RID: 13132 RVA: 0x000C1829 File Offset: 0x000BFA29
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x0600334D RID: 13133 RVA: 0x000C1831 File Offset: 0x000BFA31
	// (set) Token: 0x0600334E RID: 13134 RVA: 0x000C1839 File Offset: 0x000BFA39
	public int sortOrder
	{
		get
		{
			return this._sortOrder;
		}
		set
		{
			this._sortOrder = value;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x0600334F RID: 13135 RVA: 0x000C1842 File Offset: 0x000BFA42
	// (set) Token: 0x06003350 RID: 13136 RVA: 0x000C184A File Offset: 0x000BFA4A
	public Storage storage { get; set; }

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06003351 RID: 13137 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinTakeAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x000C1853 File Offset: 0x000BFA53
	public bool isChoreAllowedToPickup(ChoreType choreType)
	{
		return this.allowedChoreTypes == null || this.allowedChoreTypes.Contains(choreType);
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06003353 RID: 13139 RVA: 0x000C186B File Offset: 0x000BFA6B
	// (set) Token: 0x06003354 RID: 13140 RVA: 0x000C1873 File Offset: 0x000BFA73
	public bool prevent_absorb_until_stored { get; set; }

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06003355 RID: 13141 RVA: 0x000C187C File Offset: 0x000BFA7C
	// (set) Token: 0x06003356 RID: 13142 RVA: 0x000C1884 File Offset: 0x000BFA84
	public bool isKinematic { get; set; }

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06003357 RID: 13143 RVA: 0x000C188D File Offset: 0x000BFA8D
	// (set) Token: 0x06003358 RID: 13144 RVA: 0x000C1895 File Offset: 0x000BFA95
	public bool wasAbsorbed { get; private set; }

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06003359 RID: 13145 RVA: 0x000C189E File Offset: 0x000BFA9E
	// (set) Token: 0x0600335A RID: 13146 RVA: 0x000C18A6 File Offset: 0x000BFAA6
	public int cachedCell { get; private set; }

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x0600335B RID: 13147 RVA: 0x000C18AF File Offset: 0x000BFAAF
	// (set) Token: 0x0600335C RID: 13148 RVA: 0x002058E8 File Offset: 0x00203AE8
	public bool IsEntombed
	{
		get
		{
			return this.isEntombed;
		}
		set
		{
			if (value != this.isEntombed)
			{
				this.isEntombed = value;
				if (this.isEntombed)
				{
					base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				}
				else
				{
					base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
				}
				base.Trigger(-1089732772, null);
				this.UpdateEntombedVisualizer();
			}
		}
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x000C18B7 File Offset: 0x000BFAB7
	private bool CouldBePickedUpCommon(GameObject carrier)
	{
		return this.UnreservedAmount >= this.MinTakeAmount && (this.UnreservedAmount > 0f || this.FindReservedAmount(carrier) > 0f);
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x00205944 File Offset: 0x00203B44
	public bool CouldBePickedUpByMinion(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier) && (this.storage == null || !this.storage.automatable || !this.storage.automatable.GetAutomationOnly());
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x000C18E6 File Offset: 0x000BFAE6
	public bool CouldBePickedUpByTransferArm(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier) && (this.fetchable_monitor == null || this.fetchable_monitor.IsFetchable());
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x00205994 File Offset: 0x00203B94
	public float FindReservedAmount(GameObject reserver)
	{
		for (int i = 0; i < this.reservations.Count; i++)
		{
			if (this.reservations[i].reserver == reserver)
			{
				return this.reservations[i].amount;
			}
		}
		return 0f;
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06003361 RID: 13153 RVA: 0x000C1908 File Offset: 0x000BFB08
	public float UnreservedAmount
	{
		get
		{
			return this.TotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06003362 RID: 13154 RVA: 0x000C1917 File Offset: 0x000BFB17
	// (set) Token: 0x06003363 RID: 13155 RVA: 0x000C191F File Offset: 0x000BFB1F
	public float ReservedAmount { get; private set; }

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06003364 RID: 13156 RVA: 0x000C1928 File Offset: 0x000BFB28
	// (set) Token: 0x06003365 RID: 13157 RVA: 0x002059E8 File Offset: 0x00203BE8
	public float TotalAmount
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			DebugUtil.Assert(this.primaryElement != null);
			this.primaryElement.Units = value;
			if (value < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
			{
				base.gameObject.DeleteObject();
			}
			this.NotifyChanged(Grid.PosToCell(this));
		}
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x00205A40 File Offset: 0x00203C40
	private void RefreshReservedAmount()
	{
		this.ReservedAmount = 0f;
		for (int i = 0; i < this.reservations.Count; i++)
		{
			this.ReservedAmount += this.reservations[i].amount;
		}
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("UNITY_EDITOR")]
	private void Log(string evt, string param, float value)
	{
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x000C1935 File Offset: 0x000BFB35
	public void ClearReservations()
	{
		this.reservations.Clear();
		this.RefreshReservedAmount();
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x00205A8C File Offset: 0x00203C8C
	[ContextMenu("Print Reservations")]
	public void PrintReservations()
	{
		foreach (Pickupable.Reservation reservation in this.reservations)
		{
			global::Debug.Log(reservation.ToString());
		}
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x00205AEC File Offset: 0x00203CEC
	public int Reserve(string context, GameObject reserver, float amount)
	{
		int num = this.nextTicketNumber;
		this.nextTicketNumber = num + 1;
		int num2 = num;
		Pickupable.Reservation reservation = new Pickupable.Reservation(reserver, amount, num2);
		this.reservations.Add(reservation);
		this.RefreshReservedAmount();
		if (this.OnReservationsChanged != null)
		{
			this.OnReservationsChanged(this, true, reservation);
		}
		return num2;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00205B40 File Offset: 0x00203D40
	public void Unreserve(string context, int ticket)
	{
		int i = 0;
		while (i < this.reservations.Count)
		{
			if (this.reservations[i].ticket == ticket)
			{
				Pickupable.Reservation arg = this.reservations[i];
				this.reservations.RemoveAt(i);
				this.RefreshReservedAmount();
				if (this.OnReservationsChanged != null)
				{
					this.OnReservationsChanged(this, false, arg);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00205BB0 File Offset: 0x00203DB0
	private Pickupable()
	{
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00205C30 File Offset: 0x00203E30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.log = new LoggerFSSF("Pickupable");
		this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
		base.SetWorkTime(1.5f);
		this.targetWorkable = this;
		this.resetProgressOnStop = true;
		base.gameObject.layer = Game.PickupableLayer;
		Vector3 position = base.transform.GetPosition();
		this.UpdateCachedCell(Grid.PosToCell(position));
		base.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
		base.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
		base.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
		base.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
		base.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
		base.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
		this.KPrefabID.AddTag(GameTags.Pickupable, false);
		Components.Pickupables.Add(this);
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x000C1948 File Offset: 0x000BFB48
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x00205D3C File Offset: 0x00203F3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num) && this.deleteOffGrid)
		{
			base.gameObject.DeleteObject();
			return;
		}
		if (base.GetComponent<Health>() != null)
		{
			this.handleFallerComponents = false;
		}
		this.UpdateCachedCell(num);
		new ReachabilityMonitor.Instance(this).StartSM();
		this.fetchable_monitor = new FetchableMonitor.Instance(this);
		this.fetchable_monitor.StartSM();
		base.SetWorkTime(1.5f);
		this.faceTargetWhenWorking = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.65f, 0f));
		}
		this.OnTagsChanged(null);
		this.TryToOffsetIfBuried(CellOffset.none);
		DecorProvider component2 = base.GetComponent<DecorProvider>();
		if (component2 != null && string.IsNullOrEmpty(component2.overrideName))
		{
			component2.overrideName = UI.OVERLAYS.DECOR.CLUTTER;
		}
		this.UpdateEntombedVisualizer();
		base.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
		this.NotifyChanged(num);
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x00205E4C File Offset: 0x0020404C
	[OnDeserialized]
	public void OnDeserialize()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) && base.transform.position.z == 0f)
		{
			KBatchedAnimController component = base.transform.GetComponent<KBatchedAnimController>();
			component.SetSceneLayer(component.sceneLayer);
		}
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x00205EA0 File Offset: 0x002040A0
	public void UpdateListeners(bool worldSpace)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (worldSpace)
		{
			if (this.solidPartitionerEntry.IsValid())
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
			this.objectLayerListItem = new ObjectLayerListItem(base.gameObject, ObjectLayer.Pickupables, num);
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", base.gameObject, num, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			this.worldPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", this, num, GameScenePartitioner.Instance.pickupablesLayer, null);
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Pickupable.OnCellChange");
			Singleton<CellChangeMonitor>.Instance.MarkDirty(base.transform);
			Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(base.transform);
			return;
		}
		else
		{
			if (this.storedPartitionerEntry.IsValid())
			{
				return;
			}
			this.storedPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterStoredPickupable", this, num, GameScenePartitioner.Instance.storedPickupablesLayer, null);
			if (this.objectLayerListItem != null)
			{
				this.objectLayerListItem.Clear();
				this.objectLayerListItem = null;
			}
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			return;
		}
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x000C1950 File Offset: 0x000BFB50
	public void RegisterListeners()
	{
		this.UpdateListeners(true);
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x00206014 File Offset: 0x00204214
	public void UnregisterListeners()
	{
		if (this.objectLayerListItem != null)
		{
			this.objectLayerListItem.Clear();
			this.objectLayerListItem = null;
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
		base.Unsubscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate, false);
		base.Unsubscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate, false);
		base.Unsubscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate, false);
		base.Unsubscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset, false);
		if (base.isSpawned)
		{
			base.Unsubscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate, false);
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x000C1959 File Offset: 0x000BFB59
	private void OnSolidChanged(object data)
	{
		this.TryToOffsetIfBuried(CellOffset.none);
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x00206108 File Offset: 0x00204308
	private void SetWorkableOffset(object data)
	{
		CellOffset offset = CellOffset.none;
		WorkerBase workerBase = data as WorkerBase;
		if (workerBase != null)
		{
			int num = Grid.PosToCell(workerBase);
			int base_cell = Grid.PosToCell(this);
			offset = (Grid.IsValidCell(num) ? Grid.GetCellOffsetDirection(base_cell, num) : CellOffset.none);
		}
		this.TryToOffsetIfBuried(offset);
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x00206158 File Offset: 0x00204358
	private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
	{
		if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.left,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.right,
				CellOffset.rightup
			};
		}
		if (preferedDirectionOffset == CellOffset.up)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.rightup,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.leftdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.left
			};
		}
		if (preferedDirectionOffset == CellOffset.rightdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.rightdown,
				CellOffset.right
			};
		}
		if (preferedDirectionOffset == CellOffset.down)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.rightdown
			};
		}
		return new CellOffset[0];
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x002062D8 File Offset: 0x002044D8
	public void TryToOffsetIfBuried(CellOffset offset)
	{
		if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
		if ((smi == null || smi.IsDead()) && ((Grid.Solid[num] && Grid.Foundation[num]) || Grid.Properties[num] != 0))
		{
			CellOffset[] array = this.GetPreferedOffsets(offset).Concat(Pickupable.displacementOffsets);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Grid.OffsetCell(num, array[i]);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					Vector3 position = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
					KCollider2D component = base.GetComponent<KCollider2D>();
					if (component != null)
					{
						position.y += base.transform.GetPosition().y - component.bounds.min.y;
					}
					base.transform.SetPosition(position);
					num = num2;
					this.RemoveFaller();
					this.AddFaller(Vector2.zero);
					break;
				}
			}
		}
		this.HandleSolidCell(num);
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0020642C File Offset: 0x0020462C
	private bool HandleSolidCell(int cell)
	{
		bool flag = this.IsEntombed;
		bool flag2 = false;
		if (Grid.IsValidCell(cell) && Grid.Solid[cell])
		{
			DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
			if (smi == null || smi.IsDead())
			{
				this.Clearable.CancelClearing();
				flag2 = true;
			}
		}
		if (flag2 != flag && !this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.IsEntombed = flag2;
			base.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
		}
		this.UpdateEntombedVisualizer();
		return this.IsEntombed;
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x002064BC File Offset: 0x002046BC
	private void OnCellChange()
	{
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			Vector2 vector = new Vector2(-0.1f * (float)Grid.WidthInCells, 1.1f * (float)Grid.WidthInCells);
			Vector2 vector2 = new Vector2(-0.1f * (float)Grid.HeightInCells, 1.1f * (float)Grid.HeightInCells);
			if (this.deleteOffGrid && (position.x < vector.x || vector.y < position.x || position.y < vector2.x || vector2.y < position.y))
			{
				this.DeleteObject();
				return;
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
			if (this.HandleSolidCell(num))
			{
				return;
			}
			this.objectLayerListItem.Update(num);
			bool flag = false;
			if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
			{
				int num2 = Grid.CellBelow(num);
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
					while (nextItem != null)
					{
						GameObject gameObject = nextItem.gameObject;
						nextItem = nextItem.nextItem;
						Pickupable component = gameObject.GetComponent<Pickupable>();
						if (component != null)
						{
							flag = component.TryAbsorb(this, false, false);
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, num);
			GameScenePartitioner.Instance.UpdatePosition(this.worldPartitionerEntry, num);
			int cachedCell = this.cachedCell;
			this.UpdateCachedCell(num);
			if (!flag)
			{
				this.NotifyChanged(num);
			}
			if (Grid.IsValidCell(cachedCell) && num != cachedCell)
			{
				this.NotifyChanged(cachedCell);
			}
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x00206668 File Offset: 0x00204868
	private void OnTagsChanged(object data)
	{
		if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
		{
			this.UpdateListeners(true);
			this.AddFaller(Vector2.zero);
			return;
		}
		this.UpdateListeners(false);
		this.RemoveFaller();
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x000C1966 File Offset: 0x000BFB66
	private void NotifyChanged(int new_cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, this);
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x002066BC File Offset: 0x002048BC
	public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
	{
		if (other == null)
		{
			return false;
		}
		if (other.wasAbsorbed)
		{
			return false;
		}
		if (this.wasAbsorbed)
		{
			return false;
		}
		if (!other.CanAbsorb(this))
		{
			return false;
		}
		if (this.prevent_absorb_until_stored)
		{
			return false;
		}
		if (!allow_cross_storage && this.storage == null != (other.storage == null))
		{
			return false;
		}
		this.Absorb(other);
		if (!hide_effects && EffectPrefabs.Instance != null && !this.storage)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			global::Util.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position, Quaternion.identity, null, null, true, 0).SetActive(true);
		}
		return true;
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x00206784 File Offset: 0x00204984
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.ReleaseEntombedVisualizerAndAddFaller(false);
		this.RemoveFaller();
		if (this.storage)
		{
			this.storage.Remove(base.gameObject, true);
		}
		this.UnregisterListeners();
		this.fetchable_monitor = null;
		Components.Pickupables.Remove(this);
		if (this.reservations.Count > 0)
		{
			Pickupable.Reservation[] array = this.reservations.ToArray();
			this.reservations.Clear();
			if (this.OnReservationsChanged != null)
			{
				foreach (Pickupable.Reservation arg in array)
				{
					this.OnReservationsChanged(this, false, arg);
				}
			}
		}
		if (Grid.IsValidCell(this.cachedCell))
		{
			this.NotifyChanged(this.cachedCell);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x00206850 File Offset: 0x00204A50
	public Pickupable Take(float amount)
	{
		if (amount <= 0f)
		{
			return null;
		}
		if (this.OnTake == null)
		{
			if (this.storage != null)
			{
				this.storage.Remove(base.gameObject, true);
			}
			return this;
		}
		if (amount >= this.TotalAmount && this.storage != null && !this.primaryElement.KeepZeroMassObject)
		{
			this.storage.Remove(base.gameObject, true);
		}
		float num = Math.Min(this.TotalAmount, amount);
		if (num <= 0f)
		{
			return null;
		}
		return this.OnTake(this, num);
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x002068EC File Offset: 0x00204AEC
	private void Absorb(Pickupable pickupable)
	{
		global::Debug.Assert(!this.wasAbsorbed);
		global::Debug.Assert(!pickupable.wasAbsorbed);
		base.Trigger(-2064133523, pickupable);
		pickupable.Trigger(-1940207677, base.gameObject);
		pickupable.wasAbsorbed = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == pickupable.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(component, false);
		}
		pickupable.gameObject.DeleteObject();
		this.NotifyChanged(Grid.PosToCell(this));
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0020699C File Offset: 0x00204B9C
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		if (flag && data is Storage && ((Storage)data).gameObject == base.gameObject)
		{
			return;
		}
		if (!flag)
		{
			this.KPrefabID.RemoveTag(GameTags.Stored);
			this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
			return;
		}
		this.KPrefabID.AddTag(GameTags.Stored, false);
		if (this.storage == null || !this.storage.allowItemRemoval)
		{
			this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
			return;
		}
		this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00206A58 File Offset: 0x00204C58
	public void OnStore(object data)
	{
		this.storage = (data as Storage);
		bool flag = data is Storage || (data != null && (bool)data);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (this.carryAnimOverride != null && this.lastCarrier != null)
		{
			this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
			this.lastCarrier = null;
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2)
		{
			component2.IsSelectable = !flag;
		}
		if (flag)
		{
			int cachedCell = this.cachedCell;
			this.RefreshStorageTags(data);
			this.RemoveFaller();
			if (this.storage != null)
			{
				if (this.carryAnimOverride != null && this.storage.GetComponent<Navigator>() != null)
				{
					this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
					if (this.lastCarrier != null && this.lastCarrier.HasTag(GameTags.BaseMinion))
					{
						this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0f);
					}
				}
				this.UpdateCachedCell(Grid.PosToCell(this.storage));
			}
			this.NotifyChanged(cachedCell);
			if (component != null)
			{
				component.SetRegistered(false);
				return;
			}
		}
		else
		{
			if (component != null)
			{
				component.SetRegistered(true);
			}
			this.RemovedFromStorage();
		}
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x00206BA8 File Offset: 0x00204DA8
	private void RemovedFromStorage()
	{
		this.storage = null;
		this.UpdateCachedCell(Grid.PosToCell(this));
		this.RefreshStorageTags(null);
		this.AddFaller(Vector2.zero);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		base.gameObject.transform.rotation = Quaternion.identity;
		this.UpdateListeners(true);
		component.GetBatchInstanceData().ClearOverrideTransformMatrix();
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x000C197E File Offset: 0x000BFB7E
	public void UpdateCachedCellFromStoragePosition()
	{
		global::Debug.Assert(this.storage != null, "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
		this.UpdateCachedCell(Grid.PosToCell(this.storage));
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x00206C10 File Offset: 0x00204E10
	public void UpdateCachedCell(int cell)
	{
		if (this.cachedCell != cell && this.storedPartitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.storedPartitionerEntry, cell);
		}
		this.cachedCell = cell;
		this.GetOffsets(this.cachedCell);
		if (this.KPrefabID.HasTag(GameTags.PickupableStorage))
		{
			base.GetComponent<Storage>().UpdateStoredItemCachedCells();
		}
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x000C19A7 File Offset: 0x000BFBA7
	public override int GetCell()
	{
		return this.cachedCell;
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x00206C78 File Offset: 0x00204E78
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunforPickup && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "pickup", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x00206CD0 File Offset: 0x00204ED0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		if (!(this != null))
		{
			pickupableStartWorkInfo.setResultCb(null);
			return;
		}
		Pickupable pickupable = this.Take(amount);
		if (pickupable != null)
		{
			component.Store(pickupable.gameObject, false, false, true, false);
			worker.SetWorkCompleteData(pickupable);
			pickupableStartWorkInfo.setResultCb(pickupable.gameObject);
			return;
		}
		pickupableStartWorkInfo.setResultCb(null);
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x000C19AF File Offset: 0x000BFBAF
	public override Vector3 GetTargetPoint()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000C19BC File Offset: 0x000BFBBC
	public bool IsReachable()
	{
		return this.isReachable;
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x00206D5C File Offset: 0x00204F5C
	private void OnReachableChanged(object data)
	{
		this.isReachable = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, this);
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x000C19C4 File Offset: 0x000BFBC4
	private void AddFaller(Vector2 initial_velocity)
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (!GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Add(base.gameObject, initial_velocity);
		}
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x000C19F3 File Offset: 0x000BFBF3
	private void RemoveFaller()
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Remove(base.gameObject);
		}
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00206DB4 File Offset: 0x00204FB4
	private void OnOreSizeChanged(object data)
	{
		Vector3 v = Vector3.zero;
		HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			v = GameComps.Gravities.GetData(handle).velocity;
		}
		this.RemoveFaller();
		if (!this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.AddFaller(v);
		}
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x00206E1C File Offset: 0x0020501C
	private void OnLanded(object data)
	{
		if (CameraController.Instance == null)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		Vector2I vector2I = Grid.PosToXY(position);
		if (vector2I.x < 0 || Grid.WidthInCells <= vector2I.x || vector2I.y < 0 || Grid.HeightInCells <= vector2I.y)
		{
			this.DeleteObject();
			return;
		}
		Vector2 vector = (Vector2)data;
		if (vector.sqrMagnitude <= 0.2f || SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		Element element = this.primaryElement.Element;
		if (element.substance != null)
		{
			string text = element.substance.GetOreBumpSound();
			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}
			if (element.tag.ToString() == "Creature" && !base.gameObject.HasTag(GameTags.Seed))
			{
				text = "Bodyfall_rock";
			}
			else
			{
				text = "Ore_bump_" + text;
			}
			string text2 = GlobalAssets.GetSound(text, true);
			text2 = ((text2 != null) ? text2 : GlobalAssets.GetSound("Ore_bump_rock", false));
			if (CameraController.Instance.IsAudibleSound(base.transform.GetPosition(), text2))
			{
				int num = Grid.PosToCell(position);
				bool isLiquid = Grid.Element[num].IsLiquid;
				float value = 0f;
				if (isLiquid)
				{
					value = SoundUtil.GetLiquidDepth(num);
				}
				FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(text2, CameraController.Instance.GetVerticallyScaledPosition(base.transform.GetPosition(), false), 1f);
				instance.setParameterByName("velocity", vector.magnitude, false);
				instance.setParameterByName("liquidDepth", value, false);
				KFMOD.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x00206FF8 File Offset: 0x002051F8
	private void UpdateEntombedVisualizer()
	{
		if (this.IsEntombed)
		{
			if (this.entombedCell == -1)
			{
				int cell = Grid.PosToCell(this);
				if (EntombedItemManager.CanEntomb(this))
				{
					SaveGame.Instance.entombedItemManager.Add(this);
				}
				if (Grid.Objects[cell, 1] == null)
				{
					KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
					if (component != null && Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
					{
						this.entombedCell = cell;
						component.enabled = false;
						this.RemoveFaller();
						return;
					}
				}
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
		}
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x00207088 File Offset: 0x00205288
	private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
	{
		if (this.entombedCell != -1)
		{
			Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
			this.entombedCell = -1;
			base.GetComponent<KBatchedAnimController>().enabled = true;
			if (add_faller_if_necessary)
			{
				this.AddFaller(Vector2.zero);
			}
		}
	}

	// Token: 0x04002286 RID: 8838
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04002287 RID: 8839
	public const float WorkTime = 1.5f;

	// Token: 0x04002288 RID: 8840
	[SerializeField]
	private int _sortOrder;

	// Token: 0x0400228A RID: 8842
	[MyCmpReq]
	[NonSerialized]
	public KPrefabID KPrefabID;

	// Token: 0x0400228B RID: 8843
	[MyCmpAdd]
	[NonSerialized]
	public Clearable Clearable;

	// Token: 0x0400228C RID: 8844
	[MyCmpAdd]
	[NonSerialized]
	public Prioritizable prioritizable;

	// Token: 0x0400228D RID: 8845
	[SerializeField]
	public List<ChoreType> allowedChoreTypes;

	// Token: 0x0400228E RID: 8846
	public bool absorbable;

	// Token: 0x04002290 RID: 8848
	public Func<Pickupable, bool> CanAbsorb = (Pickupable other) => false;

	// Token: 0x04002291 RID: 8849
	public Func<Pickupable, float, Pickupable> OnTake;

	// Token: 0x04002292 RID: 8850
	public Action<Pickupable, bool, Pickupable.Reservation> OnReservationsChanged;

	// Token: 0x04002293 RID: 8851
	public ObjectLayerListItem objectLayerListItem;

	// Token: 0x04002294 RID: 8852
	public Workable targetWorkable;

	// Token: 0x04002295 RID: 8853
	public KAnimFile carryAnimOverride;

	// Token: 0x04002296 RID: 8854
	private KBatchedAnimController lastCarrier;

	// Token: 0x04002297 RID: 8855
	public bool useGunforPickup = true;

	// Token: 0x04002299 RID: 8857
	private static CellOffset[] displacementOffsets = new CellOffset[]
	{
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x0400229A RID: 8858
	private bool isReachable;

	// Token: 0x0400229B RID: 8859
	private bool isEntombed;

	// Token: 0x0400229C RID: 8860
	private bool cleaningUp;

	// Token: 0x0400229E RID: 8862
	public bool trackOnPickup = true;

	// Token: 0x040022A0 RID: 8864
	private int nextTicketNumber;

	// Token: 0x040022A1 RID: 8865
	[Serialize]
	public bool deleteOffGrid = true;

	// Token: 0x040022A2 RID: 8866
	private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();

	// Token: 0x040022A3 RID: 8867
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x040022A4 RID: 8868
	private HandleVector<int>.Handle worldPartitionerEntry;

	// Token: 0x040022A5 RID: 8869
	private HandleVector<int>.Handle storedPartitionerEntry;

	// Token: 0x040022A6 RID: 8870
	private FetchableMonitor.Instance fetchable_monitor;

	// Token: 0x040022A7 RID: 8871
	public bool handleFallerComponents = true;

	// Token: 0x040022A8 RID: 8872
	private LoggerFSSF log;

	// Token: 0x040022AA RID: 8874
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040022AB RID: 8875
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnLanded(data);
	});

	// Token: 0x040022AC RID: 8876
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnOreSizeChanged(data);
	});

	// Token: 0x040022AD RID: 8877
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x040022AE RID: 8878
	private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.RefreshStorageTags(data);
	});

	// Token: 0x040022AF RID: 8879
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.SetWorkableOffset(data);
	});

	// Token: 0x040022B0 RID: 8880
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x040022B1 RID: 8881
	private int entombedCell = -1;

	// Token: 0x02000AC3 RID: 2755
	public struct Reservation
	{
		// Token: 0x06003393 RID: 13203 RVA: 0x000C1A20 File Offset: 0x000BFC20
		public Reservation(GameObject reserver, float amount, int ticket)
		{
			this.reserver = reserver;
			this.amount = amount;
			this.ticket = ticket;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x00207214 File Offset: 0x00205414
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.reserver.name,
				", ",
				this.amount.ToString(),
				", ",
				this.ticket.ToString()
			});
		}

		// Token: 0x040022B2 RID: 8882
		public GameObject reserver;

		// Token: 0x040022B3 RID: 8883
		public float amount;

		// Token: 0x040022B4 RID: 8884
		public int ticket;
	}

	// Token: 0x02000AC4 RID: 2756
	public class PickupableStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x000C1A37 File Offset: 0x000BFC37
		// (set) Token: 0x06003396 RID: 13206 RVA: 0x000C1A3F File Offset: 0x000BFC3F
		public float amount { get; private set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x000C1A48 File Offset: 0x000BFC48
		// (set) Token: 0x06003398 RID: 13208 RVA: 0x000C1A50 File Offset: 0x000BFC50
		public Pickupable originalPickupable { get; private set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06003399 RID: 13209 RVA: 0x000C1A59 File Offset: 0x000BFC59
		// (set) Token: 0x0600339A RID: 13210 RVA: 0x000C1A61 File Offset: 0x000BFC61
		public Action<GameObject> setResultCb { get; private set; }

		// Token: 0x0600339B RID: 13211 RVA: 0x000C1A6A File Offset: 0x000BFC6A
		public PickupableStartWorkInfo(Pickupable pickupable, float amount, Action<GameObject> set_result_cb) : base(pickupable.targetWorkable)
		{
			this.originalPickupable = pickupable;
			this.amount = amount;
			this.setResultCb = set_result_cb;
		}
	}
}
