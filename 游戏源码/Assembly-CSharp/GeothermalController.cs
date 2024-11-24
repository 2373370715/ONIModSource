using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA7 RID: 3495
public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06004490 RID: 17552 RVA: 0x000CC5D7 File Offset: 0x000CA7D7
	// (set) Token: 0x06004491 RID: 17553 RVA: 0x000CC5DF File Offset: 0x000CA7DF
	public GeothermalController.ProgressState State
	{
		get
		{
			return this.state;
		}
		protected set
		{
			this.state = value;
		}
	}

	// Token: 0x06004492 RID: 17554 RVA: 0x00248A04 File Offset: 0x00246C04
	public List<GeothermalVent> FindVents(bool requireEnabled)
	{
		if (!requireEnabled)
		{
			return Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId());
		}
		List<GeothermalVent> list = new List<GeothermalVent>();
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsVentConnected())
			{
				list.Add(geothermalVent);
			}
		}
		return list;
	}

	// Token: 0x06004493 RID: 17555 RVA: 0x00248A80 File Offset: 0x00246C80
	public void PushToVents(GeothermalVent.ElementInfo info)
	{
		List<GeothermalVent> list = this.FindVents(true);
		if (list.Count == 0)
		{
			return;
		}
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

	// Token: 0x06004494 RID: 17556 RVA: 0x000CC5E8 File Offset: 0x000CA7E8
	public bool IsFull()
	{
		return this.storage.MassStored() > 11999.9f;
	}

	// Token: 0x06004495 RID: 17557 RVA: 0x00248B2C File Offset: 0x00246D2C
	public float ComputeContentTemperature()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
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

	// Token: 0x06004496 RID: 17558 RVA: 0x00248BCC File Offset: 0x00246DCC
	public List<GeothermalVent.ElementInfo> ComputeOutputs()
	{
		float num = this.ComputeContentTemperature();
		float temperature = GeothermalControllerConfig.CalculateOutputTemperature(num);
		GeothermalController.ImpuritiesHelper impuritiesHelper = new GeothermalController.ImpuritiesHelper();
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
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

	// Token: 0x06004497 RID: 17559 RVA: 0x00248CD8 File Offset: 0x00246ED8
	public void PushToVents()
	{
		SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && list[0].temperature >= 602f)
		{
			GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
		}
		foreach (GeothermalVent.ElementInfo info in list)
		{
			this.PushToVents(info);
		}
		this.storage.ConsumeAllIgnoringDisease();
		this.fakeProgress = 1f;
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x00248D84 File Offset: 0x00246F84
	private void TryAddConduitConsumers()
	{
		if (base.GetComponents<EntityConduitConsumer>().Length != 0)
		{
			return;
		}
		foreach (CellOffset offset in new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(2, 0),
			new CellOffset(-2, 0)
		})
		{
			EntityConduitConsumer entityConduitConsumer = base.gameObject.AddComponent<EntityConduitConsumer>();
			entityConduitConsumer.offset = offset;
			entityConduitConsumer.conduitType = ConduitType.Liquid;
		}
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x00248DFC File Offset: 0x00246FFC
	public float GetPressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			return this.storage.MassStored() / 12000f;
		}
		return 0f;
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x000CC5FC File Offset: 0x000CA7FC
	private void FakeMeterDraining(float time)
	{
		this.fakeProgress -= time / 16f;
		if (this.fakeProgress < 0f)
		{
			this.fakeProgress = 0f;
		}
		this.barometer.SetPositionPercent(this.fakeProgress);
	}

	// Token: 0x0600449B RID: 17563 RVA: 0x00248E34 File Offset: 0x00247034
	private void UpdatePressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			float pressure = this.GetPressure();
			this.barometer.SetPositionPercent(pressure);
			float num = this.ComputeContentTemperature();
			if (num > 0f)
			{
				this.thermometer.SetPositionPercent((num - 50f) / 2450f);
			}
			int num2 = 0;
			for (int i = 1; i < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; i++)
			{
				if (pressure >= GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[i])
				{
					num2 = i;
				}
			}
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (((currentAnim != null) ? currentAnim.name : null) != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2])
			{
				this.animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2], KAnim.PlayMode.Loop, 1f, 0f);
			}
			return;
		}
	}

	// Token: 0x0600449C RID: 17564 RVA: 0x00248F04 File Offset: 0x00247104
	public bool IsObstructed()
	{
		if (this.IsFull())
		{
			bool flag = false;
			foreach (GeothermalVent geothermalVent in this.FindVents(false))
			{
				if (geothermalVent.IsEntombed())
				{
					return true;
				}
				if (geothermalVent.IsVentConnected())
				{
					if (!geothermalVent.CanVent())
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

	// Token: 0x0600449D RID: 17565 RVA: 0x00248F88 File Offset: 0x00247188
	public GeothermalVent FirstObstructedVent()
	{
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsEntombed())
			{
				return geothermalVent;
			}
			if (geothermalVent.IsVentConnected() && !geothermalVent.CanVent())
			{
				return geothermalVent;
			}
		}
		return null;
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x00248FF8 File Offset: 0x002471F8
	public Notification CreateFirstBatchReadyNotification()
	{
		this.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP, null, false, 0f, null, null, base.transform, true, false, false);
		return this.dismissOnSelect;
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x00249054 File Offset: 0x00247254
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
		this.operational.SetFlag(GeothermalController.allowInputFlag, false);
		base.smi.StartSM();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.barometer = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
		this.thermometer = new MeterController(this.animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x00249100 File Offset: 0x00247300
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
		if (this.listener != null)
		{
			Components.GeothermalVents.Unregister(this.GetMyWorldId(), this.listener.onAdd, this.listener.onRemove);
		}
		Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x0024916C File Offset: 0x0024736C
	protected void OnBuildingSelected(object clicked)
	{
		if (!(bool)clicked)
		{
			return;
		}
		if (this.dismissOnSelect != null)
		{
			if (this.dismissOnSelect.customClickCallback != null)
			{
				this.dismissOnSelect.customClickCallback(this.dismissOnSelect.customClickData);
				return;
			}
			this.dismissOnSelect.Clear();
			this.dismissOnSelect = null;
		}
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x002491C8 File Offset: 0x002473C8
	public bool VentingCanFreeKeepsake()
	{
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		return list.Count != 0 && list[0].temperature >= 602f;
	}

	// Token: 0x04002F24 RID: 12068
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002F25 RID: 12069
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F26 RID: 12070
	private MeterController thermometer;

	// Token: 0x04002F27 RID: 12071
	private MeterController barometer;

	// Token: 0x04002F28 RID: 12072
	private KBatchedAnimController animController;

	// Token: 0x04002F29 RID: 12073
	public Notification dismissOnSelect;

	// Token: 0x04002F2A RID: 12074
	public static Operational.Flag allowInputFlag = new Operational.Flag("allowInputFlag", Operational.Flag.Type.Requirement);

	// Token: 0x04002F2B RID: 12075
	private GeothermalController.VentRegistrationListener listener;

	// Token: 0x04002F2C RID: 12076
	[Serialize]
	private GeothermalController.ProgressState state;

	// Token: 0x04002F2D RID: 12077
	private float fakeProgress;

	// Token: 0x02000DA8 RID: 3496
	public class ReconnectPipes : Workable
	{
		// Token: 0x060044A5 RID: 17573 RVA: 0x000CC655 File Offset: 0x000CA855
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.SetWorkTime(5f);
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE)
			};
			this.synchronizeAnims = false;
			this.faceTargetWhenWorking = true;
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x000CC68F File Offset: 0x000CA88F
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.storage != null)
			{
				this.storage.ConsumeAllIgnoringDisease();
			}
		}

		// Token: 0x04002F2E RID: 12078
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02000DA9 RID: 3497
	private class VentRegistrationListener
	{
		// Token: 0x04002F2F RID: 12079
		public Action<GeothermalVent> onAdd;

		// Token: 0x04002F30 RID: 12080
		public Action<GeothermalVent> onRemove;
	}

	// Token: 0x02000DAA RID: 3498
	public enum ProgressState
	{
		// Token: 0x04002F32 RID: 12082
		NOT_STARTED,
		// Token: 0x04002F33 RID: 12083
		FETCHING_STEEL,
		// Token: 0x04002F34 RID: 12084
		RECONNECTING_PIPES,
		// Token: 0x04002F35 RID: 12085
		NOTIFY_REPAIRED,
		// Token: 0x04002F36 RID: 12086
		REPAIRED,
		// Token: 0x04002F37 RID: 12087
		AT_CAPACITY,
		// Token: 0x04002F38 RID: 12088
		COMPLETE
	}

	// Token: 0x02000DAB RID: 3499
	private class ImpuritiesHelper
	{
		// Token: 0x060044A9 RID: 17577 RVA: 0x002491FC File Offset: 0x002473FC
		public void AddMaterial(ushort elementIdx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			Element element = ElementLoader.elements[(int)elementIdx];
			if (element.lowTemp > temperature)
			{
				Element lowTempTransition = element.lowTempTransition;
				Element element2 = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
				this.AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.lowTempTransitionOreMassConversion)));
				if (element2 != null)
				{
					this.AddMaterial(element2.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.lowTempTransitionOreMassConversion));
				}
				return;
			}
			if (element.highTemp < temperature)
			{
				Element highTempTransition = element.highTempTransition;
				Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
				this.AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.highTempTransitionOreMassConversion)));
				if (element3 != null)
				{
					this.AddMaterial(element3.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.highTempTransitionOreMassConversion));
				}
				return;
			}
			GeothermalVent.ElementInfo elementInfo = default(GeothermalVent.ElementInfo);
			for (int i = 0; i < this.results.Count; i++)
			{
				if (this.results[i].elementIdx == elementIdx)
				{
					elementInfo = this.results[i];
					elementInfo.mass += mass;
					this.results[i] = elementInfo;
					return;
				}
			}
			elementInfo.elementIdx = elementIdx;
			elementInfo.mass = mass;
			elementInfo.temperature = temperature;
			elementInfo.diseaseCount = diseaseCount;
			elementInfo.diseaseIdx = diseaseIdx;
			elementInfo.isSolid = ElementLoader.elements[(int)elementIdx].IsSolid;
			this.results.Add(elementInfo);
		}

		// Token: 0x04002F39 RID: 12089
		public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();
	}

	// Token: 0x02000DAC RID: 3500
	public class States : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController>
	{
		// Token: 0x060044AB RID: 17579 RVA: 0x002493A8 File Offset: 0x002475A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.online, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.COMPLETE).EnterTransition(this.offline, (GeothermalController.StatesInstance smi) => smi.master.State != GeothermalController.ProgressState.COMPLETE);
			this.offline.EnterTransition(this.offline.initial, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOT_STARTED).EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.RECONNECTING_PIPES).EnterTransition(this.offline.notifyRepaired, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOTIFY_REPAIRED).EnterTransition(this.offline.filling, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.REPAIRED).EnterTransition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.AT_CAPACITY).PlayAnim("off");
			this.offline.initial.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.storage.DropAll(false, false, default(Vector3), true, null);
			}).Transition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.fetchSteel.ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored()), this.offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, (GeothermalController.StatesInstance smi) => smi.GetFetchListForStatusItem());
			this.offline.checkSupplies.EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() < 1200f).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() >= 1200f).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.reconnectPipes.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.RECONNECTING_PIPES;
			}).ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairChore(smi), this.offline.notifyRepaired, this.offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes, null);
			this.offline.notifyRepaired.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.NOTIFY_REPAIRED;
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleNotification((GeothermalController.StatesInstance smi) => this.CreateRepairedNotification(smi)).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.repaired.Exit(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.State = GeothermalController.ProgressState.REPAIRED;
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.offline.filling).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filling.PlayAnim("on").Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).ToggleOperationalFlag(GeothermalController.allowInputFlag).Transition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.IsFull(), UpdateRate.SIM_200ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.AT_CAPACITY;
				smi.master.TryAddConduitConsumers();
			}).ToggleNotification((GeothermalController.StatesInstance smi) => smi.master.CreateFirstBatchReadyNotification()).EnterTransition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed()).EnterTransition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed()).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.filled.ready.PlayAnim("on").Transition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.obstructed.Transition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_200ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).defaultState = this.online.active;
			this.online.active.PlayAnim("on").Transition(this.online.venting, (GeothermalController.StatesInstance smi) => smi.master.IsFull() && !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleOperationalFlag(GeothermalController.allowInputFlag).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.venting.Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.PushToVents();
			}).PlayAnim("venting_loop", KAnim.PlayMode.Loop).Update(delegate(GeothermalController.StatesInstance smi, float f)
			{
				smi.master.FakeMeterDraining(f);
			}, UpdateRate.SIM_1000ms, false).ScheduleGoTo(16f, this.online.active).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.obstructed.Transition(this.online.active, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00249EFC File Offset: 0x002480FC
		protected Chore CreateRepairFetchChore(GeothermalController.StatesInstance smi, HashSet<Tag> tags, float mass_required)
		{
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x00249F38 File Offset: 0x00248138
		protected Chore CreateRepairChore(GeothermalController.StatesInstance smi)
		{
			return new WorkChore<GeothermalController.ReconnectPipes>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00249F70 File Offset: 0x00248170
		protected Notification CreateRepairedNotification(GeothermalController.StatesInstance smi)
		{
			smi.master.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP, null, false, 0f, delegate(object _)
			{
				smi.master.dismissOnSelect = null;
				this.SetProgressionToRepaired(smi);
			}, null, null, true, true, false);
			return smi.master.dismissOnSelect;
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x00249FF8 File Offset: 0x002481F8
		protected void SetProgressionToRepaired(GeothermalController.StatesInstance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
			GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, "geothermalplantonline_kanim", delegate
			{
				smi.GoTo(this.offline.repaired);
				SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), true);
			}, smi.master.transform);
		}

		// Token: 0x04002F3A RID: 12090
		public GeothermalController.States.OfflineStates offline;

		// Token: 0x04002F3B RID: 12091
		public GeothermalController.States.OnlineStates online;

		// Token: 0x02000DAD RID: 3501
		public class OfflineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x04002F3C RID: 12092
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State initial;

			// Token: 0x04002F3D RID: 12093
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State fetchSteel;

			// Token: 0x04002F3E RID: 12094
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State checkSupplies;

			// Token: 0x04002F3F RID: 12095
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State reconnectPipes;

			// Token: 0x04002F40 RID: 12096
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State notifyRepaired;

			// Token: 0x04002F41 RID: 12097
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State repaired;

			// Token: 0x04002F42 RID: 12098
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State filling;

			// Token: 0x04002F43 RID: 12099
			public GeothermalController.States.OfflineStates.FilledStates filled;

			// Token: 0x02000DAE RID: 3502
			public class FilledStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x04002F44 RID: 12100
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State ready;

				// Token: 0x04002F45 RID: 12101
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;
			}
		}

		// Token: 0x02000DAF RID: 3503
		public class OnlineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x04002F46 RID: 12102
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State active;

			// Token: 0x04002F47 RID: 12103
			public GeothermalController.States.OnlineStates.WorkingStates venting;

			// Token: 0x04002F48 RID: 12104
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;

			// Token: 0x02000DB0 RID: 3504
			public class WorkingStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x04002F49 RID: 12105
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State pre;

				// Token: 0x04002F4A RID: 12106
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State loop;

				// Token: 0x04002F4B RID: 12107
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State post;
			}
		}
	}

	// Token: 0x02000DB4 RID: 3508
	public class StatesInstance : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x060044F0 RID: 17648 RVA: 0x000CC914 File Offset: 0x000CAB14
		public StatesInstance(GeothermalController smi) : base(smi)
		{
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0024A094 File Offset: 0x00248294
		public IFetchList GetFetchListForStatusItem()
		{
			GeothermalController.StatesInstance.FakeList fakeList = new GeothermalController.StatesInstance.FakeList();
			float value = 1200f - base.smi.master.storage.MassStored();
			fakeList.remaining[GameTagExtensions.Create(SimHashes.Steel)] = value;
			return fakeList;
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x0024A0D8 File Offset: 0x002482D8
		bool ISidescreenButtonControl.SidescreenButtonInteractable()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return true;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return false;
			case GeothermalController.ProgressState.AT_CAPACITY:
				return !base.smi.master.IsObstructed();
			case GeothermalController.ProgressState.COMPLETE:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x000CC91D File Offset: 0x000CAB1D
		bool ISidescreenButtonControl.SidescreenEnabled()
		{
			return base.smi.master.State != GeothermalController.ProgressState.COMPLETE;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x0024A138 File Offset: 0x00248338
		private string getSidescreenButtonText()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
			default:
				return "";
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x000CC935 File Offset: 0x000CAB35
		string ISidescreenButtonControl.SidescreenButtonText
		{
			get
			{
				return this.getSidescreenButtonText();
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x0024A1A0 File Offset: 0x002483A0
		private string getSidescreenButtonTooltip()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				if (base.smi.master.IsObstructed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
			default:
				return "";
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060044F7 RID: 17655 RVA: 0x000CC93D File Offset: 0x000CAB3D
		string ISidescreenButtonControl.SidescreenButtonTooltip
		{
			get
			{
				return this.getSidescreenButtonTooltip();
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x0024A230 File Offset: 0x00248430
		void ISidescreenButtonControl.OnSidescreenButtonPressed()
		{
			switch (base.smi.master.state)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				base.smi.master.State = GeothermalController.ProgressState.FETCHING_STEEL;
				return;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				base.smi.master.State = GeothermalController.ProgressState.NOT_STARTED;
				base.smi.GoTo(base.sm.offline.initial);
				return;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.COMPLETE:
				break;
			case GeothermalController.ProgressState.AT_CAPACITY:
			{
				MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2", false);
				bool flag = base.smi.master.VentingCanFreeKeepsake();
				base.smi.master.state = GeothermalController.ProgressState.COMPLETE;
				base.smi.GoTo(base.sm.online.venting);
				if (!flag)
				{
					GeothermalFirstEmissionSequence.Start(base.smi.master);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x000ABC75 File Offset: 0x000A9E75
		int ISidescreenButtonControl.HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x000ABCBD File Offset: 0x000A9EBD
		int ISidescreenButtonControl.ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x02000DB5 RID: 3509
		protected class FakeList : IFetchList
		{
			// Token: 0x17000357 RID: 855
			// (get) Token: 0x060044FC RID: 17660 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
			Storage IFetchList.Destination
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x060044FD RID: 17661 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
			float IFetchList.GetMinimumAmount(Tag tag)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060044FE RID: 17662 RVA: 0x000CC945 File Offset: 0x000CAB45
			Dictionary<Tag, float> IFetchList.GetRemaining()
			{
				return this.remaining;
			}

			// Token: 0x060044FF RID: 17663 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
			Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
			{
				throw new NotImplementedException();
			}

			// Token: 0x04002F83 RID: 12163
			public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();
		}
	}
}
