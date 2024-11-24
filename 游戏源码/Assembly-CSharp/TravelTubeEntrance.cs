using System;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200100C RID: 4108
[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x060053BD RID: 21437 RVA: 0x000D68C1 File Offset: 0x000D4AC1
	public float AvailableJoules
	{
		get
		{
			return this.availableJoules;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x060053BE RID: 21438 RVA: 0x000D68C9 File Offset: 0x000D4AC9
	public float TotalCapacity
	{
		get
		{
			return this.jouleCapacity;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x060053BF RID: 21439 RVA: 0x000D68D1 File Offset: 0x000D4AD1
	public float UsageJoules
	{
		get
		{
			return this.joulesPerLaunch;
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x060053C0 RID: 21440 RVA: 0x000D68D9 File Offset: 0x000D4AD9
	public bool HasLaunchPower
	{
		get
		{
			return this.availableJoules > this.joulesPerLaunch;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x060053C1 RID: 21441 RVA: 0x000D68E9 File Offset: 0x000D4AE9
	public bool HasWaxForGreasyLaunch
	{
		get
		{
			return this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= this.waxPerLaunch;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x060053C2 RID: 21442 RVA: 0x000D690B File Offset: 0x000D4B0B
	public int WaxLaunchesAvailable
	{
		get
		{
			return Mathf.FloorToInt(this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / this.waxPerLaunch);
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x060053C3 RID: 21443 RVA: 0x000D692E File Offset: 0x000D4B2E
	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			return this.deliverAndUseWax && this.HasWaxForGreasyLaunch;
		}
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x00278A08 File Offset: 0x00276C08
	public static void SetTravelerGleamEffect(TravelTubeEntrance.SMInstance smi)
	{
		TravelTubeEntrance.Work component = smi.GetComponent<TravelTubeEntrance.Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x000D6940 File Offset: 0x000D4B40
	public static string GetLaunchAnimName(TravelTubeEntrance.SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	// Token: 0x060053C6 RID: 21446 RVA: 0x000D695A File Offset: 0x000D4B5A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.energyConsumer.OnConnectionChanged += this.OnConnectionChanged;
	}

	// Token: 0x060053C7 RID: 21447 RVA: 0x00278A50 File Offset: 0x00276C50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetWaxUse(this.deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), true);
		this.TubeConnectionsChanged(connections);
		this.tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], new Action<object>(this.TubeChanged));
		base.Subscribe<TravelTubeEntrance>(-592767678, TravelTubeEntrance.OnOperationalChangedDelegate);
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.waxMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.StartSM();
		this.UpdateWaxCharge();
		this.UpdateCharge();
		base.Subscribe<TravelTubeEntrance>(493375141, TravelTubeEntrance.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060053C8 RID: 21448 RVA: 0x000D6979 File Offset: 0x000D4B79
	private void OnStorageChanged(object obj)
	{
		this.UpdateWaxCharge();
	}

	// Token: 0x060053C9 RID: 21449 RVA: 0x00278BA4 File Offset: 0x00276DA4
	protected override void OnCleanUp()
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		this.ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref this.tubeChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060053CA RID: 21450 RVA: 0x00278C0C File Offset: 0x00276E0C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.deliverAndUseWax)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(true);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		else
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = this.deliverAndUseWax && this.WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, false);
		}
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x00278D14 File Offset: 0x00276F14
	public void SetWaxUse(bool usingWax)
	{
		this.deliverAndUseWax = usingWax;
		this.manualDelivery.AbortDelivery("Switching to new delivery request");
		this.manualDelivery.capacity = (usingWax ? this.storage.capacityKg : 0f);
		this.manualDelivery.refillMass = (usingWax ? this.waxPerLaunch : 0f);
		this.manualDelivery.MinimumMass = (usingWax ? this.waxPerLaunch : 0f);
		if (!usingWax)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060053CC RID: 21452 RVA: 0x00278DB0 File Offset: 0x00276FB0
	private void TubeChanged(object data)
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data == null)
		{
			this.TubeConnectionsChanged(0);
			return;
		}
		TravelTube component = gameObject.GetComponent<TravelTube>();
		if (component != null)
		{
			component.Subscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = component;
			return;
		}
		this.TubeConnectionsChanged(0);
	}

	// Token: 0x060053CD RID: 21453 RVA: 0x00278E44 File Offset: 0x00277044
	private void TubeConnectionsChanged(object data)
	{
		bool value = (UtilityConnections)data == UtilityConnections.Up;
		this.operational.SetFlag(TravelTubeEntrance.tubeConnected, value);
	}

	// Token: 0x060053CE RID: 21454 RVA: 0x00278E6C File Offset: 0x0027706C
	private bool CanAcceptMorePower()
	{
		return this.operational.IsOperational && (this.button == null || this.button.IsEnabled) && this.energyConsumer.IsExternallyPowered && this.availableJoules < this.jouleCapacity;
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x00278EC0 File Offset: 0x002770C0
	public void Sim200ms(float dt)
	{
		if (this.CanAcceptMorePower())
		{
			this.availableJoules = Mathf.Min(this.jouleCapacity, this.availableJoules + this.energyConsumer.WattsUsed * dt);
			this.UpdateCharge();
		}
		this.energyConsumer.SetSustained(this.HasLaunchPower);
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x000D6981 File Offset: 0x000D4B81
	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, true);
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x000D6991 File Offset: 0x000D4B91
	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, false);
	}

	// Token: 0x060053D2 RID: 21458 RVA: 0x000D69A1 File Offset: 0x000D4BA1
	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060053D3 RID: 21459 RVA: 0x000D69BE File Offset: 0x000D4BBE
	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060053D4 RID: 21460 RVA: 0x000D69DB File Offset: 0x000D4BDB
	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060053D5 RID: 21461 RVA: 0x000D69E9 File Offset: 0x000D4BE9
	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x00278F20 File Offset: 0x00277120
	public bool ShouldWait(GameObject reactor)
	{
		if (!this.operational.IsOperational)
		{
			return false;
		}
		if (!this.HasLaunchPower)
		{
			return false;
		}
		if (this.launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
		return this.HasChargeSlotReserved(smi, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x00278F74 File Offset: 0x00277174
	public void ConsumeCharge(GameObject reactor)
	{
		if (this.HasLaunchPower)
		{
			this.availableJoules -= this.joulesPerLaunch;
			if (this.deliverAndUseWax && this.HasWaxForGreasyLaunch)
			{
				TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
				if (smi != null)
				{
					Tag tag = SimHashes.MilkFat.CreateTag();
					float num;
					SimUtil.DiseaseInfo diseaseInfo;
					float num2;
					this.storage.ConsumeAndGetDisease(tag, this.waxPerLaunch, out num, out diseaseInfo, out num2);
					GermExposureMonitor.Instance smi2 = reactor.GetSMI<GermExposureMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Contact);
					}
					smi.SetWaxState(true);
				}
			}
			this.UpdateCharge();
			this.UpdateWaxCharge();
		}
	}

	// Token: 0x060053D8 RID: 21464 RVA: 0x000D69F7 File Offset: 0x000D4BF7
	private void CreateNewWaitReactable()
	{
		if (this.wait_reactable == null)
		{
			this.wait_reactable = new TravelTubeEntrance.WaitReactable(this);
		}
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x000D6A0D File Offset: 0x000D4C0D
	private void OrphanWaitReactable()
	{
		this.wait_reactable = null;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x000D6A16 File Offset: 0x000D4C16
	private void ClearWaitReactable()
	{
		if (this.wait_reactable != null)
		{
			this.wait_reactable.Cleanup();
			this.wait_reactable = null;
		}
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x00279010 File Offset: 0x00277210
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), flag);
		this.UpdateActive();
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x000D6A32 File Offset: 0x000D4C32
	private void OnConnectionChanged()
	{
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x000D6A40 File Offset: 0x000D4C40
	private void UpdateActive()
	{
		this.operational.SetActive(this.CanAcceptMorePower(), false);
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x00279038 File Offset: 0x00277238
	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(this.HasLaunchPower, base.smi, false);
		float positionPercent = Mathf.Clamp01(this.availableJoules / this.jouleCapacity);
		this.meter.SetPositionPercent(positionPercent);
		this.energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x002790B8 File Offset: 0x002772B8
	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.waxMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x002790F0 File Offset: 0x002772F0
	private void UpdateConnectionStatus()
	{
		bool flag = this.button != null && !this.button.IsEnabled;
		bool isConnected = this.energyConsumer.IsConnected;
		bool hasLaunchPower = this.HasLaunchPower;
		if (flag || !isConnected || hasLaunchPower)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower, null);
		}
	}

	// Token: 0x04003A79 RID: 14969
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003A7A RID: 14970
	[MyCmpReq]
	private TravelTubeEntrance.Work launch_workable;

	// Token: 0x04003A7B RID: 14971
	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	// Token: 0x04003A7C RID: 14972
	[MyCmpGet]
	private BuildingEnabledButton button;

	// Token: 0x04003A7D RID: 14973
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003A7E RID: 14974
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003A7F RID: 14975
	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	// Token: 0x04003A80 RID: 14976
	public float jouleCapacity = 1f;

	// Token: 0x04003A81 RID: 14977
	public float joulesPerLaunch = 1f;

	// Token: 0x04003A82 RID: 14978
	public float waxPerLaunch;

	// Token: 0x04003A83 RID: 14979
	[Serialize]
	private float availableJoules;

	// Token: 0x04003A84 RID: 14980
	[Serialize]
	private bool deliverAndUseWax;

	// Token: 0x04003A85 RID: 14981
	private TravelTube travelTube;

	// Token: 0x04003A86 RID: 14982
	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	// Token: 0x04003A87 RID: 14983
	private TravelTubeEntrance.WaitReactable wait_reactable;

	// Token: 0x04003A88 RID: 14984
	private MeterController meter;

	// Token: 0x04003A89 RID: 14985
	private MeterController waxMeter;

	// Token: 0x04003A8A RID: 14986
	private const int MAX_CHARGES = 3;

	// Token: 0x04003A8B RID: 14987
	private const float RECHARGE_TIME = 10f;

	// Token: 0x04003A8C RID: 14988
	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	// Token: 0x04003A8D RID: 14989
	private HandleVector<int>.Handle tubeChangedEntry;

	// Token: 0x04003A8E RID: 14990
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003A8F RID: 14991
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003A90 RID: 14992
	private Guid connectedStatus;

	// Token: 0x0200100D RID: 4109
	private class LaunchReactable : WorkableReactable
	{
		// Token: 0x060053E5 RID: 21477 RVA: 0x000D6A84 File Offset: 0x000D4C84
		public LaunchReactable(Workable workable, TravelTubeEntrance entrance) : base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance, WorkableReactable.AllowedDirection.Any)
		{
			this.entrance = entrance;
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x002791DC File Offset: 0x002773DC
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				return component && this.entrance.HasChargeSlotReserved(component);
			}
			return false;
		}

		// Token: 0x04003A91 RID: 14993
		private TravelTubeEntrance entrance;
	}

	// Token: 0x0200100E RID: 4110
	private class WaitReactable : Reactable
	{
		// Token: 0x060053E7 RID: 21479 RVA: 0x00279214 File Offset: 0x00277414
		public WaitReactable(TravelTubeEntrance entrance) : base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.entrance = entrance;
			this.preventChoreInterruption = false;
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x000D6AAE File Offset: 0x000D4CAE
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.entrance == null)
			{
				base.Cleanup();
				return false;
			}
			return this.entrance.ShouldWait(new_reactor);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x00279270 File Offset: 0x00277470
		protected override void InternalBegin()
		{
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.entrance.OrphanWaitReactable();
			this.entrance.CreateNewWaitReactable();
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x000D6AE2 File Offset: 0x000D4CE2
		public override void Update(float dt)
		{
			if (this.entrance == null)
			{
				base.Cleanup();
				return;
			}
			if (!this.entrance.ShouldWait(this.reactor))
			{
				base.Cleanup();
			}
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x000C9196 File Offset: 0x000C7396
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04003A92 RID: 14994
		private TravelTubeEntrance entrance;
	}

	// Token: 0x0200100F RID: 4111
	public class SMInstance : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		// Token: 0x060053ED RID: 21485 RVA: 0x000D6B12 File Offset: 0x000D4D12
		public SMInstance(TravelTubeEntrance master) : base(master)
		{
		}
	}

	// Token: 0x02001010 RID: 4112
	public class States : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance>
	{
		// Token: 0x060053EE RID: 21486 RVA: 0x002792F0 File Offset: 0x002774F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notoperational;
			this.root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge, null);
			this.notoperational.DefaultState(this.notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.notube, (TravelTubeEntrance.SMInstance smi) => !smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected));
			this.notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.normal, (TravelTubeEntrance.SMInstance smi) => smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected, null);
			this.notready.PlayAnim("off").ParamTransition<bool>(this.hasLaunchCharges, this.ready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((TravelTubeEntrance.SMInstance smi) => new TravelTubeEntrance.LaunchReactable(smi.master.GetComponent<TravelTubeEntrance.Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition<bool>(this.hasLaunchCharges, this.notready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim(new Func<TravelTubeEntrance.SMInstance, string>(TravelTubeEntrance.GetLaunchAnimName), KAnim.PlayMode.Once).QueueAnim("working_loop", true, null).Enter(new StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State.Callback(TravelTubeEntrance.SetTravelerGleamEffect)).WorkableStopTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x04003A93 RID: 14995
		public StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.BoolParameter hasLaunchCharges;

		// Token: 0x04003A94 RID: 14996
		public TravelTubeEntrance.States.NotOperationalStates notoperational;

		// Token: 0x04003A95 RID: 14997
		public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notready;

		// Token: 0x04003A96 RID: 14998
		public TravelTubeEntrance.States.ReadyStates ready;

		// Token: 0x02001011 RID: 4113
		public class NotOperationalStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x04003A97 RID: 14999
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State normal;

			// Token: 0x04003A98 RID: 15000
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notube;
		}

		// Token: 0x02001012 RID: 4114
		public class ReadyStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x04003A99 RID: 15001
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State free;

			// Token: 0x04003A9A RID: 15002
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State occupied;

			// Token: 0x04003A9B RID: 15003
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State post;
		}
	}

	// Token: 0x02001014 RID: 4116
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x060053FB RID: 21499 RVA: 0x000D6B8D File Offset: 0x000D4D8D
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.showProgressBar = false;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_tube_launcher_kanim")
			};
			this.workLayer = Grid.SceneLayer.BuildingUse;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x000D6BC9 File Offset: 0x000D4DC9
		protected override void OnStartWork(WorkerBase worker)
		{
			base.SetWorkTime(1f);
		}

		// Token: 0x04003AA4 RID: 15012
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";
	}
}
