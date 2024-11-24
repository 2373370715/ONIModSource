using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C5E RID: 3166
[SerializationConfig(MemberSerialization.OptIn)]
public class BottleEmptier : StateMachineComponent<BottleEmptier.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003CA0 RID: 15520 RVA: 0x000C73C3 File Offset: 0x000C55C3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.DefineManualPumpingAffectedBuildings();
		base.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
		base.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
	}

	// Token: 0x06003CA1 RID: 15521 RVA: 0x000AD332 File Offset: 0x000AB532
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x0022E394 File Offset: 0x0022C594
	private void DefineManualPumpingAffectedBuildings()
	{
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			return;
		}
		List<string> list = new List<string>();
		Tag tag = this.isGasEmptier ? GameTags.GasSource : GameTags.LiquidSource;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.BuildingComplete.HasTag(tag))
			{
				list.Add(buildingDef.Name);
			}
		}
		BottleEmptier.manualPumpingAffectedBuildings.Add(this.isGasEmptier, list.ToArray());
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x000C73FE File Offset: 0x000C55FE
	private void OnChangeAllowManualPumpingStationFetching()
	{
		this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x0022E440 File Offset: 0x0022C640
	private void OnRefreshUserMenu(object data)
	{
		string text = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP;
		string text2 = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP;
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			foreach (string arg in BottleEmptier.manualPumpingAffectedBuildings[this.isGasEmptier])
			{
				string str = string.Format(UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.ITEM, arg);
				text += str;
				text2 += str;
			}
		}
		if (this.isGasEmptier)
		{
			KIconButtonMenu.ButtonInfo button = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 0.4f);
			return;
		}
		KIconButtonMenu.ButtonInfo button2 = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button2, 0.4f);
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x0022E5E0 File Offset: 0x0022C7E0
	private void OnCopySettings(object data)
	{
		BottleEmptier component = ((GameObject)data).GetComponent<BottleEmptier>();
		this.allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x04002953 RID: 10579
	public float emptyRate = 10f;

	// Token: 0x04002954 RID: 10580
	[Serialize]
	public bool allowManualPumpingStationFetching;

	// Token: 0x04002955 RID: 10581
	[Serialize]
	public bool emit = true;

	// Token: 0x04002956 RID: 10582
	public bool isGasEmptier;

	// Token: 0x04002957 RID: 10583
	private static Dictionary<bool, string[]> manualPumpingAffectedBuildings = new Dictionary<bool, string[]>();

	// Token: 0x04002958 RID: 10584
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002959 RID: 10585
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000C5F RID: 3167
	public class StatesInstance : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x000C7474 File Offset: 0x000C5674
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000C747C File Offset: 0x000C567C
		public MeterController meter { get; private set; }

		// Token: 0x06003CAA RID: 15530 RVA: 0x0022E610 File Offset: 0x0022C810
		public StatesInstance(BottleEmptier smi) : base(smi)
		{
			TreeFilterable component = base.master.GetComponent<TreeFilterable>();
			component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			this.meter.meterController.GetComponent<KBatchedAnimTracker>().synchronizeEnabledState = false;
			this.meter.meterController.enabled = false;
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
			base.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x0022E6E4 File Offset: 0x0022C8E4
		public void CreateChore()
		{
			HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
			Tag[] forbidden_tags;
			if (!base.master.allowManualPumpingStationFetching)
			{
				forbidden_tags = new Tag[]
				{
					GameTags.LiquidSource,
					GameTags.GasSource
				};
			}
			else
			{
				forbidden_tags = new Tag[0];
			}
			Storage component = base.GetComponent<Storage>();
			this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, null, true, null, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x000C7485 File Offset: 0x000C5685
		public void CancelChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Storage Changed");
				this.chore = null;
			}
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x000C74A6 File Offset: 0x000C56A6
		public void RefreshChore()
		{
			this.GoTo(base.sm.unoperational);
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x000C74B9 File Offset: 0x000C56B9
		private void OnFilterChanged(HashSet<Tag> tags)
		{
			this.RefreshChore();
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x0022E76C File Offset: 0x0022C96C
		private void OnStorageChange(object data)
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.RemainingCapacity() / this.storage.capacityKg));
			this.meter.meterController.enabled = (this.storage.MassStored() > 0f);
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x000C74B9 File Offset: 0x000C56B9
		private void OnOnlyFetchMarkedItemsSettingChanged(object data)
		{
			this.RefreshChore();
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0022E7C4 File Offset: 0x0022C9C4
		public void StartMeter()
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			base.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), firstPrimaryElement.Element.substance.colour);
			this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
			this.meter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
			Color32 colour = firstPrimaryElement.Element.substance.colour;
			colour.a = byte.MaxValue;
			this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x0022E8B8 File Offset: 0x0022CAB8
		private PrimaryElement GetFirstPrimaryElement()
		{
			for (int i = 0; i < this.storage.Count; i++)
			{
				GameObject gameObject = this.storage[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null))
					{
						return component;
					}
				}
			}
			return null;
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x0022E904 File Offset: 0x0022CB04
		public void Emit(float dt)
		{
			if (!base.smi.master.emit)
			{
				return;
			}
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			float num = Mathf.Min(firstPrimaryElement.Mass, base.master.emptyRate * dt);
			if (num <= 0f)
			{
				return;
			}
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(prefabTag, num, out num2, out diseaseInfo, out temperature);
			Vector3 position = base.transform.GetPosition();
			position.y += 1.8f;
			bool flag = base.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += (flag ? -0.2f : 0.2f);
			int num3 = Grid.PosToCell(position) + (flag ? -1 : 1);
			if (Grid.Solid[num3])
			{
				num3 += (flag ? 1 : -1);
			}
			Element element = firstPrimaryElement.Element;
			ushort idx = element.idx;
			if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, idx, num2, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
				return;
			}
			SimMessages.ModifyCell(num3, idx, temperature, num2, diseaseInfo.idx, diseaseInfo.count, SimMessages.ReplaceType.None, false, -1);
		}

		// Token: 0x0400295A RID: 10586
		[MyCmpGet]
		public Storage storage;

		// Token: 0x0400295B RID: 10587
		private FetchChore chore;
	}

	// Token: 0x02000C60 RID: 3168
	public class States : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
	{
		// Token: 0x06003CB4 RID: 15540 RVA: 0x0022EA40 File Offset: 0x0022CC40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waitingfordelivery;
			this.statusItem = new StatusItem("BottleEmptier", "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveStringCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME;
				}
				return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME;
			};
			this.statusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				string result;
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					if (bottleEmptier.isGasEmptier)
					{
						result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP;
					}
					else
					{
						result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP;
					}
				}
				else if (bottleEmptier.isGasEmptier)
				{
					result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP;
				}
				else
				{
					result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP;
				}
				return result;
			};
			this.root.ToggleStatusItem(this.statusItem, (BottleEmptier.StatesInstance smi) => smi.master);
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() > 0f).Enter("CreateChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CreateChore();
			}).Exit("CancelChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CancelChore();
			}).PlayAnim("on");
			this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() == 0f).Enter("StartMeter", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.StartMeter();
			}).Update("Emit", delegate(BottleEmptier.StatesInstance smi, float dt)
			{
				smi.Emit(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x0400295D RID: 10589
		private StatusItem statusItem;

		// Token: 0x0400295E RID: 10590
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;

		// Token: 0x0400295F RID: 10591
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;

		// Token: 0x04002960 RID: 10592
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;
	}
}
