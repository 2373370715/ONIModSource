using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IGameObjectEffectDescriptor, IStorage
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060035A5 RID: 13733 RVA: 0x000C2FF3 File Offset: 0x000C11F3
	public bool ShouldOnlyTransferFromLowerPriority
	{
		get
		{
			return this.onlyTransferFromLowerPriority || this.allowItemRemoval;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060035A6 RID: 13734 RVA: 0x000C3005 File Offset: 0x000C1205
	// (set) Token: 0x060035A7 RID: 13735 RVA: 0x000C300D File Offset: 0x000C120D
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x17000251 RID: 593
	public GameObject this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060035A9 RID: 13737 RVA: 0x000C3024 File Offset: 0x000C1224
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060035AA RID: 13738 RVA: 0x000C3031 File Offset: 0x000C1231
	// (set) Token: 0x060035AB RID: 13739 RVA: 0x000C3039 File Offset: 0x000C1239
	public bool ShouldSaveItems
	{
		get
		{
			return this.shouldSaveItems;
		}
		set
		{
			this.shouldSaveItems = value;
		}
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000C3042 File Offset: 0x000C1242
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x000C304A File Offset: 0x000C124A
	public List<GameObject> GetItems()
	{
		return this.items;
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x000C3052 File Offset: 0x000C1252
	public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
	{
		this.defaultStoredItemModifers = modifiers;
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060035AF RID: 13743 RVA: 0x000C305B File Offset: 0x000C125B
	public PrioritySetting masterPriority
	{
		get
		{
			if (this.prioritizable)
			{
				return this.prioritizable.GetMasterPriority();
			}
			return Chore.DefaultPrioritySetting;
		}
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x0020FD34 File Offset: 0x0020DF34
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunForDelivery && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "store", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x0020FD8C File Offset: 0x0020DF8C
	public override Vector3 GetTargetPoint()
	{
		Vector3 vector = base.GetTargetPoint();
		if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
		{
			if (this.rotatable != null)
			{
				vector += this.rotatable.GetRotatedOffset(this.gunTargetOffset);
			}
			else
			{
				vector += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0f);
			}
		}
		return vector;
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060035B2 RID: 13746 RVA: 0x0020FE10 File Offset: 0x0020E010
	// (remove) Token: 0x060035B3 RID: 13747 RVA: 0x0020FE48 File Offset: 0x0020E048
	public event System.Action OnStorageIncreased;

	// Token: 0x060035B4 RID: 13748 RVA: 0x0020FE80 File Offset: 0x0020E080
	protected override void OnPrefabInit()
	{
		if (this.useWideOffsets)
		{
			base.SetOffsetTable(OffsetGroups.InvertedWideTable);
		}
		else
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
		base.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
		base.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.SetupStorageStatusItems();
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x0020FF28 File Offset: 0x0020E128
	private void SetupStorageStatusItems()
	{
		if (Storage.capacityStatusItem == null)
		{
			Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Storage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				Storage storage = (Storage)data;
				float num = storage.MassStored();
				float num2 = storage.capacityKg;
				if (num > num2 - storage.storageFullMargin && num < num2)
				{
					num = num2;
				}
				else
				{
					num = Mathf.Floor(num);
				}
				string newValue = Util.FormatWholeNumber(num);
				IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
				if (component != null)
				{
					num2 = Mathf.Min(component.UserMaxCapacity, num2);
				}
				string newValue2 = Util.FormatWholeNumber(num2);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				if (component != null)
				{
					str = str.Replace("{Units}", component.CapacityUnits);
				}
				else
				{
					str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
				}
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, this);
		}
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x000C307B File Offset: 0x000C127B
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!this.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = false;
		}
		this.UpdateFetchCategory();
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x0020FFE0 File Offset: 0x0020E1E0
	protected override void OnSpawn()
	{
		base.SetWorkTime(this.storageWorkTime);
		foreach (GameObject go in this.items)
		{
			this.ApplyStoredItemModifiers(go, true, true);
			if (this.sendOnStoreOnSpawn)
			{
				go.Trigger(856640610, this);
			}
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.SetSymbolVisiblity("sweep", this.onlyFetchMarkedItems);
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			Prioritizable prioritizable = component2;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		}
		this.UpdateFetchCategory();
		if (this.showUnreachableStatus)
		{
			base.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
			new ReachabilityMonitor.Instance(this).StartSM();
		}
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x002100D8 File Offset: 0x0020E2D8
	public GameObject Store(GameObject go, bool hide_popups = false, bool block_events = false, bool do_disease_transfer = true, bool is_deserializing = false)
	{
		if (go == null)
		{
			return null;
		}
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		GameObject result = go;
		if (!hide_popups && PopFXManager.Instance != null)
		{
			LocString loc_string;
			Transform transform;
			if (this.fxPrefix == Storage.FXPrefix.Delivered)
			{
				loc_string = UI.DELIVERED;
				transform = base.transform;
			}
			else
			{
				loc_string = UI.PICKEDUP;
				transform = go.transform;
			}
			string text;
			if (!Assets.IsTagCountable(go.PrefabID()))
			{
				text = string.Format(loc_string, GameUtil.GetFormattedMass(component.Units, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), go.GetProperName());
			}
			else
			{
				text = string.Format(loc_string, (int)component.Units, go.GetProperName());
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, transform, this.storageFXOffset, 1.5f, false, false);
		}
		go.transform.parent = base.transform;
		Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Move);
		position.z = go.transform.GetPosition().z;
		go.transform.SetPosition(position);
		if (!block_events && do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		if (!is_deserializing)
		{
			Pickupable component2 = go.GetComponent<Pickupable>();
			if (component2 != null)
			{
				if (component2 != null && component2.prevent_absorb_until_stored)
				{
					component2.prevent_absorb_until_stored = false;
				}
				foreach (GameObject gameObject in this.items)
				{
					if (gameObject != null)
					{
						Pickupable component3 = gameObject.GetComponent<Pickupable>();
						if (component3 != null && component3.TryAbsorb(component2, hide_popups, true))
						{
							if (!block_events)
							{
								base.Trigger(-1697596308, go);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(go);
								}
								base.Trigger(-778359855, this);
								if (this.OnStorageIncreased != null)
								{
									this.OnStorageIncreased();
								}
							}
							this.ApplyStoredItemModifiers(go, true, false);
							result = gameObject;
							go = null;
							break;
						}
					}
				}
			}
		}
		if (go != null)
		{
			this.items.Add(go);
			if (!is_deserializing)
			{
				this.ApplyStoredItemModifiers(go, true, false);
			}
			if (!block_events)
			{
				go.Trigger(856640610, this);
				base.Trigger(-1697596308, go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(go);
				}
				base.Trigger(-778359855, this);
				if (this.OnStorageIncreased != null)
				{
					this.OnStorageIncreased();
				}
			}
		}
		return result;
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x0021036C File Offset: 0x0020E56C
	public PrimaryElement AddElement(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		Element element2 = ElementLoader.FindElementByHash(element);
		if (element2.IsGas)
		{
			return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsLiquid)
		{
			return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsSolid)
		{
			return this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		return null;
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x002103D0 File Offset: 0x0020E5D0
	public PrimaryElement AddOre(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			GameObject gameObject = element2.substance.SpawnResource(base.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
			gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
			element2.substance.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
			this.Store(gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x002104B4 File Offset: 0x0020E6B4
	public PrimaryElement AddLiquid(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x00210588 File Offset: 0x0020E788
	public PrimaryElement AddGasChunk(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float mass2 = primaryElement.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass2, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.SetMassTemperature(mass2 + mass, finalTemperature);
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x000C3092 File Offset: 0x000C1292
	public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
	{
		while (this.items.Count > 0)
		{
			this.Transfer(this.items[0], target, block_events, hide_popups);
		}
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00210650 File Offset: 0x0020E850
	public bool TransferMass(Storage dest_storage, Tag tag, float amount, bool flatten = false, bool block_events = false, bool hide_popups = false)
	{
		float num = amount;
		while (num > 0f && this.GetAmountAvailable(tag) > 0f)
		{
			num -= this.Transfer(dest_storage, tag, num, block_events, hide_popups);
		}
		if (flatten)
		{
			dest_storage.Flatten(tag);
		}
		return num <= 0f;
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x002106A0 File Offset: 0x0020E8A0
	public float Transfer(Storage dest_storage, Tag tag, float amount, bool block_events = false, bool hide_popups = false)
	{
		GameObject gameObject = this.FindFirst(tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (amount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.Take(amount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, component2.gameObject);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(component2.gameObject);
					}
				}
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				amount = component.Units;
			}
			return amount;
		}
		return 0f;
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x0021073C File Offset: 0x0020E93C
	public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
	{
		this.items.RemoveAll((GameObject it) => it == null);
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.items[i] == go)
			{
				this.items.RemoveAt(i);
				this.ApplyStoredItemModifiers(go, false, false);
				target.Store(go, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, go);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(go);
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x002107E8 File Offset: 0x0020E9E8
	public bool DropSome(Tag tag, float amount, bool ventGas = false, bool dumpLiquid = false, Vector3 offset = default(Vector3), bool doDiseaseTransfer = true, bool showInWorldNotification = false)
	{
		bool result = false;
		float num = amount;
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(tag, pooledList);
		foreach (GameObject gameObject in pooledList)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component)
			{
				Pickupable pickupable = component.Take(num);
				if (pickupable != null)
				{
					bool flag = false;
					if (ventGas || dumpLiquid)
					{
						Dumpable component2 = pickupable.GetComponent<Dumpable>();
						if (component2 != null)
						{
							if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
							if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange2 = this.OnStorageChange;
								if (onStorageChange2 != null)
								{
									onStorageChange2(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
						}
					}
					if (!flag)
					{
						Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) + offset;
						pickupable.transform.SetPosition(position);
						KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
						if (component3)
						{
							component3.SetSceneLayer(Grid.SceneLayer.Ore);
						}
						num -= pickupable.GetComponent<PrimaryElement>().Mass;
						this.MakeWorldActive(pickupable.gameObject);
						base.Trigger(-1697596308, pickupable.gameObject);
						Action<GameObject> onStorageChange3 = this.OnStorageChange;
						if (onStorageChange3 != null)
						{
							onStorageChange3(pickupable.gameObject);
						}
						result = true;
						if (showInWorldNotification)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
						}
					}
				}
			}
			if (num <= 0f)
			{
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x00210B3C File Offset: 0x0020ED3C
	public void DropAll(Vector3 position, bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		while (this.items.Count > 0)
		{
			GameObject gameObject = this.items[0];
			if (do_disease_transfer)
			{
				this.TransferDiseaseWithObject(gameObject);
			}
			this.items.RemoveAt(0);
			if (gameObject != null)
			{
				bool flag = false;
				if (vent_gas || dump_liquid)
				{
					Dumpable component = gameObject.GetComponent<Dumpable>();
					if (component != null)
					{
						if (vent_gas && gameObject.GetComponent<PrimaryElement>().Element.IsGas)
						{
							component.Dump(position + offset);
							flag = true;
						}
						if (dump_liquid && gameObject.GetComponent<PrimaryElement>().Element.IsLiquid)
						{
							component.Dump(position + offset);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					gameObject.transform.SetPosition(position + offset);
					KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
					if (component2)
					{
						component2.SetSceneLayer(Grid.SceneLayer.Ore);
					}
					this.MakeWorldActive(gameObject);
					if (collect_dropped_items != null)
					{
						collect_dropped_items.Add(gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000C30BA File Offset: 0x000C12BA
	public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		this.DropAll(Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x00210C34 File Offset: 0x0020EE34
	public void Drop(Tag t, List<GameObject> obj_list)
	{
		this.Find(t, obj_list);
		foreach (GameObject go in obj_list)
		{
			this.Drop(go, true);
		}
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x00210C90 File Offset: 0x0020EE90
	public void Drop(Tag t)
	{
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(t, pooledList);
		foreach (GameObject go in pooledList)
		{
			this.Drop(go, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x00210CF8 File Offset: 0x0020EEF8
	public void DropUnlessMatching(FetchChore chore)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				KPrefabID component = this.items[i].GetComponent<KPrefabID>();
				if (!(((chore.criteria == FetchChore.MatchCriteria.MatchID && chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && component.HasTag(chore.tagsFirst))) & (!chore.requiredTag.IsValid || component.HasTag(chore.requiredTag)) & !component.HasAnyTags(chore.forbiddenTags)))
				{
					GameObject gameObject = this.items[i];
					this.items.RemoveAt(i);
					i--;
					this.TransferDiseaseWithObject(gameObject);
					this.MakeWorldActive(gameObject);
				}
			}
		}
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x00210DDC File Offset: 0x0020EFDC
	public GameObject[] DropUnlessHasTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && !this.items[i].GetComponent<KPrefabID>().HasTag(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x00210E94 File Offset: 0x0020F094
	public GameObject[] DropHasTags(Tag[] tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && this.items[i].GetComponent<KPrefabID>().HasAllTags(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x00210F4C File Offset: 0x0020F14C
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		if (go == null)
		{
			return null;
		}
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(go != this.items[i]))
			{
				this.items[i] = this.items[count - 1];
				this.items.RemoveAt(count - 1);
				if (do_disease_transfer)
				{
					this.TransferDiseaseWithObject(go);
				}
				this.MakeWorldActive(go);
				break;
			}
		}
		return go;
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x00210FCC File Offset: 0x0020F1CC
	public void RenotifyAll()
	{
		this.items.RemoveAll((GameObject it) => it == null);
		foreach (GameObject go in this.items)
		{
			go.Trigger(856640610, this);
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x00211050 File Offset: 0x0020F250
	private void TransferDiseaseWithObject(GameObject obj)
	{
		if (obj == null || !this.doDiseaseTransfer || this.primaryElement == null)
		{
			return;
		}
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component.DiseaseIdx;
		invalid.count = (int)((float)component.DiseaseCount * 0.05f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = this.primaryElement.DiseaseIdx;
		invalid2.count = (int)((float)this.primaryElement.DiseaseCount * 0.05f);
		component.ModifyDiseaseCount(-invalid.count, "Storage.TransferDiseaseWithObject");
		this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
		if (invalid.count > 0)
		{
			this.primaryElement.AddDisease(invalid.idx, invalid.count, "Storage.TransferDiseaseWithObject");
		}
		if (invalid2.count > 0)
		{
			component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x00211158 File Offset: 0x0020F358
	private void MakeWorldActive(GameObject go)
	{
		go.transform.parent = null;
		if (this.dropOffset != Vector2.zero)
		{
			go.transform.Translate(this.dropOffset);
		}
		go.Trigger(856640610, null);
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
		if (go != null)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.KeepZeroMassObject = false;
				if (component.Mass <= 0f)
				{
					Util.KDestroyGameObject(go);
				}
			}
		}
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x00211208 File Offset: 0x0020F408
	public List<GameObject> Find(Tag tag, List<GameObject> result)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result.Add(gameObject);
			}
		}
		return result;
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x00211254 File Offset: 0x0020F454
	public GameObject FindFirst(Tag tag)
	{
		GameObject result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result = gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x002112A0 File Offset: 0x0020F4A0
	public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0f)
	{
		PrimaryElement result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Mass > 0f && component.Mass >= mass)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x00211308 File Offset: 0x0020F508
	private void Flatten(Tag tag_to_combine)
	{
		GameObject gameObject = this.FindFirst(tag_to_combine);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject2 = this.items[i];
			if (gameObject2.HasTag(tag_to_combine) && gameObject2 != gameObject)
			{
				PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
				component.Mass += component2.Mass;
				this.ConsumeIgnoringDisease(gameObject2);
			}
		}
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x00211388 File Offset: 0x0020F588
	public HashSet<Tag> GetAllIDsInStorage()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject go = this.items[i];
			hashSet.Add(go.PrefabID());
		}
		return hashSet;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x002113CC File Offset: 0x0020F5CC
	public GameObject Find(int ID)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (ID == gameObject.PrefabID().GetHashCode())
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x000C30D6 File Offset: 0x000C12D6
	public void ConsumeAllIgnoringDisease()
	{
		this.ConsumeAllIgnoringDisease(Tag.Invalid);
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x00211418 File Offset: 0x0020F618
	public void ConsumeAllIgnoringDisease(Tag tag)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (!(tag != Tag.Invalid) || this.items[i].HasTag(tag))
			{
				this.ConsumeIgnoringDisease(this.items[i]);
			}
		}
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00211470 File Offset: 0x0020F670
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature)
	{
		DebugUtil.Assert(tag.IsValid);
		amount_consumed = 0f;
		disease_info = SimUtil.DiseaseInfo.Invalid;
		aggregate_temperature = 0f;
		bool flag = false;
		int num = 0;
		while (num < this.items.Count && amount > 0f)
		{
			GameObject gameObject = this.items[num];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Units > 0f)
				{
					flag = true;
					float num2 = Math.Min(component.Units, amount);
					global::Debug.Assert(num2 > 0f, "Delta amount was zero, which should be impossible.");
					aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, num2, component.Temperature);
					SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, num2 / component.Units);
					disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
					component.Units -= num2;
					component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
					amount -= num2;
					amount_consumed += num2;
				}
				if (component.Units <= 0f && !component.KeepZeroMassObject)
				{
					if (this.deleted_objects == null)
					{
						this.deleted_objects = new List<GameObject>();
					}
					this.deleted_objects.Add(gameObject);
				}
				base.Trigger(-1697596308, gameObject);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange != null)
				{
					onStorageChange(gameObject);
				}
			}
			num++;
		}
		if (!flag)
		{
			aggregate_temperature = base.GetComponent<PrimaryElement>().Temperature;
		}
		if (this.deleted_objects != null)
		{
			for (int i = 0; i < this.deleted_objects.Count; i++)
			{
				this.items.Remove(this.deleted_objects[i]);
				Util.KDestroyGameObject(this.deleted_objects[i]);
			}
			this.deleted_objects.Clear();
		}
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x00211654 File Offset: 0x0020F854
	public void ConsumeAndGetDisease(Recipe.Ingredient ingredient, out SimUtil.DiseaseInfo disease_info, out float temperature)
	{
		float num;
		this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out disease_info, out temperature);
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00211678 File Offset: 0x0020F878
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.ConsumeAndGetDisease(tag, amount, out num, out diseaseInfo, out num2);
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00211694 File Offset: 0x0020F894
	public void ConsumeIgnoringDisease(GameObject item_go)
	{
		if (this.items.Contains(item_go))
		{
			PrimaryElement component = item_go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.Units = 0f;
				component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange == null)
				{
					return;
				}
				onStorageChange(item_go);
				return;
			}
			else
			{
				this.items.Remove(item_go);
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(item_go);
				}
				item_go.DeleteObject();
			}
		}
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x000C30E3 File Offset: 0x000C12E3
	public GameObject Drop(int ID)
	{
		return this.Drop(this.Find(ID), true);
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x00211738 File Offset: 0x0020F938
	private void OnDeath(object data)
	{
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = true;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x000C30F3 File Offset: 0x000C12F3
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x000C3105 File Offset: 0x000C1305
	public bool IsEmpty()
	{
		return this.items.Count == 0;
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x000C3115 File Offset: 0x000C1315
	public float Capacity()
	{
		return this.capacityKg;
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x000C311D File Offset: 0x000C131D
	public bool IsEndOfLife()
	{
		return this.endOfLife;
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x00211774 File Offset: 0x0020F974
	public float ExactMassStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units * component.MassPerUnit;
				}
			}
		}
		return num;
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x000C3125 File Offset: 0x000C1325
	public float MassStored()
	{
		return (float)Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x002117E0 File Offset: 0x0020F9E0
	public float UnitsStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units;
				}
			}
		}
		return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00211854 File Offset: 0x0020FA54
	public bool Has(Tag tag)
	{
		bool result = false;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.HasTag(tag) && component.Mass > 0f)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x002118D0 File Offset: 0x0020FAD0
	public PrimaryElement AddToPrimaryElement(SimHashes element, float additional_mass, float temperature)
	{
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
			primaryElement.Mass += additional_mass;
			primaryElement.Temperature = finalTemperature;
		}
		return primaryElement;
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00211918 File Offset: 0x0020FB18
	public PrimaryElement FindPrimaryElement(SimHashes element)
	{
		PrimaryElement result = null;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x000C313F File Offset: 0x000C133F
	public float RemainingCapacity()
	{
		return this.capacityKg - this.MassStored();
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x000C314E File Offset: 0x000C134E
	public bool GetOnlyFetchMarkedItems()
	{
		return this.onlyFetchMarkedItems;
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x000C3156 File Offset: 0x000C1356
	public void SetOnlyFetchMarkedItems(bool is_set)
	{
		if (is_set != this.onlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = is_set;
			this.UpdateFetchCategory();
			base.Trigger(644822890, null);
			base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("sweep", is_set);
		}
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x000C3190 File Offset: 0x000C1390
	private void UpdateFetchCategory()
	{
		if (this.fetchCategory == Storage.FetchCategory.Building)
		{
			return;
		}
		this.fetchCategory = (this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage);
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x000C31AD File Offset: 0x000C13AD
	protected override void OnCleanUp()
	{
		if (this.items.Count != 0)
		{
			global::Debug.LogWarning("Storage for [" + base.gameObject.name + "] is being destroyed but it still contains items!", base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x00211984 File Offset: 0x0020FB84
	private void OnQueueDestroyObject(object data)
	{
		this.endOfLife = true;
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = false;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
		this.OnCleanUp();
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x000C31E7 File Offset: 0x000C13E7
	public void Remove(GameObject go, bool do_disease_transfer = true)
	{
		this.items.Remove(go);
		if (do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x002119D0 File Offset: 0x0020FBD0
	public bool ForceStore(Tag tag, float amount)
	{
		global::Debug.Assert(amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				gameObject.GetComponent<PrimaryElement>().Mass += amount;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x00211A38 File Offset: 0x0020FC38
	public float GetAmountAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x00211A90 File Offset: 0x0020FC90
	public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmountAvailable(tag);
		}
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag) && !gameObject.HasAnyTags(forbiddenTags))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x00211A38 File Offset: 0x0020FC38
	public float GetUnitsAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x00211AFC File Offset: 0x0020FCFC
	public float GetMassAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x00211B54 File Offset: 0x0020FD54
	public float GetMassAvailable(SimHashes element)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					num += component.Mass;
				}
			}
		}
		return num;
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x00211BB0 File Offset: 0x0020FDB0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.showDescriptor)
		{
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x00211C20 File Offset: 0x0020FE20
	public static void MakeItemTemperatureInsulated(GameObject go, bool is_stored, bool is_initializing)
	{
		SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		component.enabled = !is_stored;
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x00211C48 File Offset: 0x0020FE48
	public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
	{
		if (is_initializing)
		{
			return;
		}
		bool flag = !is_stored;
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != flag)
		{
			component.enabled = flag;
		}
		KSelectable component2 = go.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != flag)
		{
			component2.enabled = flag;
		}
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x000C3227 File Offset: 0x000C1427
	public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000C3257 File Offset: 0x000C1457
	public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
		}
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x00211CA0 File Offset: 0x0020FEA0
	private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
	{
		List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
		for (int i = 0; i < list.Count; i++)
		{
			Storage.StoredItemModifier storedItemModifier = list[i];
			for (int j = 0; j < Storage.StoredItemModifierHandlers.Count; j++)
			{
				Storage.StoredItemModifierInfo storedItemModifierInfo = Storage.StoredItemModifierHandlers[j];
				if (storedItemModifierInfo.modifier == storedItemModifier)
				{
					storedItemModifierInfo.toggleState(go, is_stored, is_initializing);
					break;
				}
			}
		}
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x00211D0C File Offset: 0x0020FF0C
	protected virtual void OnCopySettings(object data)
	{
		Storage component = ((GameObject)data).GetComponent<Storage>();
		if (component != null)
		{
			this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
		}
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x00211D3C File Offset: 0x0020FF3C
	private void OnPriorityChanged(PrioritySetting priority)
	{
		foreach (GameObject go in this.items)
		{
			go.Trigger(-1626373771, this);
		}
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x00211D94 File Offset: 0x0020FF94
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, this);
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x00211DE0 File Offset: 0x0020FFE0
	public void SetContentsDeleteOffGrid(bool delete_off_grid)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			Pickupable component = this.items[i].GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = delete_off_grid;
			}
			Storage component2 = this.items[i].GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(delete_off_grid);
			}
		}
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x00211E48 File Offset: 0x00210048
	private bool ShouldSaveItem(GameObject go)
	{
		if (!this.shouldSaveItems)
		{
			return false;
		}
		bool result = false;
		if (go != null && go.GetComponent<SaveLoadRoot>() != null && go.GetComponent<PrimaryElement>().Mass > 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x00211E90 File Offset: 0x00210090
	public void Serialize(BinaryWriter writer)
	{
		int num = 0;
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ShouldSaveItem(this.items[i]))
			{
				num++;
			}
		}
		writer.Write(num);
		if (num == 0)
		{
			return;
		}
		if (this.items != null && this.items.Count > 0)
		{
			for (int j = 0; j < this.items.Count; j++)
			{
				GameObject gameObject = this.items[j];
				if (this.ShouldSaveItem(gameObject))
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
						writer.WriteKleiString(name);
						component.Save(writer);
					}
					else
					{
						global::Debug.Log("Tried to save obj in storage but obj has no SaveLoadRoot", gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00211F6C File Offset: 0x0021016C
	public void Deserialize(IReader reader)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.ClearItems();
		int num4 = reader.ReadInt32();
		this.items = new List<GameObject>(num4);
		for (int i = 0; i < num4; i++)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			Tag tag = TagManager.Create(reader.ReadKleiString());
			SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
			num += Time.realtimeSinceStartup - realtimeSinceStartup2;
			if (saveLoadRoot != null)
			{
				KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.enabled = false;
				}
				saveLoadRoot.SetRegistered(false);
				float realtimeSinceStartup3 = Time.realtimeSinceStartup;
				GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
				num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
				if (gameObject != null)
				{
					Pickupable component2 = gameObject.GetComponent<Pickupable>();
					if (component2 != null)
					{
						float realtimeSinceStartup4 = Time.realtimeSinceStartup;
						component2.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
					}
					Storable component3 = gameObject.GetComponent<Storable>();
					if (component3 != null)
					{
						float realtimeSinceStartup5 = Time.realtimeSinceStartup;
						component3.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
					}
					if (this.dropOnLoad)
					{
						this.Drop(saveLoadRoot.gameObject, true);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("Tried to deserialize " + tag.ToString() + " into storage but failed", base.gameObject);
			}
		}
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x002120E8 File Offset: 0x002102E8
	private void ClearItems()
	{
		foreach (GameObject go in this.items)
		{
			go.DeleteObject();
		}
		this.items.Clear();
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x00212144 File Offset: 0x00210344
	public void UpdateStoredItemCachedCells()
	{
		foreach (GameObject gameObject in this.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component != null)
			{
				component.UpdateCachedCellFromStoragePosition();
			}
		}
	}

	// Token: 0x0400247D RID: 9341
	public bool allowItemRemoval;

	// Token: 0x0400247E RID: 9342
	public bool ignoreSourcePriority;

	// Token: 0x0400247F RID: 9343
	public bool onlyTransferFromLowerPriority;

	// Token: 0x04002480 RID: 9344
	public float capacityKg = 20000f;

	// Token: 0x04002481 RID: 9345
	public bool showDescriptor;

	// Token: 0x04002483 RID: 9347
	public bool doDiseaseTransfer = true;

	// Token: 0x04002484 RID: 9348
	public List<Tag> storageFilters;

	// Token: 0x04002485 RID: 9349
	public bool useGunForDelivery = true;

	// Token: 0x04002486 RID: 9350
	public bool sendOnStoreOnSpawn;

	// Token: 0x04002487 RID: 9351
	public bool showInUI = true;

	// Token: 0x04002488 RID: 9352
	public bool storeDropsFromButcherables;

	// Token: 0x04002489 RID: 9353
	public bool allowClearable;

	// Token: 0x0400248A RID: 9354
	public bool showCapacityStatusItem;

	// Token: 0x0400248B RID: 9355
	public bool showCapacityAsMainStatus;

	// Token: 0x0400248C RID: 9356
	public bool showUnreachableStatus;

	// Token: 0x0400248D RID: 9357
	public bool showSideScreenTitleBar;

	// Token: 0x0400248E RID: 9358
	public bool useWideOffsets;

	// Token: 0x0400248F RID: 9359
	public Action<List<GameObject>> onDestroyItemsDropped;

	// Token: 0x04002490 RID: 9360
	public Action<GameObject> OnStorageChange;

	// Token: 0x04002491 RID: 9361
	public Vector2 dropOffset = Vector2.zero;

	// Token: 0x04002492 RID: 9362
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002493 RID: 9363
	public Vector2 gunTargetOffset;

	// Token: 0x04002494 RID: 9364
	public Storage.FetchCategory fetchCategory;

	// Token: 0x04002495 RID: 9365
	public int storageNetworkID = -1;

	// Token: 0x04002496 RID: 9366
	public Tag storageID = GameTags.StoragesIds.DefaultStorage;

	// Token: 0x04002497 RID: 9367
	public float storageFullMargin;

	// Token: 0x04002498 RID: 9368
	public Vector3 storageFXOffset = Vector3.zero;

	// Token: 0x04002499 RID: 9369
	private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x0400249A RID: 9370
	public Storage.FXPrefix fxPrefix;

	// Token: 0x0400249B RID: 9371
	public List<GameObject> items = new List<GameObject>();

	// Token: 0x0400249C RID: 9372
	[MyCmpGet]
	public Prioritizable prioritizable;

	// Token: 0x0400249D RID: 9373
	[MyCmpGet]
	public Automatable automatable;

	// Token: 0x0400249E RID: 9374
	[MyCmpGet]
	protected PrimaryElement primaryElement;

	// Token: 0x0400249F RID: 9375
	public bool dropOnLoad;

	// Token: 0x040024A0 RID: 9376
	protected float maxKGPerItem = float.MaxValue;

	// Token: 0x040024A1 RID: 9377
	private bool endOfLife;

	// Token: 0x040024A2 RID: 9378
	public bool allowSettingOnlyFetchMarkedItems = true;

	// Token: 0x040024A3 RID: 9379
	[Serialize]
	private bool onlyFetchMarkedItems;

	// Token: 0x040024A4 RID: 9380
	[Serialize]
	private bool shouldSaveItems = true;

	// Token: 0x040024A5 RID: 9381
	public float storageWorkTime = 1.5f;

	// Token: 0x040024A6 RID: 9382
	private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>
	{
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
	};

	// Token: 0x040024A7 RID: 9383
	[SerializeField]
	private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};

	// Token: 0x040024A8 RID: 9384
	public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	// Token: 0x040024A9 RID: 9385
	public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x040024AA RID: 9386
	public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x040024AC RID: 9388
	private static StatusItem capacityStatusItem;

	// Token: 0x040024AD RID: 9389
	private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, delegate(Storage component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040024AE RID: 9390
	private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnQueueDestroyObject(data);
	});

	// Token: 0x040024AF RID: 9391
	private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040024B0 RID: 9392
	private List<GameObject> deleted_objects;

	// Token: 0x02000B1E RID: 2846
	public enum StoredItemModifier
	{
		// Token: 0x040024B2 RID: 9394
		Insulate,
		// Token: 0x040024B3 RID: 9395
		Hide,
		// Token: 0x040024B4 RID: 9396
		Seal,
		// Token: 0x040024B5 RID: 9397
		Preserve
	}

	// Token: 0x02000B1F RID: 2847
	public enum FetchCategory
	{
		// Token: 0x040024B7 RID: 9399
		Building,
		// Token: 0x040024B8 RID: 9400
		GeneralStorage,
		// Token: 0x040024B9 RID: 9401
		StorageSweepOnly
	}

	// Token: 0x02000B20 RID: 2848
	public enum FXPrefix
	{
		// Token: 0x040024BB RID: 9403
		Delivered,
		// Token: 0x040024BC RID: 9404
		PickedUp
	}

	// Token: 0x02000B21 RID: 2849
	private struct StoredItemModifierInfo
	{
		// Token: 0x06003603 RID: 13827 RVA: 0x000C3287 File Offset: 0x000C1487
		public StoredItemModifierInfo(Storage.StoredItemModifier modifier, Action<GameObject, bool, bool> toggle_state)
		{
			this.modifier = modifier;
			this.toggleState = toggle_state;
		}

		// Token: 0x040024BD RID: 9405
		public Storage.StoredItemModifier modifier;

		// Token: 0x040024BE RID: 9406
		public Action<GameObject, bool, bool> toggleState;
	}
}
