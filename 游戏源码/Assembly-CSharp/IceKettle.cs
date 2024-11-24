using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000E02 RID: 3586
public class IceKettle : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
	// Token: 0x0600468A RID: 18058 RVA: 0x0024F428 File Offset: 0x0024D628
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.root.EventHandlerTransition(GameHashes.WorkableStartWork, this.inUse, (IceKettle.Instance smi, object obj) => true).EventHandler(GameHashes.OnStorageChange, delegate(IceKettle.Instance smi)
		{
			smi.UpdateMeter();
		});
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.idle);
		this.operational.idle.PlayAnim(IceKettle.IDEL_ANIM_STATE).DefaultState(this.operational.idle.waitingForSolids);
		this.operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.waitingForSpaceInLiquidTank, (IceKettle.Instance smi) => IceKettle.HasEnoughSolidsToMelt(smi));
		this.operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.notEnoughFuel, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.LiquidTankHasCapacityForNextBatch));
		this.operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel, null).EventTransition(GameHashes.OnStorageChange, this.operational.melting, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch));
		this.operational.melting.Toggle("Operational Active State", new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesTrue), new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesFalse)).DefaultState(this.operational.melting.entering);
		this.operational.melting.entering.PlayAnim(IceKettle.BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.melting.working);
		this.operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting, null).DefaultState(this.operational.melting.working.idle).PlayAnim(IceKettle.BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
		this.operational.melting.working.idle.ParamTransition<float>(this.MeltingTimer, this.operational.melting.working.complete, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>.Callback(IceKettle.IsDoneMelting)).Update(new Action<IceKettle.Instance, float>(IceKettle.MeltingTimerUpdate), UpdateRate.SIM_200ms, false);
		this.operational.melting.working.complete.Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.ResetMeltingTimer)).Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.MeltNextBatch)).EnterTransition(this.operational.melting.working.idle, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)).EnterTransition(this.operational.melting.exit, GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Not(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)));
		this.operational.melting.exit.PlayAnim(IceKettle.BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.idle);
		this.inUse.EventHandlerTransition(GameHashes.WorkableStopWork, this.noOperational, (IceKettle.Instance smi, object obj) => true).ScheduleGoTo(new Func<IceKettle.Instance, float>(IceKettle.GetInUseTimeout), this.noOperational);
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x000CDB45 File Offset: 0x000CBD45
	public static void SetOperationalActiveStatesTrue(IceKettle.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x000CDB54 File Offset: 0x000CBD54
	public static void SetOperationalActiveStatesFalse(IceKettle.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x000CDB63 File Offset: 0x000CBD63
	public static float GetInUseTimeout(IceKettle.Instance smi)
	{
		return smi.InUseWorkableDuration + 1f;
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x000CDB71 File Offset: 0x000CBD71
	public static void ResetMeltingTimer(IceKettle.Instance smi)
	{
		smi.sm.MeltingTimer.Set(0f, smi, false);
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x000CDB8B File Offset: 0x000CBD8B
	public static bool HasEnoughSolidsToMelt(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x000CDB93 File Offset: 0x000CBD93
	public static bool LiquidTankHasCapacityForNextBatch(IceKettle.Instance smi)
	{
		return smi.LiquidTankHasCapacityForNextBatch;
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x000CDB9B File Offset: 0x000CBD9B
	public static bool HasEnoughFuelForNextBacth(IceKettle.Instance smi)
	{
		return smi.HasEnoughFuelUnitsToMeltNextBatch;
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x000CDBA3 File Offset: 0x000CBDA3
	public static bool CanMeltNextBatch(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && IceKettle.LiquidTankHasCapacityForNextBatch(smi) && IceKettle.HasEnoughFuelForNextBacth(smi);
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x000CDBBD File Offset: 0x000CBDBD
	public static bool IsDoneMelting(IceKettle.Instance smi, float timePassed)
	{
		return timePassed >= smi.MeltDurationPerBatch;
	}

	// Token: 0x06004694 RID: 18068 RVA: 0x0024F814 File Offset: 0x0024DA14
	public static void MeltingTimerUpdate(IceKettle.Instance smi, float dt)
	{
		float num = smi.sm.MeltingTimer.Get(smi);
		smi.sm.MeltingTimer.Set(num + dt, smi, false);
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x000CDBCB File Offset: 0x000CBDCB
	public static void MeltNextBatch(IceKettle.Instance smi)
	{
		smi.MeltNextBatch();
	}

	// Token: 0x040030CD RID: 12493
	public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";

	// Token: 0x040030CE RID: 12494
	public static string LIQUID_METER_ANIM_NAME = "meter_kettle";

	// Token: 0x040030CF RID: 12495
	public static string IDEL_ANIM_STATE = "on";

	// Token: 0x040030D0 RID: 12496
	public static string BOILING_PRE_ANIM_NAME = "boiling_pre";

	// Token: 0x040030D1 RID: 12497
	public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";

	// Token: 0x040030D2 RID: 12498
	public static string BOILING_PST_ANIM_NAME = "boiling_pst";

	// Token: 0x040030D3 RID: 12499
	private const float InUseTimeout = 5f;

	// Token: 0x040030D4 RID: 12500
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State noOperational;

	// Token: 0x040030D5 RID: 12501
	public IceKettle.OperationalStates operational;

	// Token: 0x040030D6 RID: 12502
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State inUse;

	// Token: 0x040030D7 RID: 12503
	public StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.FloatParameter MeltingTimer;

	// Token: 0x02000E03 RID: 3587
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004698 RID: 18072 RVA: 0x0024F84C File Offset: 0x0024DA4C
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			string txt = string.Format(UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string tooltip = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			Descriptor item = new Descriptor(txt, tooltip, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}

		// Token: 0x040030D8 RID: 12504
		public SimHashes exhaust_tag;

		// Token: 0x040030D9 RID: 12505
		public Tag targetElementTag;

		// Token: 0x040030DA RID: 12506
		public Tag fuelElementTag;

		// Token: 0x040030DB RID: 12507
		public float KGToMeltPerBatch;

		// Token: 0x040030DC RID: 12508
		public float KGMeltedPerSecond;

		// Token: 0x040030DD RID: 12509
		public float TargetTemperature;

		// Token: 0x040030DE RID: 12510
		public float EnergyPerUnitOfLumber;

		// Token: 0x040030DF RID: 12511
		public float ExhaustMassPerUnitOfLumber;
	}

	// Token: 0x02000E04 RID: 3588
	public class WorkingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040030E0 RID: 12512
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State idle;

		// Token: 0x040030E1 RID: 12513
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State complete;
	}

	// Token: 0x02000E05 RID: 3589
	public class MeltingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040030E2 RID: 12514
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State entering;

		// Token: 0x040030E3 RID: 12515
		public IceKettle.WorkingStates working;

		// Token: 0x040030E4 RID: 12516
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State exit;
	}

	// Token: 0x02000E06 RID: 3590
	public class IdleStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040030E5 RID: 12517
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State notEnoughFuel;

		// Token: 0x040030E6 RID: 12518
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSolids;

		// Token: 0x040030E7 RID: 12519
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSpaceInLiquidTank;
	}

	// Token: 0x02000E07 RID: 3591
	public class OperationalStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040030E8 RID: 12520
		public IceKettle.MeltingStates melting;

		// Token: 0x040030E9 RID: 12521
		public IceKettle.IdleStates idle;
	}

	// Token: 0x02000E08 RID: 3592
	public new class Instance : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.GameInstance
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600469E RID: 18078 RVA: 0x000CDC21 File Offset: 0x000CBE21
		public float CurrentTemperatureOfSolidsStored
		{
			get
			{
				if (this.kettleStorage.MassStored() <= 0f)
				{
					return 0f;
				}
				return this.kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x000CDC56 File Offset: 0x000CBE56
		public float MeltDurationPerBatch
		{
			get
			{
				return base.def.KGToMeltPerBatch / base.def.KGMeltedPerSecond;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x000CDC6F File Offset: 0x000CBE6F
		public float FuelUnitsAvailable
		{
			get
			{
				return this.fuelStorage.MassStored();
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060046A1 RID: 18081 RVA: 0x000CDC7C File Offset: 0x000CBE7C
		public bool HasAtLeastOneBatchOfSolidsWaitingToMelt
		{
			get
			{
				return this.kettleStorage.MassStored() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x000CDC99 File Offset: 0x000CBE99
		public bool HasEnoughFuelUnitsToMeltNextBatch
		{
			get
			{
				return this.kettleStorage.MassStored() <= 0f || this.FuelUnitsAvailable >= this.FuelRequiredForNextBratch;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x000CDCC0 File Offset: 0x000CBEC0
		public bool LiquidTankHasCapacityForNextBatch
		{
			get
			{
				return this.outputStorage.RemainingCapacity() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x000CDCDD File Offset: 0x000CBEDD
		public float LiquidTankCapacity
		{
			get
			{
				return this.outputStorage.capacityKg;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x000CDCEA File Offset: 0x000CBEEA
		public float LiquidStored
		{
			get
			{
				return this.outputStorage.MassStored();
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x000CDCF7 File Offset: 0x000CBEF7
		public float FuelRequiredForNextBratch
		{
			get
			{
				return this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, this.CurrentTemperatureOfSolidsStored);
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x000CDD16 File Offset: 0x000CBF16
		public float InUseWorkableDuration
		{
			get
			{
				return this.dupeWorkable.workTime;
			}
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x0024F8C8 File Offset: 0x0024DAC8
		public Instance(IStateMachineTarget master, IceKettle.Def def) : base(master, def)
		{
			this.elementToMelt = ElementLoader.GetElement(def.targetElementTag);
			this.LiquidMeter = new MeterController(this.animController, IceKettle.LIQUID_METER_TARGET_NAME, IceKettle.LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.fuelStorage = components[0];
			this.kettleStorage = components[1];
			this.outputStorage = components[2];
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x000CDD23 File Offset: 0x000CBF23
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateMeter();
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x000CDD31 File Offset: 0x000CBF31
		public void UpdateMeter()
		{
			this.LiquidMeter.SetPositionPercent(this.outputStorage.MassStored() / this.outputStorage.capacityKg);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0024F938 File Offset: 0x0024DB38
		public void MeltNextBatch()
		{
			if (!this.HasAtLeastOneBatchOfSolidsWaitingToMelt)
			{
				return;
			}
			PrimaryElement component = this.kettleStorage.FindFirst(base.def.targetElementTag).GetComponent<PrimaryElement>();
			float num = Mathf.Min(this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, component.Temperature), this.FuelUnitsAvailable);
			float mass = 0f;
			float num2 = 0f;
			SimUtil.DiseaseInfo diseaseInfo;
			this.kettleStorage.ConsumeAndGetDisease(this.elementToMelt.id.CreateTag(), base.def.KGToMeltPerBatch, out mass, out diseaseInfo, out num2);
			this.outputStorage.AddElement(this.elementToMelt.highTempTransitionTarget, mass, base.def.TargetTemperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			float temperature = this.fuelStorage.FindFirst(base.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
			this.fuelStorage.ConsumeIgnoringDisease(base.def.fuelElementTag, num);
			float mass2 = num * base.def.ExhaustMassPerUnitOfLumber;
			Element element = ElementLoader.FindElementByHash(base.def.exhaust_tag);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.gameObject), element.id, null, mass2, temperature, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x0024FA7C File Offset: 0x0024DC7C
		public float GetUnitsOfFuelRequiredToMelt(Element elementToMelt, float massToMelt_KG, float elementToMelt_initialTemperature)
		{
			if (!elementToMelt.IsSolid)
			{
				return -1f;
			}
			float num = massToMelt_KG * elementToMelt.specificHeatCapacity * elementToMelt_initialTemperature;
			float targetTemperature = base.def.TargetTemperature;
			return (massToMelt_KG * elementToMelt.specificHeatCapacity * targetTemperature - num) / base.def.EnergyPerUnitOfLumber;
		}

		// Token: 0x040030EA RID: 12522
		private Storage fuelStorage;

		// Token: 0x040030EB RID: 12523
		private Storage kettleStorage;

		// Token: 0x040030EC RID: 12524
		private Storage outputStorage;

		// Token: 0x040030ED RID: 12525
		private Element elementToMelt;

		// Token: 0x040030EE RID: 12526
		private MeterController LiquidMeter;

		// Token: 0x040030EF RID: 12527
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040030F0 RID: 12528
		[MyCmpGet]
		private IceKettleWorkable dupeWorkable;

		// Token: 0x040030F1 RID: 12529
		[MyCmpGet]
		private KBatchedAnimController animController;
	}
}
