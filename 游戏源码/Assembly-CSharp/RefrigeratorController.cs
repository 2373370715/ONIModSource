using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000F4E RID: 3918
public class RefrigeratorController : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>
{
	// Token: 0x06004F3D RID: 20285 RVA: 0x0026AC20 File Offset: 0x00268E20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.DefaultState(this.operational.steady).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Not(new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational))).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(false, false);
		});
		this.operational.cooling.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplyCoolingExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.steady, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AllFoodCool), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeCooling, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main);
		this.operational.steady.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplySteadyExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.cooling, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AnyWarmFood), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeSteady, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(true);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(false);
		});
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x0026AE50 File Offset: 0x00269050
	private bool AllFoodCool(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x0026AEF0 File Offset: 0x002690F0
	private bool AnyWarmFood(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x000D3A30 File Offset: 0x000D1C30
	private bool IsOperational(RefrigeratorController.StatesInstance smi)
	{
		return smi.operational.IsOperational;
	}

	// Token: 0x0400374F RID: 14159
	public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State inoperational;

	// Token: 0x04003750 RID: 14160
	public RefrigeratorController.OperationalStates operational;

	// Token: 0x02000F4F RID: 3919
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004F42 RID: 20290 RVA: 0x0026AF90 File Offset: 0x00269190
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature));
			Descriptor item = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.coolingHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item);
			return list;
		}

		// Token: 0x04003751 RID: 14161
		public float activeCoolingStartBuffer = 2f;

		// Token: 0x04003752 RID: 14162
		public float activeCoolingStopBuffer = 0.1f;

		// Token: 0x04003753 RID: 14163
		public float simulatedInternalTemperature = 274.15f;

		// Token: 0x04003754 RID: 14164
		public float simulatedInternalHeatCapacity = 400f;

		// Token: 0x04003755 RID: 14165
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x04003756 RID: 14166
		public float powerSaverEnergyUsage;

		// Token: 0x04003757 RID: 14167
		public float coolingHeatKW;

		// Token: 0x04003758 RID: 14168
		public float steadyHeatKW;
	}

	// Token: 0x02000F50 RID: 3920
	public class OperationalStates : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State
	{
		// Token: 0x04003759 RID: 14169
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State cooling;

		// Token: 0x0400375A RID: 14170
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State steady;
	}

	// Token: 0x02000F51 RID: 3921
	public class StatesInstance : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.GameInstance
	{
		// Token: 0x06004F45 RID: 20293 RVA: 0x0026B000 File Offset: 0x00269200
		public StatesInstance(IStateMachineTarget master, RefrigeratorController.Def def) : base(master, def)
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, this.storage);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x000D3A8C File Offset: 0x000D1C8C
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
			base.OnCleanUp();
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x000D3A9F File Offset: 0x000D1C9F
		public float GetSaverPower()
		{
			return base.def.powerSaverEnergyUsage;
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x000D3AAC File Offset: 0x000D1CAC
		public float GetNormalPower()
		{
			return base.GetComponent<EnergyConsumer>().WattsNeededWhenActive;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x0026B050 File Offset: 0x00269250
		public void SetEnergySaver(bool energySaving)
		{
			EnergyConsumer component = base.GetComponent<EnergyConsumer>();
			if (energySaving)
			{
				component.BaseWattageRating = this.GetSaverPower();
				return;
			}
			component.BaseWattageRating = this.GetNormalPower();
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x000D3AB9 File Offset: 0x000D1CB9
		public void ApplyCoolingExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x000D3AE3 File Offset: 0x000D1CE3
		public void ApplySteadyExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x0400375B RID: 14171
		[MyCmpReq]
		public Operational operational;

		// Token: 0x0400375C RID: 14172
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400375D RID: 14173
		private HandleVector<int>.Handle structureTemperature;

		// Token: 0x0400375E RID: 14174
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
