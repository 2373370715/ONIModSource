using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000FC2 RID: 4034
public class StorageTile : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
	// Token: 0x060051AE RID: 20910 RVA: 0x002726CC File Offset: 0x002708CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.OnStorageChanged)).EventHandler(GameHashes.StorageTileTargetItemChanged, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals));
		this.idle.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
		this.change.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.NoLongerAwaitingForSettingChange)).DefaultState(this.change.awaitingSettingsChange);
		this.change.awaitingSettingsChange.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.StartWorkChore)).Exit(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.CancelWorkChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget, null).WorkableCompleteTransition((StorageTile.Instance smi) => smi.GetWorkable(), this.change.complete);
		this.change.complete.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.ApplySettings)).Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.DropUndesiredItems)).EnterTransition(this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EnterTransition(this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery));
		this.awaitingDelivery.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
	}

	// Token: 0x060051AF RID: 20911 RVA: 0x000D5396 File Offset: 0x000D3596
	public static void DropUndesiredItems(StorageTile.Instance smi)
	{
		smi.DropUndesiredItems();
	}

	// Token: 0x060051B0 RID: 20912 RVA: 0x000D539E File Offset: 0x000D359E
	public static void ApplySettings(StorageTile.Instance smi)
	{
		smi.ApplySettings();
	}

	// Token: 0x060051B1 RID: 20913 RVA: 0x000D53A6 File Offset: 0x000D35A6
	public static void StartWorkChore(StorageTile.Instance smi)
	{
		smi.StartChangeSettingChore();
	}

	// Token: 0x060051B2 RID: 20914 RVA: 0x000D53AE File Offset: 0x000D35AE
	public static void CancelWorkChore(StorageTile.Instance smi)
	{
		smi.CanceChangeSettingChore();
	}

	// Token: 0x060051B3 RID: 20915 RVA: 0x000D53B6 File Offset: 0x000D35B6
	public static void RefreshContentVisuals(StorageTile.Instance smi)
	{
		smi.UpdateContentSymbol();
	}

	// Token: 0x060051B4 RID: 20916 RVA: 0x000D53BE File Offset: 0x000D35BE
	public static bool IsAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return smi.IsPendingChange;
	}

	// Token: 0x060051B5 RID: 20917 RVA: 0x000D53C6 File Offset: 0x000D35C6
	public static bool NoLongerAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange;
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x000D53D1 File Offset: 0x000D35D1
	public static bool HasAnyDesiredItemStored(StorageTile.Instance smi)
	{
		return smi.HasAnyDesiredContents;
	}

	// Token: 0x060051B7 RID: 20919 RVA: 0x000D53D9 File Offset: 0x000D35D9
	public static void OnStorageChanged(StorageTile.Instance smi)
	{
		smi.PlayDoorAnimation();
		StorageTile.RefreshContentVisuals(smi);
	}

	// Token: 0x060051B8 RID: 20920 RVA: 0x000D53E7 File Offset: 0x000D35E7
	public static bool IsAwaitingDelivery(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange && !smi.HasAnyDesiredContents;
	}

	// Token: 0x04003923 RID: 14627
	public const string METER_TARGET = "meter_target";

	// Token: 0x04003924 RID: 14628
	public const string METER_ANIMATION = "meter";

	// Token: 0x04003925 RID: 14629
	public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");

	// Token: 0x04003926 RID: 14630
	public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");

	// Token: 0x04003927 RID: 14631
	public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");

	// Token: 0x04003928 RID: 14632
	public const string ITEM_SYMBOL_ANIMATION = "meter_object";

	// Token: 0x04003929 RID: 14633
	public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");

	// Token: 0x0400392A RID: 14634
	public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");

	// Token: 0x0400392B RID: 14635
	public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";

	// Token: 0x0400392C RID: 14636
	public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");

	// Token: 0x0400392D RID: 14637
	public const string DEFAULT_ANIMATION_NAME = "on";

	// Token: 0x0400392E RID: 14638
	public const string STORAGE_CHANGE_ANIMATION_NAME = "door";

	// Token: 0x0400392F RID: 14639
	public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";

	// Token: 0x04003930 RID: 14640
	public static Tag INVALID_TAG = GameTags.Void;

	// Token: 0x04003931 RID: 14641
	private StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter TargetItemTag = new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter(StorageTile.INVALID_TAG);

	// Token: 0x04003932 RID: 14642
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State idle;

	// Token: 0x04003933 RID: 14643
	public StorageTile.SettingsChangeStates change;

	// Token: 0x04003934 RID: 14644
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingDelivery;

	// Token: 0x02000FC3 RID: 4035
	public class SpecificItemTagSizeInstruction
	{
		// Token: 0x060051BB RID: 20923 RVA: 0x000D5414 File Offset: 0x000D3614
		public SpecificItemTagSizeInstruction(Tag tag, float size)
		{
			this.tag = tag;
			this.sizeMultiplier = size;
		}

		// Token: 0x04003935 RID: 14645
		public Tag tag;

		// Token: 0x04003936 RID: 14646
		public float sizeMultiplier;
	}

	// Token: 0x02000FC4 RID: 4036
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060051BC RID: 20924 RVA: 0x0027294C File Offset: 0x00270B4C
		public StorageTile.SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
		{
			if (this.specialItemCases == null)
			{
				return null;
			}
			KPrefabID component = obj.GetComponent<KPrefabID>();
			foreach (StorageTile.SpecificItemTagSizeInstruction specificItemTagSizeInstruction in this.specialItemCases)
			{
				if (component.HasTag(specificItemTagSizeInstruction.tag))
				{
					return specificItemTagSizeInstruction;
				}
			}
			return null;
		}

		// Token: 0x04003937 RID: 14647
		public float MaxCapacity;

		// Token: 0x04003938 RID: 14648
		public StorageTile.SpecificItemTagSizeInstruction[] specialItemCases;
	}

	// Token: 0x02000FC5 RID: 4037
	public class SettingsChangeStates : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State
	{
		// Token: 0x04003939 RID: 14649
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingSettingsChange;

		// Token: 0x0400393A RID: 14650
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State complete;
	}

	// Token: 0x02000FC6 RID: 4038
	public new class Instance : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.GameInstance, IUserControlledCapacity
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060051BF RID: 20927 RVA: 0x000D5432 File Offset: 0x000D3632
		public Tag TargetTag
		{
			get
			{
				return base.smi.sm.TargetItemTag.Get(base.smi);
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x000D544F File Offset: 0x000D364F
		public bool HasContents
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060051C1 RID: 20929 RVA: 0x000D5463 File Offset: 0x000D3663
		public bool HasAnyDesiredContents
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.AmountOfDesiredContentStored > 0f;
				}
				return !this.HasContents;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060051C2 RID: 20930 RVA: 0x000D548E File Offset: 0x000D368E
		public float AmountOfDesiredContentStored
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.storage.GetMassAvailable(this.TargetTag);
				}
				return 0f;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060051C3 RID: 20931 RVA: 0x000D54B9 File Offset: 0x000D36B9
		public bool IsPendingChange
		{
			get
			{
				return this.GetTreeFilterableCurrentTag() != this.TargetTag;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x000D54CC File Offset: 0x000D36CC
		// (set) Token: 0x060051C5 RID: 20933 RVA: 0x000D54E4 File Offset: 0x000D36E4
		public float UserMaxCapacity
		{
			get
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			set
			{
				this.userMaxCapacity = value;
				this.filteredStorage.FilterChanged();
				this.RefreshAmountMeter();
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060051C6 RID: 20934 RVA: 0x000D54FE File Offset: 0x000D36FE
		public float AmountStored
		{
			get
			{
				return this.storage.MassStored();
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060051C7 RID: 20935 RVA: 0x000BCEBF File Offset: 0x000BB0BF
		public float MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060051C8 RID: 20936 RVA: 0x000D550B File Offset: 0x000D370B
		public float MaxCapacity
		{
			get
			{
				return base.def.MaxCapacity;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060051C9 RID: 20937 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool WholeValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x000C8D02 File Offset: 0x000C6F02
		public LocString CapacityUnits
		{
			get
			{
				return GameUtil.GetCurrentMassUnit(false);
			}
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x000D5518 File Offset: 0x000D3718
		private Tag GetTreeFilterableCurrentTag()
		{
			if (this.treeFilterable.GetTags() != null && this.treeFilterable.GetTags().Count != 0)
			{
				return this.treeFilterable.GetTags().GetRandom<Tag>();
			}
			return StorageTile.INVALID_TAG;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x000D554F File Offset: 0x000D374F
		public StorageTileSwitchItemWorkable GetWorkable()
		{
			return base.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00272994 File Offset: 0x00270B94
		public Instance(IStateMachineTarget master, StorageTile.Def def) : base(master, def)
		{
			this.itemSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_SYMBOL_TARGET, StorageTile.ITEM_SYMBOL_NAME, "meter_object");
			this.itemSymbol.usingNewSymbolOverrideSystem = true;
			this.itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(this.itemSymbol.gameObject);
			this.itemPreviewSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_PREVIEW_SYMBOL_TARGET, StorageTile.ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
			this.defaultItemSymbolScale = this.itemSymbol.transform.localScale.x;
			this.defaultItemLocalPosition = this.itemSymbol.transform.localPosition;
			this.doorSymbol = this.CreateEmptyKAnimController(StorageTile.DOOR_SYMBOL_NAME.ToString());
			this.doorSymbol.initialAnim = "on";
			foreach (KAnim.Build.Symbol symbol in this.doorSymbol.AnimFiles[0].GetData().build.symbols)
			{
				this.doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == StorageTile.DOOR_SYMBOL_NAME);
			}
			this.doorSymbol.transform.SetParent(this.animController.transform, false);
			this.doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
			this.doorSymbol.onAnimComplete += this.OnDoorAnimationCompleted;
			this.doorSymbol.gameObject.SetActive(true);
			this.animController.SetSymbolVisiblity(StorageTile.DOOR_SYMBOL_NAME, false);
			this.doorAnimLink = new KAnimLink(this.animController, this.doorSymbol);
			this.amountMeter = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
			this.filteredStorage = new FilteredStorage(this.storage, null, this, false, fetch_chore_type);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.Subscribe(1606648047, new Action<object>(this.OnObjectReplaced));
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x000D5561 File Offset: 0x000D3761
		public override void StartSM()
		{
			base.StartSM();
			this.filteredStorage.FilterChanged();
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00272BFC File Offset: 0x00270DFC
		private void OnObjectReplaced(object data)
		{
			Constructable.ReplaceCallbackParameters replaceCallbackParameters = (Constructable.ReplaceCallbackParameters)data;
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			if (replaceCallbackParameters.Worker != null)
			{
				foreach (GameObject gameObject in list)
				{
					gameObject.GetComponent<Pickupable>().Trigger(580035959, replaceCallbackParameters.Worker);
				}
			}
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x000D5574 File Offset: 0x000D3774
		private void OnDoorAnimationCompleted(HashedString animName)
		{
			if (animName == "door")
			{
				this.doorSymbol.Play("on", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00272C90 File Offset: 0x00270E90
		private KBatchedAnimController CreateEmptyKAnimController(string name)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("storagetile_kanim")
			};
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingFront;
			return kbatchedAnimController;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00272CEC File Offset: 0x00270EEC
		private KBatchedAnimController CreateSymbolOverrideCapsule(HashedString symbolTarget, HashedString symbolName, string animationName)
		{
			KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolTarget.ToString());
			kbatchedAnimController.initialAnim = animationName;
			bool flag;
			Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolTarget, out flag);
			bool flag2;
			Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolTarget, out flag2);
			Vector3 position = symbolTransform.GetColumn(3);
			Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
			kbatchedAnimController.transform.SetParent(base.transform, false);
			kbatchedAnimController.transform.SetPosition(position);
			Vector3 localPosition = kbatchedAnimController.transform.localPosition;
			localPosition.z = -0.0025f;
			kbatchedAnimController.transform.localPosition = localPosition;
			kbatchedAnimController.transform.localScale = localScale;
			kbatchedAnimController.gameObject.SetActive(false);
			this.animController.SetSymbolVisiblity(symbolTarget, false);
			return kbatchedAnimController;
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00272DC4 File Offset: 0x00270FC4
		private void OnCopySettings(object sourceOBJ)
		{
			if (sourceOBJ != null)
			{
				StorageTile.Instance smi = ((GameObject)sourceOBJ).GetSMI<StorageTile.Instance>();
				if (smi != null)
				{
					this.SetTargetItem(smi.TargetTag);
					this.UserMaxCapacity = smi.UserMaxCapacity;
				}
			}
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x00272DFC File Offset: 0x00270FFC
		public void RefreshAmountMeter()
		{
			float positionPercent = (this.UserMaxCapacity == 0f) ? 0f : Mathf.Clamp(this.AmountOfDesiredContentStored / this.UserMaxCapacity, 0f, 1f);
			this.amountMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x000D55A8 File Offset: 0x000D37A8
		public void PlayDoorAnimation()
		{
			this.doorSymbol.Play("door", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x000D55CA File Offset: 0x000D37CA
		public void SetTargetItem(Tag tag)
		{
			base.sm.TargetItemTag.Set(tag, this, false);
			base.gameObject.Trigger(-2076953849, null);
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x00272E48 File Offset: 0x00271048
		public void ApplySettings()
		{
			Tag treeFilterableCurrentTag = this.GetTreeFilterableCurrentTag();
			this.treeFilterable.RemoveTagFromFilter(treeFilterableCurrentTag);
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x00272E68 File Offset: 0x00271068
		public void DropUndesiredItems()
		{
			Vector3 position = Grid.CellToPos(this.GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			if (this.TargetTag != StorageTile.INVALID_TAG)
			{
				this.treeFilterable.AddTagToFilter(this.TargetTag);
				GameObject[] array = this.storage.DropUnlessHasTag(this.TargetTag);
				if (array != null)
				{
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].transform.SetPosition(position);
					}
				}
			}
			else
			{
				this.storage.DropAll(position, false, false, default(Vector3), true, null);
			}
			this.storage.DropUnlessHasTag(this.TargetTag);
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x00272F58 File Offset: 0x00271158
		public void UpdateContentSymbol()
		{
			this.RefreshAmountMeter();
			bool flag = this.TargetTag == StorageTile.INVALID_TAG;
			if (flag && !this.HasContents)
			{
				this.itemSymbol.gameObject.SetActive(false);
				this.itemPreviewSymbol.gameObject.SetActive(false);
				this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, false);
				return;
			}
			bool flag2 = !flag && (this.IsPendingChange || !this.HasAnyDesiredContents);
			string text = "";
			GameObject gameObject = (this.TargetTag == StorageTile.INVALID_TAG) ? Assets.GetPrefab(this.storage.items[0].PrefabID()) : Assets.GetPrefab(this.TargetTag);
			KAnimFile animFileFromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, flag2 ? "ui" : "", out text);
			this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, flag2);
			this.itemPreviewSymbol.gameObject.SetActive(flag2);
			this.itemSymbol.gameObject.SetActive(!flag2);
			if (flag2)
			{
				this.itemPreviewSymbol.SwapAnims(new KAnimFile[]
				{
					animFileFromPrefabWithTag
				});
				this.itemPreviewSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				string text2 = text;
				if (!string.IsNullOrEmpty(text2))
				{
					this.itemSymbolOverrideController.ApplySymbolOverridesByAffix(animFileFromPrefabWithTag, text2, null, 0);
				}
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			else
			{
				this.itemSymbolOverrideController.RemoveAllSymbolOverrides(0);
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			this.itemSymbol.SwapAnims(new KAnimFile[]
			{
				animFileFromPrefabWithTag
			});
			this.itemSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			StorageTile.SpecificItemTagSizeInstruction sizeInstructionForObject = base.def.GetSizeInstructionForObject(gameObject);
			this.itemSymbol.transform.localScale = Vector3.one * ((sizeInstructionForObject != null) ? sizeInstructionForObject.sizeMultiplier : this.defaultItemSymbolScale);
			KCollider2D component = gameObject.GetComponent<KCollider2D>();
			Vector3 localPosition = this.defaultItemLocalPosition;
			localPosition.y += ((component == null || component is KCircleCollider2D) ? 0f : (-component.offset.y * 0.5f));
			this.itemSymbol.transform.localPosition = localPosition;
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x000D55F1 File Offset: 0x000D37F1
		private void AbortChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Change settings Chore aborted");
				this.chore = null;
			}
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x002731C4 File Offset: 0x002713C4
		public void StartChangeSettingChore()
		{
			this.AbortChore();
			this.chore = new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, this.GetWorkable(), null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x000D5612 File Offset: 0x000D3812
		public void CanceChangeSettingChore()
		{
			this.AbortChore();
		}

		// Token: 0x0400393B RID: 14651
		[Serialize]
		private float userMaxCapacity = float.PositiveInfinity;

		// Token: 0x0400393C RID: 14652
		[MyCmpGet]
		private Storage storage;

		// Token: 0x0400393D RID: 14653
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x0400393E RID: 14654
		[MyCmpGet]
		private TreeFilterable treeFilterable;

		// Token: 0x0400393F RID: 14655
		private FilteredStorage filteredStorage;

		// Token: 0x04003940 RID: 14656
		private Chore chore;

		// Token: 0x04003941 RID: 14657
		private MeterController amountMeter;

		// Token: 0x04003942 RID: 14658
		private KBatchedAnimController doorSymbol;

		// Token: 0x04003943 RID: 14659
		private KBatchedAnimController itemSymbol;

		// Token: 0x04003944 RID: 14660
		private SymbolOverrideController itemSymbolOverrideController;

		// Token: 0x04003945 RID: 14661
		private KBatchedAnimController itemPreviewSymbol;

		// Token: 0x04003946 RID: 14662
		private KAnimLink doorAnimLink;

		// Token: 0x04003947 RID: 14663
		private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

		// Token: 0x04003948 RID: 14664
		private float defaultItemSymbolScale = -1f;

		// Token: 0x04003949 RID: 14665
		private Vector3 defaultItemLocalPosition = Vector3.zero;
	}
}
