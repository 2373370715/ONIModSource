using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	private class LaunchReactable : WorkableReactable
	{
		private TravelTubeEntrance entrance;

		public LaunchReactable(Workable workable, TravelTubeEntrance entrance)
			: base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance)
		{
			this.entrance = entrance;
		}

		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				if (!component)
				{
					return false;
				}
				return entrance.HasChargeSlotReserved(component);
			}
			return false;
		}
	}

	private class WaitReactable : Reactable
	{
		private TravelTubeEntrance entrance;

		public WaitReactable(TravelTubeEntrance entrance)
			: base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1)
		{
			this.entrance = entrance;
			preventChoreInterruption = false;
		}

		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (reactor != null)
			{
				return false;
			}
			if (entrance == null)
			{
				Cleanup();
				return false;
			}
			return entrance.ShouldWait(new_reactor);
		}

		protected override void InternalBegin()
		{
			KBatchedAnimController component = reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre");
			component.Queue("idle_default", KAnim.PlayMode.Loop);
			entrance.OrphanWaitReactable();
			entrance.CreateNewWaitReactable();
		}

		public override void Update(float dt)
		{
			if (entrance == null)
			{
				Cleanup();
			}
			else if (!entrance.ShouldWait(reactor))
			{
				Cleanup();
			}
		}

		protected override void InternalEnd()
		{
			if (reactor != null)
			{
				reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		protected override void InternalCleanup()
		{
		}
	}

	public class SMInstance : GameStateMachine<States, SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		public SMInstance(TravelTubeEntrance master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, SMInstance, TravelTubeEntrance>
	{
		public class NotOperationalStates : State
		{
			public State normal;

			public State notube;
		}

		public class ReadyStates : State
		{
			public State free;

			public State occupied;

			public State post;
		}

		public BoolParameter hasLaunchCharges;

		public NotOperationalStates notoperational;

		public State notready;

		public ReadyStates ready;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = notoperational;
			root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge);
			notoperational.DefaultState(notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, ready);
			notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, notoperational.notube, (SMInstance smi) => !smi.master.operational.GetFlag(tubeConnected));
			notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, notoperational.normal, (SMInstance smi) => smi.master.operational.GetFlag(tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected);
			notready.PlayAnim("off").ParamTransition(hasLaunchCharges, ready, (SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, notoperational, on_remove: true);
			ready.DefaultState(ready.free).ToggleReactable((SMInstance smi) => new LaunchReactable(smi.master.GetComponent<Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition(hasLaunchCharges, notready, (SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges)
				.TagTransition(GameTags.Operational, notoperational, on_remove: true);
			ready.free.PlayAnim("on").WorkableStartTransition((SMInstance smi) => smi.GetComponent<Work>(), ready.occupied);
			ready.occupied.PlayAnim(GetLaunchAnimName).QueueAnim("working_loop", loop: true).Enter(SetTravelerGleamEffect)
				.WorkableStopTransition((SMInstance smi) => smi.GetComponent<Work>(), ready.post);
			ready.post.PlayAnim("working_pst").OnAnimQueueComplete(ready);
		}
	}

	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			resetProgressOnStop = true;
			showProgressBar = false;
			overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_tube_launcher_kanim") };
			workLayer = Grid.SceneLayer.BuildingUse;
		}

		protected override void OnStartWork(Worker worker)
		{
			SetWorkTime(1f);
		}
	}

	[MyCmpReq]
	private Operational operational;

	[MyCmpReq]
	private Work launch_workable;

	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	[MyCmpGet]
	private BuildingEnabledButton button;

	[MyCmpReq]
	private KSelectable selectable;

	[MyCmpReq]
	private Storage storage;

	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	public float jouleCapacity = 1f;

	public float joulesPerLaunch = 1f;

	public float waxPerLaunch;

	[Serialize]
	private float availableJoules;

	[Serialize]
	private bool deliverAndUseWax;

	private TravelTube travelTube;

	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	private WaitReactable wait_reactable;

	private MeterController meter;

	private MeterController waxMeter;

	private const int MAX_CHARGES = 3;

	private const float RECHARGE_TIME = 10f;

	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	private HandleVector<int>.Handle tubeChangedEntry;

	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	private Guid connectedStatus;

	public float AvailableJoules => availableJoules;

	public float TotalCapacity => jouleCapacity;

	public float UsageJoules => joulesPerLaunch;

	public bool HasLaunchPower => availableJoules > joulesPerLaunch;

	public bool HasWaxForGreasyLaunch => storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= waxPerLaunch;

	public int WaxLaunchesAvailable => Mathf.FloorToInt(storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / waxPerLaunch);

	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			if (deliverAndUseWax)
			{
				return HasWaxForGreasyLaunch;
			}
			return false;
		}
	}

	public static void SetTravelerGleamEffect(SMInstance smi)
	{
		Work component = smi.GetComponent<Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	public static string GetLaunchAnimName(SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		energyConsumer.OnConnectionChanged += OnConnectionChanged;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetWaxUse(deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), is_physical_building: true);
		TubeConnectionsChanged(connections);
		tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], TubeChanged);
		Subscribe(-592767678, OnOperationalChangedDelegate);
		Subscribe(-1697596308, OnStorageChanged);
		meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
		waxMeter = new MeterController(GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
		CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(availableJoules / joulesPerLaunch));
		base.smi.StartSM();
		UpdateWaxCharge();
		UpdateCharge();
		Subscribe(493375141, OnRefreshUserMenuDelegate);
	}

	private void OnStorageChanged(object obj)
	{
		UpdateWaxCharge();
	}

	protected override void OnCleanUp()
	{
		if (travelTube != null)
		{
			travelTube.Unsubscribe(-1041684577, TubeConnectionsChanged);
			travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref tubeChangedEntry);
		base.OnCleanUp();
	}

	private void OnRefreshUserMenu(object data)
	{
		if (!deliverAndUseWax)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate
			{
				SetWaxUse(usingWax: true);
			}, Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP));
		}
		else
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME, delegate
			{
				SetWaxUse(usingWax: false);
			}, Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP));
		}
		KSelectable component = GetComponent<KSelectable>();
		bool flag = deliverAndUseWax && WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
			}
			else
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady);
			}
		}
	}

	public void SetWaxUse(bool usingWax)
	{
		deliverAndUseWax = usingWax;
		manualDelivery.AbortDelivery("Switching to new delivery request");
		manualDelivery.capacity = (usingWax ? storage.capacityKg : 0f);
		manualDelivery.refillMass = (usingWax ? waxPerLaunch : 0f);
		manualDelivery.MinimumMass = (usingWax ? waxPerLaunch : 0f);
		if (!usingWax)
		{
			storage.DropAll();
		}
		OnRefreshUserMenu(null);
	}

	private void TubeChanged(object data)
	{
		if (travelTube != null)
		{
			travelTube.Unsubscribe(-1041684577, TubeConnectionsChanged);
			travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data != null)
		{
			TravelTube component = gameObject.GetComponent<TravelTube>();
			if (component != null)
			{
				component.Subscribe(-1041684577, TubeConnectionsChanged);
				travelTube = component;
			}
			else
			{
				TubeConnectionsChanged(0);
			}
		}
		else
		{
			TubeConnectionsChanged(0);
		}
	}

	private void TubeConnectionsChanged(object data)
	{
		bool value = (UtilityConnections)data == UtilityConnections.Up;
		operational.SetFlag(tubeConnected, value);
	}

	private bool CanAcceptMorePower()
	{
		if (operational.IsOperational && (button == null || button.IsEnabled) && energyConsumer.IsExternallyPowered)
		{
			return availableJoules < jouleCapacity;
		}
		return false;
	}

	public void Sim200ms(float dt)
	{
		if (CanAcceptMorePower())
		{
			availableJoules = Mathf.Min(jouleCapacity, availableJoules + energyConsumer.WattsUsed * dt);
			UpdateCharge();
		}
		energyConsumer.SetSustained(HasLaunchPower);
		UpdateActive();
		UpdateConnectionStatus();
	}

	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, reserve: true);
	}

	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, reserve: false);
	}

	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	public bool ShouldWait(GameObject reactor)
	{
		if (!operational.IsOperational)
		{
			return false;
		}
		if (!HasLaunchPower)
		{
			return false;
		}
		if (launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance sMI = reactor.GetSMI<TubeTraveller.Instance>();
		return HasChargeSlotReserved(sMI, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	public void ConsumeCharge(GameObject reactor)
	{
		if (!HasLaunchPower)
		{
			return;
		}
		availableJoules -= joulesPerLaunch;
		if (deliverAndUseWax && HasWaxForGreasyLaunch)
		{
			TubeTraveller.Instance sMI = reactor.GetSMI<TubeTraveller.Instance>();
			if (sMI != null)
			{
				Tag source = SimHashes.MilkFat.CreateTag();
				storage.ConsumeAndGetDisease(source, waxPerLaunch, out var _, out var disease_info, out var _);
				reactor.GetSMI<GermExposureMonitor.Instance>()?.TryInjectDisease(disease_info.idx, disease_info.count, source, Sickness.InfectionVector.Contact);
				sMI.SetWaxState(isWaxed: true);
			}
		}
		UpdateCharge();
		UpdateWaxCharge();
	}

	private void CreateNewWaitReactable()
	{
		if (wait_reactable == null)
		{
			wait_reactable = new WaitReactable(this);
		}
	}

	private void OrphanWaitReactable()
	{
		wait_reactable = null;
	}

	private void ClearWaitReactable()
	{
		if (wait_reactable != null)
		{
			wait_reactable.Cleanup();
			wait_reactable = null;
		}
	}

	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), flag);
		UpdateActive();
	}

	private void OnConnectionChanged()
	{
		UpdateActive();
		UpdateConnectionStatus();
	}

	private void UpdateActive()
	{
		operational.SetActive(CanAcceptMorePower());
	}

	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(HasLaunchPower, base.smi);
		float positionPercent = Mathf.Clamp01(availableJoules / jouleCapacity);
		meter.SetPositionPercent(positionPercent);
		energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(availableJoules / joulesPerLaunch));
		OnRefreshUserMenu(null);
	}

	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(storage.MassStored() / storage.capacityKg);
		waxMeter.SetPositionPercent(positionPercent);
	}

	private void UpdateConnectionStatus()
	{
		bool num = button != null && !button.IsEnabled;
		bool isConnected = energyConsumer.IsConnected;
		bool hasLaunchPower = HasLaunchPower;
		if (num || !isConnected || hasLaunchPower)
		{
			connectedStatus = selectable.RemoveStatusItem(connectedStatus);
		}
		else if (connectedStatus == Guid.Empty)
		{
			connectedStatus = selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower);
		}
	}
}
