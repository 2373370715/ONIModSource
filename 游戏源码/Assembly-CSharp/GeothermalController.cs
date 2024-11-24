using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
	public class ReconnectPipes : Workable
	{
		[MyCmpGet]
		private Storage storage;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			SetWorkTime(5f);
			overrideAnims = new KAnimFile[1] { Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE) };
			synchronizeAnims = false;
			faceTargetWhenWorking = true;
		}

		protected override void OnCompleteWork(Worker worker)
		{
			base.OnCompleteWork(worker);
			if (storage != null)
			{
				storage.ConsumeAllIgnoringDisease();
			}
		}
	}

	private class VentRegistrationListener
	{
		public Action<GeothermalVent> onAdd;

		public Action<GeothermalVent> onRemove;
	}

	public enum ProgressState
	{
		NOT_STARTED,
		FETCHING_STEEL,
		RECONNECTING_PIPES,
		NOTIFY_REPAIRED,
		REPAIRED,
		AT_CAPACITY,
		COMPLETE
	}

	private class ImpuritiesHelper
	{
		public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();

		public void AddMaterial(ushort elementIdx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			Element element = ElementLoader.elements[elementIdx];
			if (element.lowTemp > temperature)
			{
				Element lowTempTransition = element.lowTempTransition;
				Element element2 = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
				AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.lowTempTransitionOreMassConversion)));
				if (element2 != null)
				{
					AddMaterial(element2.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.lowTempTransitionOreMassConversion));
				}
				return;
			}
			if (element.highTemp < temperature)
			{
				Element highTempTransition = element.highTempTransition;
				Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
				AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.highTempTransitionOreMassConversion)));
				if (element3 != null)
				{
					AddMaterial(element3.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.highTempTransitionOreMassConversion));
				}
				return;
			}
			GeothermalVent.ElementInfo item = default(GeothermalVent.ElementInfo);
			for (int i = 0; i < results.Count; i++)
			{
				if (results[i].elementIdx == elementIdx)
				{
					item = results[i];
					item.mass += mass;
					results[i] = item;
					return;
				}
			}
			item.elementIdx = elementIdx;
			item.mass = mass;
			item.temperature = temperature;
			item.diseaseCount = diseaseCount;
			item.diseaseIdx = diseaseIdx;
			item.isSolid = ElementLoader.elements[elementIdx].IsSolid;
			results.Add(item);
		}
	}

	public class States : GameStateMachine<States, StatesInstance, GeothermalController>
	{
		public class OfflineStates : State
		{
			public class FilledStates : State
			{
				public State ready;

				public State obstructed;
			}

			public State initial;

			public State fetchSteel;

			public State checkSupplies;

			public State reconnectPipes;

			public State notifyRepaired;

			public State repaired;

			public State filling;

			public FilledStates filled;
		}

		public class OnlineStates : State
		{
			public class WorkingStates : State
			{
				public State pre;

				public State loop;

				public State post;
			}

			public State active;

			public WorkingStates venting;

			public State obstructed;
		}

		public OfflineStates offline;

		public OnlineStates online;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			root.EnterTransition(online, (StatesInstance smi) => smi.master.State == ProgressState.COMPLETE).EnterTransition(offline, (StatesInstance smi) => smi.master.State != ProgressState.COMPLETE);
			offline.EnterTransition(offline.initial, (StatesInstance smi) => smi.master.State == ProgressState.NOT_STARTED).EnterTransition(offline.fetchSteel, (StatesInstance smi) => smi.master.State == ProgressState.FETCHING_STEEL).EnterTransition(offline.reconnectPipes, (StatesInstance smi) => smi.master.State == ProgressState.RECONNECTING_PIPES)
				.EnterTransition(offline.notifyRepaired, (StatesInstance smi) => smi.master.State == ProgressState.NOTIFY_REPAIRED)
				.EnterTransition(offline.filling, (StatesInstance smi) => smi.master.State == ProgressState.REPAIRED)
				.EnterTransition(offline.filled, (StatesInstance smi) => smi.master.State == ProgressState.AT_CAPACITY)
				.PlayAnim("off");
			offline.initial.Enter(delegate(StatesInstance smi)
			{
				smi.master.storage.DropAll();
			}).Transition(offline.fetchSteel, (StatesInstance smi) => smi.master.State == ProgressState.FETCHING_STEEL).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline);
			offline.fetchSteel.ToggleChore((StatesInstance smi) => CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored()), offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, (StatesInstance smi) => smi.GetFetchListForStatusItem());
			offline.checkSupplies.EnterTransition(offline.fetchSteel, (StatesInstance smi) => smi.master.storage.MassStored() < 1200f).EnterTransition(offline.reconnectPipes, (StatesInstance smi) => smi.master.storage.MassStored() >= 1200f).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline);
			offline.reconnectPipes.Enter(delegate(StatesInstance smi)
			{
				smi.master.state = ProgressState.RECONNECTING_PIPES;
			}).ToggleChore((StatesInstance smi) => CreateRepairChore(smi), offline.notifyRepaired, offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes);
			offline.notifyRepaired.Enter(delegate(StatesInstance smi)
			{
				smi.master.state = ProgressState.NOTIFY_REPAIRED;
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline).ToggleNotification((StatesInstance smi) => CreateRepairedNotification(smi))
				.ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
			offline.repaired.Exit(delegate(StatesInstance smi)
			{
				smi.master.State = ProgressState.REPAIRED;
			}).PlayAnim("on_pre").OnAnimQueueComplete(offline.filling)
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master);
			offline.filling.PlayAnim("on").Enter(delegate(StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).ToggleOperationalFlag(allowInputFlag)
				.Transition(offline.filled, (StatesInstance smi) => smi.master.IsFull())
				.Update(delegate(StatesInstance smi, float _)
				{
					smi.master.UpdatePressure();
				}, UpdateRate.SIM_1000ms)
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master);
			offline.filled.Enter(delegate(StatesInstance smi)
			{
				smi.master.state = ProgressState.AT_CAPACITY;
				smi.master.TryAddConduitConsumers();
			}).ToggleNotification((StatesInstance smi) => smi.master.CreateFirstBatchReadyNotification()).EnterTransition(offline.filled.ready, (StatesInstance smi) => !smi.master.IsObstructed())
				.EnterTransition(offline.filled.obstructed, (StatesInstance smi) => smi.master.IsObstructed())
				.ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
			offline.filled.ready.PlayAnim("on").Transition(offline.filled.obstructed, (StatesInstance smi) => smi.master.IsObstructed()).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master);
			offline.filled.obstructed.Transition(offline.filled.ready, (StatesInstance smi) => !smi.master.IsObstructed()).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (StatesInstance smi) => smi.master);
			online.Enter(delegate(StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).defaultState = online.active;
			online.active.PlayAnim("on").Transition(online.venting, (StatesInstance smi) => smi.master.IsFull() && !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Transition(online.obstructed, (StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_1000ms)
				.Update(delegate(StatesInstance smi, float _)
				{
					smi.master.UpdatePressure();
				}, UpdateRate.SIM_1000ms)
				.ToggleOperationalFlag(allowInputFlag)
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master);
			online.venting.Transition(online.obstructed, (StatesInstance smi) => smi.master.IsObstructed()).Enter(delegate(StatesInstance smi)
			{
				smi.master.PushToVents();
			}).PlayAnim("venting_loop", KAnim.PlayMode.Loop)
				.Update(delegate(StatesInstance smi, float f)
				{
					smi.master.FakeMeterDraining(f);
				}, UpdateRate.SIM_1000ms)
				.ScheduleGoTo(16f, online.active)
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master);
			online.obstructed.Transition(online.active, (StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (StatesInstance smi) => smi.master)
				.ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
		}

		protected Chore CreateRepairFetchChore(StatesInstance smi, HashSet<Tag> tags, float mass_required)
		{
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, run_until_complete: true, null, null, null, Operational.State.None);
		}

		protected Chore CreateRepairChore(StatesInstance smi)
		{
			return new WorkChore<ReconnectPipes>(Db.Get().ChoreTypes.Repair, smi.master, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false, null, is_preemptable: false, allow_in_context_menu: true, allow_prioritization: true, PriorityScreen.PriorityClass.high);
		}

		protected Notification CreateRepairedNotification(StatesInstance smi)
		{
			smi.master.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP, null, expires: false, 0f, delegate
			{
				smi.master.dismissOnSelect = null;
				SetProgressionToRepaired(smi);
			}, null, null, volume_attenuation: true, clear_on_click: true);
			return smi.master.dismissOnSelect;
		}

		protected void SetProgressionToRepaired(StatesInstance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
			GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, "geothermalplantonline_kanim", delegate
			{
				smi.GoTo(offline.repaired);
				SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), skipSound: true);
			}, smi.master.transform);
		}
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, GeothermalController, object>.GameInstance, ISidescreenButtonControl
	{
		protected class FakeList : IFetchList
		{
			public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();

			Storage IFetchList.Destination
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			float IFetchList.GetMinimumAmount(Tag tag)
			{
				throw new NotImplementedException();
			}

			Dictionary<Tag, float> IFetchList.GetRemaining()
			{
				return remaining;
			}

			Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
			{
				throw new NotImplementedException();
			}
		}

		string ISidescreenButtonControl.SidescreenButtonText => getSidescreenButtonText();

		string ISidescreenButtonControl.SidescreenButtonTooltip => getSidescreenButtonTooltip();

		public StatesInstance(GeothermalController smi)
			: base(smi)
		{
		}

		public IFetchList GetFetchListForStatusItem()
		{
			FakeList fakeList = new FakeList();
			float value = 1200f - base.smi.master.storage.MassStored();
			fakeList.remaining[GameTagExtensions.Create(SimHashes.Steel)] = value;
			return fakeList;
		}

		bool ISidescreenButtonControl.SidescreenButtonInteractable()
		{
			switch (base.smi.master.State)
			{
			case ProgressState.NOT_STARTED:
			case ProgressState.FETCHING_STEEL:
			case ProgressState.RECONNECTING_PIPES:
				return true;
			case ProgressState.NOTIFY_REPAIRED:
			case ProgressState.REPAIRED:
				return false;
			case ProgressState.AT_CAPACITY:
				return !base.smi.master.IsObstructed();
			case ProgressState.COMPLETE:
				return false;
			default:
				return false;
			}
		}

		bool ISidescreenButtonControl.SidescreenEnabled()
		{
			return base.smi.master.State != ProgressState.COMPLETE;
		}

		private string getSidescreenButtonText()
		{
			switch (base.smi.master.State)
			{
			case ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
			case ProgressState.FETCHING_STEEL:
			case ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
			case ProgressState.NOTIFY_REPAIRED:
			case ProgressState.REPAIRED:
			case ProgressState.AT_CAPACITY:
			case ProgressState.COMPLETE:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
			default:
				return "";
			}
		}

		private string getSidescreenButtonTooltip()
		{
			switch (base.smi.master.State)
			{
			case ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
			case ProgressState.FETCHING_STEEL:
			case ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
			case ProgressState.NOTIFY_REPAIRED:
			case ProgressState.REPAIRED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
			case ProgressState.AT_CAPACITY:
			case ProgressState.COMPLETE:
				if (base.smi.master.IsObstructed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
			default:
				return "";
			}
		}

		void ISidescreenButtonControl.OnSidescreenButtonPressed()
		{
			switch (base.smi.master.state)
			{
			case ProgressState.NOT_STARTED:
				base.smi.master.State = ProgressState.FETCHING_STEEL;
				break;
			case ProgressState.FETCHING_STEEL:
			case ProgressState.RECONNECTING_PIPES:
				base.smi.master.State = ProgressState.NOT_STARTED;
				base.smi.GoTo(base.sm.offline.initial);
				break;
			case ProgressState.AT_CAPACITY:
			{
				MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2");
				bool num = base.smi.master.VentingCanFreeKeepsake();
				base.smi.master.state = ProgressState.COMPLETE;
				base.smi.GoTo(base.sm.online.venting);
				if (!num)
				{
					GeothermalFirstEmissionSequence.Start(base.smi.master);
				}
				break;
			}
			case ProgressState.NOTIFY_REPAIRED:
			case ProgressState.REPAIRED:
			case ProgressState.COMPLETE:
				break;
			}
		}

		void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		int ISidescreenButtonControl.HorizontalGroupID()
		{
			return -1;
		}

		int ISidescreenButtonControl.ButtonSideScreenSortOrder()
		{
			return 20;
		}
	}

	[MyCmpGet]
	private Storage storage;

	[MyCmpGet]
	private Operational operational;

	private MeterController thermometer;

	private MeterController barometer;

	private KBatchedAnimController animController;

	public Notification dismissOnSelect;

	public static Operational.Flag allowInputFlag = new Operational.Flag("allowInputFlag", Operational.Flag.Type.Requirement);

	private VentRegistrationListener listener;

	[Serialize]
	private ProgressState state;

	private float fakeProgress;

	public ProgressState State
	{
		get
		{
			return state;
		}
		protected set
		{
			state = value;
		}
	}

	public List<GeothermalVent> FindVents(bool requireEnabled)
	{
		if (!requireEnabled)
		{
			return Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId());
		}
		List<GeothermalVent> list = new List<GeothermalVent>();
		foreach (GeothermalVent item in FindVents(requireEnabled: false))
		{
			if (item.IsVentConnected())
			{
				list.Add(item);
			}
		}
		return list;
	}

	public void PushToVents(GeothermalVent.ElementInfo info)
	{
		List<GeothermalVent> list = FindVents(requireEnabled: true);
		if (list.Count != 0)
		{
			float[] array = new float[list.Count];
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = GeothermalControllerConfig.OUTPUT_VENT_WEIGHT_RANGE.Get();
				num += array[i];
			}
			GeothermalVent.ElementInfo info2 = info;
			for (int j = 0; j < list.Count; j++)
			{
				info2.mass = array[j] * info.mass / num;
				info2.diseaseCount = (int)(array[j] * (float)info.diseaseCount / num);
				list[j].addMaterial(info2);
			}
		}
	}

	public bool IsFull()
	{
		return storage.MassStored() > 11999.9f;
	}

	public float ComputeContentTemperature()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (GameObject item in storage.items)
		{
			PrimaryElement component = item.GetComponent<PrimaryElement>();
			float num3 = component.Mass * component.Element.specificHeatCapacity;
			num += num3 * component.Temperature;
			num2 += num3;
		}
		float result = 0f;
		if (num2 != 0f)
		{
			result = num / num2;
		}
		return result;
	}

	public List<GeothermalVent.ElementInfo> ComputeOutputs()
	{
		float num = ComputeContentTemperature();
		float temperature = GeothermalControllerConfig.CalculateOutputTemperature(num);
		ImpuritiesHelper impuritiesHelper = new ImpuritiesHelper();
		foreach (GameObject item in storage.items)
		{
			PrimaryElement component = item.GetComponent<PrimaryElement>();
			impuritiesHelper.AddMaterial(component.Element.idx, component.Mass * 0.92f, temperature, component.DiseaseIdx, component.DiseaseCount);
		}
		foreach (GeothermalControllerConfig.Impurity impurity in GeothermalControllerConfig.GetImpurities())
		{
			MathUtil.MinMax required_temp_range = impurity.required_temp_range;
			if (required_temp_range.Contains(num))
			{
				impuritiesHelper.AddMaterial(impurity.elementIdx, impurity.mass_kg, temperature, byte.MaxValue, 0);
			}
		}
		return impuritiesHelper.results;
	}

	public void PushToVents()
	{
		SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
		List<GeothermalVent.ElementInfo> list = ComputeOutputs();
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && list[0].temperature >= 602f)
		{
			GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
		}
		foreach (GeothermalVent.ElementInfo item in list)
		{
			PushToVents(item);
		}
		storage.ConsumeAllIgnoringDisease();
		fakeProgress = 1f;
	}

	private void TryAddConduitConsumers()
	{
		if (GetComponents<EntityConduitConsumer>().Length == 0)
		{
			CellOffset[] array = new CellOffset[3]
			{
				new CellOffset(0, 0),
				new CellOffset(2, 0),
				new CellOffset(-2, 0)
			};
			foreach (CellOffset offset in array)
			{
				EntityConduitConsumer entityConduitConsumer = base.gameObject.AddComponent<EntityConduitConsumer>();
				entityConduitConsumer.offset = offset;
				entityConduitConsumer.conduitType = ConduitType.Liquid;
			}
		}
	}

	public float GetPressure()
	{
		switch (state)
		{
		case ProgressState.NOT_STARTED:
		case ProgressState.FETCHING_STEEL:
		case ProgressState.RECONNECTING_PIPES:
			return 0f;
		default:
			return storage.MassStored() / 12000f;
		}
	}

	private void FakeMeterDraining(float time)
	{
		fakeProgress -= time / 16f;
		if (fakeProgress < 0f)
		{
			fakeProgress = 0f;
		}
		barometer.SetPositionPercent(fakeProgress);
	}

	private void UpdatePressure()
	{
		ProgressState progressState = state;
		if ((uint)progressState <= 2u)
		{
			return;
		}
		if ((uint)(progressState - 3) > 3u)
		{
		}
		float pressure = GetPressure();
		barometer.SetPositionPercent(pressure);
		float num = ComputeContentTemperature();
		if (num > 0f)
		{
			thermometer.SetPositionPercent((num - 50f) / 2450f);
		}
		int num2 = 0;
		for (int i = 1; i < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; i++)
		{
			if (pressure >= GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[i])
			{
				num2 = i;
			}
		}
		if (animController.GetCurrentAnim()?.name != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2])
		{
			animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2], KAnim.PlayMode.Loop);
		}
	}

	public bool IsObstructed()
	{
		if (IsFull())
		{
			bool flag = false;
			foreach (GeothermalVent item in FindVents(requireEnabled: false))
			{
				if (item.IsEntombed())
				{
					return true;
				}
				if (item.IsVentConnected())
				{
					if (!item.CanVent())
					{
						return true;
					}
					flag = true;
				}
			}
			return !flag;
		}
		return false;
	}

	public GeothermalVent FirstObstructedVent()
	{
		foreach (GeothermalVent item in FindVents(requireEnabled: false))
		{
			if (item.IsEntombed())
			{
				return item;
			}
			if (item.IsVentConnected() && !item.CanVent())
			{
				return item;
			}
		}
		return null;
	}

	public Notification CreateFirstBatchReadyNotification()
	{
		dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP, null, expires: false, 0f, null, null, base.transform);
		return dismissOnSelect;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
		operational.SetFlag(allowInputFlag, value: false);
		base.smi.StartSM();
		animController = GetComponent<KBatchedAnimController>();
		barometer = new MeterController(animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
		thermometer = new MeterController(animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
		Subscribe(-1503271301, OnBuildingSelected);
	}

	protected override void OnCleanUp()
	{
		Unsubscribe(-1503271301, OnBuildingSelected);
		if (listener != null)
		{
			Components.GeothermalVents.Unregister(this.GetMyWorldId(), listener.onAdd, listener.onRemove);
		}
		Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	protected void OnBuildingSelected(object clicked)
	{
		if ((bool)clicked && dismissOnSelect != null)
		{
			if (dismissOnSelect.customClickCallback != null)
			{
				dismissOnSelect.customClickCallback(dismissOnSelect.customClickData);
				return;
			}
			dismissOnSelect.Clear();
			dismissOnSelect = null;
		}
	}

	public bool VentingCanFreeKeepsake()
	{
		List<GeothermalVent.ElementInfo> list = ComputeOutputs();
		if (list.Count == 0)
		{
			return false;
		}
		return list[0].temperature >= 602f;
	}
}
