using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class StorageTile : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
	public class SpecificItemTagSizeInstruction
	{
		public Tag tag;

		public float sizeMultiplier;

		public SpecificItemTagSizeInstruction(Tag tag, float size)
		{
			this.tag = tag;
			sizeMultiplier = size;
		}
	}

	public class Def : BaseDef
	{
		public float MaxCapacity;

		public SpecificItemTagSizeInstruction[] specialItemCases;

		public SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
		{
			if (specialItemCases == null)
			{
				return null;
			}
			KPrefabID component = obj.GetComponent<KPrefabID>();
			SpecificItemTagSizeInstruction[] array = specialItemCases;
			foreach (SpecificItemTagSizeInstruction specificItemTagSizeInstruction in array)
			{
				if (component.HasTag(specificItemTagSizeInstruction.tag))
				{
					return specificItemTagSizeInstruction;
				}
			}
			return null;
		}
	}

	public class SettingsChangeStates : State
	{
		public State awaitingSettingsChange;

		public State complete;
	}

	public new class Instance : GameInstance, IUserControlledCapacity
	{
		[Serialize]
		private float userMaxCapacity = float.PositiveInfinity;

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private KBatchedAnimController animController;

		[MyCmpGet]
		private TreeFilterable treeFilterable;

		private FilteredStorage filteredStorage;

		private Chore chore;

		private MeterController amountMeter;

		private KBatchedAnimController doorSymbol;

		private KBatchedAnimController itemSymbol;

		private SymbolOverrideController itemSymbolOverrideController;

		private KBatchedAnimController itemPreviewSymbol;

		private KAnimLink doorAnimLink;

		private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

		private float defaultItemSymbolScale = -1f;

		private Vector3 defaultItemLocalPosition = Vector3.zero;

		public Tag TargetTag => base.smi.sm.TargetItemTag.Get(base.smi);

		public bool HasContents => storage.MassStored() > 0f;

		public bool HasAnyDesiredContents
		{
			get
			{
				if (!(TargetTag == INVALID_TAG))
				{
					return AmountOfDesiredContentStored > 0f;
				}
				return !HasContents;
			}
		}

		public float AmountOfDesiredContentStored
		{
			get
			{
				if (!(TargetTag == INVALID_TAG))
				{
					return storage.GetMassAvailable(TargetTag);
				}
				return 0f;
			}
		}

		public bool IsPendingChange => GetTreeFilterableCurrentTag() != TargetTag;

		public float UserMaxCapacity
		{
			get
			{
				return Mathf.Min(userMaxCapacity, storage.capacityKg);
			}
			set
			{
				userMaxCapacity = value;
				filteredStorage.FilterChanged();
				RefreshAmountMeter();
			}
		}

		public float AmountStored => storage.MassStored();

		public float MinCapacity => 0f;

		public float MaxCapacity => base.def.MaxCapacity;

		public bool WholeValues => false;

		public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

		private Tag GetTreeFilterableCurrentTag()
		{
			if (treeFilterable.GetTags() != null && treeFilterable.GetTags().Count != 0)
			{
				return treeFilterable.GetTags().GetRandom();
			}
			return INVALID_TAG;
		}

		public StorageTileSwitchItemWorkable GetWorkable()
		{
			return base.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			itemSymbol = CreateSymbolOverrideCapsule(ITEM_SYMBOL_TARGET, ITEM_SYMBOL_NAME, "meter_object");
			itemSymbol.usingNewSymbolOverrideSystem = true;
			itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(itemSymbol.gameObject);
			itemPreviewSymbol = CreateSymbolOverrideCapsule(ITEM_PREVIEW_SYMBOL_TARGET, ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
			defaultItemSymbolScale = itemSymbol.transform.localScale.x;
			defaultItemLocalPosition = itemSymbol.transform.localPosition;
			doorSymbol = CreateEmptyKAnimController(DOOR_SYMBOL_NAME.ToString());
			doorSymbol.initialAnim = "on";
			KAnim.Build.Symbol[] symbols = doorSymbol.AnimFiles[0].GetData().build.symbols;
			foreach (KAnim.Build.Symbol symbol in symbols)
			{
				doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == DOOR_SYMBOL_NAME);
			}
			doorSymbol.transform.SetParent(animController.transform, worldPositionStays: false);
			doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
			doorSymbol.onAnimComplete += OnDoorAnimationCompleted;
			doorSymbol.gameObject.SetActive(value: true);
			animController.SetSymbolVisiblity(DOOR_SYMBOL_NAME, is_visible: false);
			doorAnimLink = new KAnimLink(animController, doorSymbol);
			amountMeter = new MeterController(animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
			ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(choreTypeID);
			filteredStorage = new FilteredStorage(storage, null, this, use_logic_meter: false, fetch_chore_type);
			Subscribe(-905833192, OnCopySettings);
			Subscribe(1606648047, OnObjectReplaced);
		}

		public override void StartSM()
		{
			base.StartSM();
			filteredStorage.FilterChanged();
		}

		private void OnObjectReplaced(object data)
		{
			Constructable.ReplaceCallbackParameters replaceCallbackParameters = (Constructable.ReplaceCallbackParameters)data;
			List<GameObject> list = new List<GameObject>();
			storage.DropAll(vent_gas: false, dump_liquid: false, default(Vector3), do_disease_transfer: true, list);
			if (!(replaceCallbackParameters.Worker != null))
			{
				return;
			}
			foreach (GameObject item in list)
			{
				item.GetComponent<Pickupable>().Trigger(580035959, replaceCallbackParameters.Worker);
			}
		}

		private void OnDoorAnimationCompleted(HashedString animName)
		{
			if (animName == "door")
			{
				doorSymbol.Play("on");
			}
		}

		private KBatchedAnimController CreateEmptyKAnimController(string name)
		{
			GameObject obj = new GameObject(base.gameObject.name + "-" + name);
			obj.SetActive(value: false);
			KBatchedAnimController kBatchedAnimController = obj.AddComponent<KBatchedAnimController>();
			kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim("storagetile_kanim") };
			kBatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingFront;
			return kBatchedAnimController;
		}

		private KBatchedAnimController CreateSymbolOverrideCapsule(HashedString symbolTarget, HashedString symbolName, string animationName)
		{
			KBatchedAnimController kBatchedAnimController = CreateEmptyKAnimController(symbolTarget.ToString());
			kBatchedAnimController.initialAnim = animationName;
			bool symbolVisible;
			Matrix4x4 symbolTransform = animController.GetSymbolTransform(symbolTarget, out symbolVisible);
			bool symbolVisible2;
			Matrix2x3 symbolLocalTransform = animController.GetSymbolLocalTransform(symbolTarget, out symbolVisible2);
			Vector3 position = symbolTransform.GetColumn(3);
			Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
			kBatchedAnimController.transform.SetParent(base.transform, worldPositionStays: false);
			kBatchedAnimController.transform.SetPosition(position);
			Vector3 localPosition = kBatchedAnimController.transform.localPosition;
			localPosition.z = -0.0025f;
			kBatchedAnimController.transform.localPosition = localPosition;
			kBatchedAnimController.transform.localScale = localScale;
			kBatchedAnimController.gameObject.SetActive(value: false);
			animController.SetSymbolVisiblity(symbolTarget, is_visible: false);
			return kBatchedAnimController;
		}

		private void OnCopySettings(object sourceOBJ)
		{
			if (sourceOBJ != null)
			{
				Instance sMI = ((GameObject)sourceOBJ).GetSMI<Instance>();
				if (sMI != null)
				{
					SetTargetItem(sMI.TargetTag);
					UserMaxCapacity = sMI.UserMaxCapacity;
				}
			}
		}

		public void RefreshAmountMeter()
		{
			float positionPercent = ((UserMaxCapacity == 0f) ? 0f : Mathf.Clamp(AmountOfDesiredContentStored / UserMaxCapacity, 0f, 1f));
			amountMeter.SetPositionPercent(positionPercent);
		}

		public void PlayDoorAnimation()
		{
			doorSymbol.Play("door");
		}

		public void SetTargetItem(Tag tag)
		{
			base.sm.TargetItemTag.Set(tag, this);
			base.gameObject.Trigger(-2076953849);
		}

		public void ApplySettings()
		{
			Tag treeFilterableCurrentTag = GetTreeFilterableCurrentTag();
			treeFilterable.RemoveTagFromFilter(treeFilterableCurrentTag);
		}

		public void DropUndesiredItems()
		{
			Vector3 position = Grid.CellToPos(GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			if (TargetTag != INVALID_TAG)
			{
				treeFilterable.AddTagToFilter(TargetTag);
				GameObject[] array = storage.DropUnlessHasTag(TargetTag);
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
				storage.DropAll(position);
			}
			storage.DropUnlessHasTag(TargetTag);
		}

		public void UpdateContentSymbol()
		{
			RefreshAmountMeter();
			bool flag = TargetTag == INVALID_TAG;
			if (!flag || HasContents)
			{
				bool flag2 = !flag && (IsPendingChange || !HasAnyDesiredContents);
				string animName = "";
				GameObject gameObject = ((TargetTag == INVALID_TAG) ? Assets.GetPrefab(storage.items[0].PrefabID()) : Assets.GetPrefab(TargetTag));
				KAnimFile animFileFromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, flag2 ? "ui" : "", out animName);
				animController.SetSymbolVisiblity(ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, flag2);
				itemPreviewSymbol.gameObject.SetActive(flag2);
				itemSymbol.gameObject.SetActive(!flag2);
				if (flag2)
				{
					itemPreviewSymbol.SwapAnims(new KAnimFile[1] { animFileFromPrefabWithTag });
					itemPreviewSymbol.Play(animName);
					return;
				}
				if (gameObject.HasTag(GameTags.Egg))
				{
					string text = animName;
					if (!string.IsNullOrEmpty(text))
					{
						itemSymbolOverrideController.ApplySymbolOverridesByAffix(animFileFromPrefabWithTag, text);
					}
					animName = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
				}
				else
				{
					itemSymbolOverrideController.RemoveAllSymbolOverrides();
					animName = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
				}
				itemSymbol.SwapAnims(new KAnimFile[1] { animFileFromPrefabWithTag });
				itemSymbol.Play(animName);
				SpecificItemTagSizeInstruction sizeInstructionForObject = base.def.GetSizeInstructionForObject(gameObject);
				itemSymbol.transform.localScale = Vector3.one * (sizeInstructionForObject?.sizeMultiplier ?? defaultItemSymbolScale);
				KCollider2D component = gameObject.GetComponent<KCollider2D>();
				Vector3 localPosition = defaultItemLocalPosition;
				localPosition.y += ((component == null || component is KCircleCollider2D) ? 0f : ((0f - component.offset.y) * 0.5f));
				itemSymbol.transform.localPosition = localPosition;
			}
			else
			{
				itemSymbol.gameObject.SetActive(value: false);
				itemPreviewSymbol.gameObject.SetActive(value: false);
				animController.SetSymbolVisiblity(ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, is_visible: false);
			}
		}

		private void AbortChore()
		{
			if (chore != null)
			{
				chore.Cancel("Change settings Chore aborted");
				chore = null;
			}
		}

		public void StartChangeSettingChore()
		{
			AbortChore();
			chore = new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, GetWorkable());
		}

		public void CanceChangeSettingChore()
		{
			AbortChore();
		}
	}

	public const string METER_TARGET = "meter_target";

	public const string METER_ANIMATION = "meter";

	public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");

	public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");

	public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");

	public const string ITEM_SYMBOL_ANIMATION = "meter_object";

	public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");

	public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");

	public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";

	public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");

	public const string DEFAULT_ANIMATION_NAME = "on";

	public const string STORAGE_CHANGE_ANIMATION_NAME = "door";

	public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";

	public static Tag INVALID_TAG = GameTags.Void;

	private TagParameter TargetItemTag = new TagParameter(INVALID_TAG);

	public State idle;

	public SettingsChangeStates change;

	public State awaitingDelivery;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = idle;
		root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, OnStorageChanged).EventHandler(GameHashes.StorageTileTargetItemChanged, RefreshContentVisuals);
		idle.Enter(RefreshContentVisuals).EventTransition(GameHashes.OnStorageChange, awaitingDelivery, IsAwaitingDelivery).EventTransition(GameHashes.StorageTileTargetItemChanged, change, IsAwaitingForSettingChange);
		change.Enter(RefreshContentVisuals).EventTransition(GameHashes.StorageTileTargetItemChanged, idle, NoLongerAwaitingForSettingChange).DefaultState(change.awaitingSettingsChange);
		change.awaitingSettingsChange.Enter(StartWorkChore).Exit(CancelWorkChore).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget)
			.WorkableCompleteTransition((Instance smi) => smi.GetWorkable(), change.complete);
		change.complete.Enter(ApplySettings).Enter(DropUndesiredItems).EnterTransition(idle, HasAnyDesiredItemStored)
			.EnterTransition(awaitingDelivery, IsAwaitingDelivery);
		awaitingDelivery.Enter(RefreshContentVisuals).EventTransition(GameHashes.OnStorageChange, idle, HasAnyDesiredItemStored).EventTransition(GameHashes.StorageTileTargetItemChanged, change, IsAwaitingForSettingChange);
	}

	public static void DropUndesiredItems(Instance smi)
	{
		smi.DropUndesiredItems();
	}

	public static void ApplySettings(Instance smi)
	{
		smi.ApplySettings();
	}

	public static void StartWorkChore(Instance smi)
	{
		smi.StartChangeSettingChore();
	}

	public static void CancelWorkChore(Instance smi)
	{
		smi.CanceChangeSettingChore();
	}

	public static void RefreshContentVisuals(Instance smi)
	{
		smi.UpdateContentSymbol();
	}

	public static bool IsAwaitingForSettingChange(Instance smi)
	{
		return smi.IsPendingChange;
	}

	public static bool NoLongerAwaitingForSettingChange(Instance smi)
	{
		return !smi.IsPendingChange;
	}

	public static bool HasAnyDesiredItemStored(Instance smi)
	{
		return smi.HasAnyDesiredContents;
	}

	public static void OnStorageChanged(Instance smi)
	{
		smi.PlayDoorAnimation();
		RefreshContentVisuals(smi);
	}

	public static bool IsAwaitingDelivery(Instance smi)
	{
		if (!smi.IsPendingChange)
		{
			return !smi.HasAnyDesiredContents;
		}
		return false;
	}
}
