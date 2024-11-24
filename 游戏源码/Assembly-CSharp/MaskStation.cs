using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E8C RID: 3724
public class MaskStation : StateMachineComponent<MaskStation.SMInstance>, IBasicBuilding
{
	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x000D06C3 File Offset: 0x000CE8C3
	// (set) Token: 0x06004AE5 RID: 19173 RVA: 0x000D06D0 File Offset: 0x000CE8D0
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x000D06DA File Offset: 0x000CE8DA
	// (set) Token: 0x06004AE7 RID: 19175 RVA: 0x000D06E7 File Offset: 0x000CE8E7
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x0025C934 File Offset: 0x0025AB34
	public void UpdateOperational()
	{
		bool flag = this.GetTotalOxygenAmount() >= this.oxygenConsumedPerMask * (float)this.maxUses;
		this.shouldPump = this.IsPumpable();
		if (this.operational.IsOperational && this.shouldPump && !flag)
		{
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidMaskStationConsumptionState, this.noElementStatusGuid, !this.shouldPump, null);
	}

	// Token: 0x06004AE9 RID: 19177 RVA: 0x0025C9CC File Offset: 0x0025ABCC
	private bool IsPumpable()
	{
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool result = false;
		foreach (ElementConsumer elementConsumer in components)
		{
			for (int j = 0; j < (int)elementConsumer.consumptionRadius; j++)
			{
				for (int k = 0; k < (int)elementConsumer.consumptionRadius; k++)
				{
					int num2 = num + k + Grid.WidthInCells * j;
					bool flag = Grid.Element[num2].IsState(Element.State.Gas);
					bool flag2 = Grid.Element[num2].id == elementConsumer.elementToConsume;
					if (flag && flag2)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004AEA RID: 19178 RVA: 0x0025CA70 File Offset: 0x0025AC70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, null, false, fetch_chore_type);
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x0025CAAC File Offset: 0x0025ACAC
	private List<GameObject> GetPossibleMaterials()
	{
		List<GameObject> result = new List<GameObject>();
		this.materialStorage.Find(this.materialTag, result);
		return result;
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x000D06F1 File Offset: 0x000CE8F1
	private float GetTotalMaterialAmount()
	{
		return this.materialStorage.GetMassAvailable(this.materialTag);
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x000D0704 File Offset: 0x000CE904
	private float GetTotalOxygenAmount()
	{
		return this.oxygenStorage.GetMassAvailable(this.oxygenTag);
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x0025CAD4 File Offset: 0x0025ACD4
	private void RefreshMeters()
	{
		float num = this.GetTotalMaterialAmount();
		num = Mathf.Clamp01(num / ((float)this.maxUses * this.materialConsumedPerMask));
		float num2 = this.GetTotalOxygenAmount();
		num2 = Mathf.Clamp01(num2 / ((float)this.maxUses * this.oxygenConsumedPerMask));
		this.materialsMeter.SetPositionPercent(num);
		this.oxygenMeter.SetPositionPercent(num2);
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x0025CB34 File Offset: 0x0025AD34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.CreateNewReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<MaskStation>(-592767678, MaskStation.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<MaskStation>(-1643076535, MaskStation.OnRotatedDelegate);
		this.materialsMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_resources_target", "meter_resources", this.materialsMeterOffset, Grid.SceneLayer.BuildingBack, new string[]
		{
			"meter_resources_target"
		});
		this.oxygenMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_oxygen_target", "meter_oxygen", this.oxygenMeterOffset, Grid.SceneLayer.BuildingFront, new string[]
		{
			"meter_oxygen_target"
		});
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		base.Subscribe<MaskStation>(-1697596308, MaskStation.OnStorageChangeDelegate);
		this.RefreshMeters();
	}

	// Token: 0x06004AF0 RID: 19184 RVA: 0x0025CC40 File Offset: 0x0025AE40
	private void Update()
	{
		float a = this.GetTotalMaterialAmount() / this.materialConsumedPerMask;
		float b = this.GetTotalOxygenAmount() / this.oxygenConsumedPerMask;
		int fullLockerCount = (int)Mathf.Min(a, b);
		int emptyLockerCount = 0;
		Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
	}

	// Token: 0x06004AF1 RID: 19185 RVA: 0x0025CC8C File Offset: 0x0025AE8C
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x000D0717 File Offset: 0x000CE917
	private void OnOperationalChanged(bool isOperational)
	{
		this.isOperational = isOperational;
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x000D0720 File Offset: 0x000CE920
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x000D0728 File Offset: 0x000CE928
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x000D074C File Offset: 0x000CE94C
	private void CreateNewReactable()
	{
		this.reactable = new MaskStation.OxygenMaskReactable(this);
	}

	// Token: 0x040033DB RID: 13275
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040033DC RID: 13276
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x040033DD RID: 13277
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x040033DE RID: 13278
	public float materialConsumedPerMask = 1f;

	// Token: 0x040033DF RID: 13279
	public float oxygenConsumedPerMask = 1f;

	// Token: 0x040033E0 RID: 13280
	public Tag materialTag = GameTags.Metal;

	// Token: 0x040033E1 RID: 13281
	public Tag oxygenTag = GameTags.Breathable;

	// Token: 0x040033E2 RID: 13282
	public int maxUses = 10;

	// Token: 0x040033E3 RID: 13283
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040033E4 RID: 13284
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040033E5 RID: 13285
	public Storage materialStorage;

	// Token: 0x040033E6 RID: 13286
	public Storage oxygenStorage;

	// Token: 0x040033E7 RID: 13287
	private bool shouldPump;

	// Token: 0x040033E8 RID: 13288
	private MaskStation.OxygenMaskReactable reactable;

	// Token: 0x040033E9 RID: 13289
	private MeterController materialsMeter;

	// Token: 0x040033EA RID: 13290
	private MeterController oxygenMeter;

	// Token: 0x040033EB RID: 13291
	public Meter.Offset materialsMeterOffset = Meter.Offset.Behind;

	// Token: 0x040033EC RID: 13292
	public Meter.Offset oxygenMeterOffset;

	// Token: 0x040033ED RID: 13293
	public string choreTypeID;

	// Token: 0x040033EE RID: 13294
	protected FilteredStorage filteredStorage;

	// Token: 0x040033EF RID: 13295
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x040033F0 RID: 13296
	private int cell;

	// Token: 0x040033F1 RID: 13297
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x040033F2 RID: 13298
	private Guid noElementStatusGuid;

	// Token: 0x040033F3 RID: 13299
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x02000E8D RID: 3725
	private class OxygenMaskReactable : Reactable
	{
		// Token: 0x06004AF8 RID: 19192 RVA: 0x0025CD98 File Offset: 0x0025AF98
		public OxygenMaskReactable(MaskStation mask_station) : base(mask_station.gameObject, "OxygenMask", Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.maskStation = mask_station;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0025CDEC File Offset: 0x0025AFEC
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.maskStation == null)
			{
				base.Cleanup();
				return false;
			}
			bool flag = !new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			int x = transition.navGridTransition.x;
			if (x == 0)
			{
				return false;
			}
			if (!flag)
			{
				return (x >= 0 || !this.maskStation.isRotated) && (x <= 0 || this.maskStation.isRotated);
			}
			return this.maskStation.smi.IsReady() && (x <= 0 || !this.maskStation.isRotated) && (x >= 0 || this.maskStation.isRotated);
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0025CEBC File Offset: 0x0025B0BC
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.maskStation.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.maskStation.CreateNewReactable();
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0025CF50 File Offset: 0x0025B150
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.maskStation)
			{
				facing.SetFacing(this.maskStation.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				this.Run();
				base.Cleanup();
			}
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0025CFC8 File Offset: 0x0025B1C8
		private void Run()
		{
			GameObject reactor = this.reactor;
			Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
			bool flag = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			Navigator component = reactor.GetComponent<Navigator>();
			bool flag2 = component != null && (component.flags & this.maskStation.PathFlag) > PathFinder.PotentialPath.Flags.None;
			if (flag)
			{
				if (!this.maskStation.smi.IsReady())
				{
					return;
				}
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Oxygen_Mask".ToTag()), null, null);
				gameObject.SetActive(true);
				SimHashes elementID = this.maskStation.GetPossibleMaterials()[0].GetComponent<PrimaryElement>().ElementID;
				gameObject.GetComponent<PrimaryElement>().SetElement(elementID, false);
				SuitTank component2 = gameObject.GetComponent<SuitTank>();
				this.maskStation.materialStorage.ConsumeIgnoringDisease(this.maskStation.materialTag, this.maskStation.materialConsumedPerMask);
				this.maskStation.oxygenStorage.Transfer(component2.storage, component2.elementTag, this.maskStation.oxygenConsumedPerMask, false, true);
				Equippable component3 = gameObject.GetComponent<Equippable>();
				component3.Assign(equipment.GetComponent<IAssignableIdentity>());
				component3.isEquipped = true;
			}
			if (!flag)
			{
				Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
				assignable.Unassign();
				if (!flag2)
				{
					Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
					assignable.GetComponent<Notifier>().Add(notification, "");
				}
			}
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x000D075A File Offset: 0x000CE95A
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.maskStation.interactAnim);
			}
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040033F4 RID: 13300
		private MaskStation maskStation;

		// Token: 0x040033F5 RID: 13301
		private float startTime;
	}

	// Token: 0x02000E8F RID: 3727
	public class SMInstance : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.GameInstance
	{
		// Token: 0x06004B02 RID: 19202 RVA: 0x000D079D File Offset: 0x000CE99D
		public SMInstance(MaskStation master) : base(master)
		{
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x000D07A6 File Offset: 0x000CE9A6
		private bool HasSufficientMaterials()
		{
			return base.master.GetTotalMaterialAmount() >= base.master.materialConsumedPerMask;
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x000D07C3 File Offset: 0x000CE9C3
		private bool HasSufficientOxygen()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask;
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x000D07E0 File Offset: 0x000CE9E0
		public bool OxygenIsFull()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask * (float)base.master.maxUses;
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x000D080A File Offset: 0x000CEA0A
		public bool IsReady()
		{
			return this.HasSufficientMaterials() && this.HasSufficientOxygen();
		}
	}

	// Token: 0x02000E90 RID: 3728
	public class States : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation>
	{
		// Token: 0x06004B07 RID: 19207 RVA: 0x0025D170 File Offset: 0x0025B370
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notOperational;
			this.notOperational.PlayAnim("off").TagTransition(GameTags.Operational, this.charging, false);
			this.charging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.notCharging, (MaskStation.SMInstance smi) => smi.OxygenIsFull() || !smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (smi.OxygenIsFull() || !smi.master.shouldPump)
				{
					smi.GoTo(this.notCharging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.charging.openChargingPre);
					return;
				}
				smi.GoTo(this.charging.closedChargingPre);
			});
			this.charging.opening.QueueAnim("opening_charging", false, null).OnAnimQueueComplete(this.charging.open);
			this.charging.open.PlayAnim("open_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.charging.closing.QueueAnim("closing_charging", false, null).OnAnimQueueComplete(this.charging.closed);
			this.charging.closed.PlayAnim("closed_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.charging.openChargingPre.PlayAnim("open_charging_pre").OnAnimQueueComplete(this.charging.open);
			this.charging.closedChargingPre.PlayAnim("closed_charging_pre").OnAnimQueueComplete(this.charging.closed);
			this.notCharging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.charging, (MaskStation.SMInstance smi) => !smi.OxygenIsFull() && smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (!smi.OxygenIsFull() && smi.master.shouldPump)
				{
					smi.GoTo(this.charging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.notCharging.openChargingPst);
					return;
				}
				smi.GoTo(this.notCharging.closedChargingPst);
			});
			this.notCharging.opening.PlayAnim("opening_not_charging").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.open.PlayAnim("open_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.notCharging.closing.PlayAnim("closing_not_charging").OnAnimQueueComplete(this.notCharging.closed);
			this.notCharging.closed.PlayAnim("closed_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.notCharging.openChargingPst.PlayAnim("open_charging_pst").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.closedChargingPst.PlayAnim("closed_charging_pst").OnAnimQueueComplete(this.notCharging.closed);
		}

		// Token: 0x040033F8 RID: 13304
		public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State notOperational;

		// Token: 0x040033F9 RID: 13305
		public MaskStation.States.ChargingStates charging;

		// Token: 0x040033FA RID: 13306
		public MaskStation.States.NotChargingStates notCharging;

		// Token: 0x02000E91 RID: 3729
		public class ChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x040033FB RID: 13307
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x040033FC RID: 13308
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x040033FD RID: 13309
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x040033FE RID: 13310
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x040033FF RID: 13311
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPre;

			// Token: 0x04003400 RID: 13312
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPre;
		}

		// Token: 0x02000E92 RID: 3730
		public class NotChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x04003401 RID: 13313
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x04003402 RID: 13314
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x04003403 RID: 13315
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x04003404 RID: 13316
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x04003405 RID: 13317
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPst;

			// Token: 0x04003406 RID: 13318
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPst;
		}
	}
}
