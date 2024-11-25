﻿using System;
using Klei;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), this, "TurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		this.destCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -1));
			this.destCells[i] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
		}
		this.smi = new Turbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

		private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
		this.smi.UpdateMeter();
	}

		protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "Turbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

		private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), this, "TurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

		private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((Turbine)data).OnSimConsume(mass_cb_info);
	}

		private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minEmitMass && this.simEmitCBHandle.IsValid())
			{
				float mass = this.storedMass / (float)this.destCells.Length;
				int disease_count = this.diseaseCount / this.destCells.Length;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				int[] array = this.destCells;
				for (int i = 0; i < array.Length; i++)
				{
					SimMessages.EmitMass(array[i], mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
				}
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

		private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((Turbine)data).OnSimEmitted(info);
	}

		private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded != 1)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
			this.storedMass += info.mass;
			if (info.diseaseIdx != 255)
			{
				SimUtil.DiseaseInfo a = new SimUtil.DiseaseInfo
				{
					idx = this.diseaseIdx,
					count = this.diseaseCount
				};
				SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo
				{
					idx = info.diseaseIdx,
					count = info.diseaseCount
				};
				SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
				this.diseaseIdx = diseaseInfo.idx;
				this.diseaseCount = diseaseInfo.count;
			}
		}
	}

		public static void InitializeStatusItems()
	{
		Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = string.Format(str, (int)turbine.currentRPM);
			return str;
		};
		Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientMassStatusItem.resolveTooltipCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
			return str;
		};
		Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
		Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
	}

		private static string ResolveStrings(string str, object data)
	{
		Turbine turbine = (Turbine)data;
		str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
		str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		return str;
	}

		public SimHashes srcElem;

		public float requiredMassFlowDifferential = 3f;

		public float activePercent = 0.75f;

		public float minEmitMass;

		public float minActiveTemperature = 400f;

		public float emitTemperature = 300f;

		public float maxRPM;

		public float rpmAcceleration;

		public float rpmDeceleration;

		public float minGenerationRPM;

		public float pumpKGRate;

		private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

		[Serialize]
	private float storedMass;

		[Serialize]
	private float storedTemperature;

		[Serialize]
	private byte diseaseIdx = byte.MaxValue;

		[Serialize]
	private int diseaseCount;

		[MyCmpGet]
	private Generator generator;

		[Serialize]
	private float currentRPM;

		private int[] srcCells;

		private int[] destCells;

		private Turbine.Instance smi;

		private static StatusItem inputBlockedStatusItem;

		private static StatusItem outputBlockedStatusItem;

		private static StatusItem insufficientMassStatusItem;

		private static StatusItem insufficientTemperatureStatusItem;

		private static StatusItem activeStatusItem;

		private static StatusItem spinningUpStatusItem;

		private MeterController meter;

		private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

		public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Turbine.InitializeStatusItems();
			default_state = this.operational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (Turbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null).Enter(delegate(Turbine.Instance smi)
			{
				smi.master.currentRPM = 0f;
				smi.UpdateMeter();
			});
			this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (Turbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(Turbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(Turbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.spinningUp.ToggleStatusItem((Turbine.Instance smi) => Turbine.spinningUpStatusItem, (Turbine.Instance smi) => smi.master).QueueAnim("buildup", true, null);
			this.operational.active.Update("UpdateActive", delegate(Turbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((Turbine.Instance smi) => Turbine.activeStatusItem, (Turbine.Instance smi) => smi.master).Enter(delegate(Turbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Turbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

				public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;

				public Turbine.States.OperationalStates operational;

				private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

				public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
		{
						public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;

						public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;

						public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
		}
	}

		public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
	{
				public Instance(Turbine master) : base(master)
		{
		}

				public void UpdateState(float dt)
		{
			float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? base.master.rpmAcceleration : (-base.master.rpmDeceleration);
			base.master.currentRPM = Mathf.Clamp(base.master.currentRPM + dt * num, 0f, base.master.maxRPM);
			this.UpdateMeter();
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (base.master.currentRPM >= base.master.minGenerationRPM)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
				}
				base.smi.master.generator.GenerateJoules(base.smi.master.generator.WattageRating * dt, false);
				return;
			}
			if (base.master.currentRPM > 0f)
			{
				if (currentState != base.sm.operational.spinningUp)
				{
					base.smi.GoTo(base.sm.operational.spinningUp);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

				public void UpdateMeter()
		{
			if (base.master.meter != null)
			{
				float num = Mathf.Clamp01(base.master.currentRPM / base.master.maxRPM);
				base.master.meter.SetPositionPercent(num);
				base.master.meter.SetSymbolTint(Turbine.TINT_SYMBOL, (num >= base.master.activePercent) ? Color.green : Color.red);
			}
		}

				private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			this.isInputBlocked = false;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num4 = base.master.srcCells[i];
				float b = Grid.Mass[num4];
				if (Grid.Element[num4].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
				}
				float b2 = Grid.Temperature[num4];
				num2 = Mathf.Max(num2, b2);
				ushort index = Grid.ElementIdx[num4];
				Element element = ElementLoader.elements[(int)index];
				if (element.IsLiquid || element.IsSolid)
				{
					this.isInputBlocked = true;
				}
			}
			this.isOutputBlocked = false;
			for (int j = 0; j < base.master.destCells.Length; j++)
			{
				int i2 = base.master.destCells[j];
				float b3 = Grid.Mass[i2];
				num3 = Mathf.Min(num3, b3);
				ushort index2 = Grid.ElementIdx[i2];
				Element element2 = ElementLoader.elements[(int)index2];
				if (element2.IsLiquid || element2.IsSolid)
				{
					this.isOutputBlocked = true;
				}
			}
			insufficient_mass = (num - num3 < base.master.requiredMassFlowDifferential);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

				public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
			this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
			this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
		}

				private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
		{
			Guid result = current_handle;
			if (show != (current_handle != Guid.Empty))
			{
				if (show)
				{
					result = ksel.AddStatusItem(item, base.master);
				}
				else
				{
					result = ksel.RemoveStatusItem(current_handle, false);
				}
			}
			return result;
		}

				public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.inputBlockedHandle, false);
			component.RemoveStatusItem(this.outputBlockedHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
		}

				public bool isInputBlocked;

				public bool isOutputBlocked;

				public bool insufficientMass;

				public bool insufficientTemperature;

				private Guid inputBlockedHandle = Guid.Empty;

				private Guid outputBlockedHandle = Guid.Empty;

				private Guid insufficientMassHandle = Guid.Empty;

				private Guid insufficientTemperatureHandle = Guid.Empty;
	}
}
