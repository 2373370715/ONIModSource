using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class IceKettle : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public SimHashes exhaust_tag;

		public Tag targetElementTag;

		public Tag fuelElementTag;

		public float KGToMeltPerBatch;

		public float KGMeltedPerSecond;

		public float TargetTemperature;

		public float EnergyPerUnitOfLumber;

		public float ExhaustMassPerUnitOfLumber;

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			string txt = string.Format(UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond));
			string tooltip = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(KGToMeltPerBatch), GameUtil.GetFormattedTemperature(TargetTemperature));
			Descriptor item = new Descriptor(txt, tooltip);
			list.Add(item);
			return list;
		}
	}

	public class WorkingStates : State
	{
		public State idle;

		public State complete;
	}

	public class MeltingStates : State
	{
		public State entering;

		public WorkingStates working;

		public State exit;
	}

	public class IdleStates : State
	{
		public State notEnoughFuel;

		public State waitingForSolids;

		public State waitingForSpaceInLiquidTank;
	}

	public class OperationalStates : State
	{
		public MeltingStates melting;

		public IdleStates idle;
	}

	public new class Instance : GameInstance
	{
		private Storage fuelStorage;

		private Storage kettleStorage;

		private Storage outputStorage;

		private Element elementToMelt;

		private MeterController LiquidMeter;

		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		private IceKettleWorkable dupeWorkable;

		[MyCmpGet]
		private KBatchedAnimController animController;

		public float CurrentTemperatureOfSolidsStored
		{
			get
			{
				if (!(kettleStorage.MassStored() > 0f))
				{
					return 0f;
				}
				return kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
		}

		public float MeltDurationPerBatch => base.def.KGToMeltPerBatch / base.def.KGMeltedPerSecond;

		public float FuelUnitsAvailable => fuelStorage.MassStored();

		public bool HasAtLeastOneBatchOfSolidsWaitingToMelt => kettleStorage.MassStored() >= base.def.KGToMeltPerBatch;

		public bool HasEnoughFuelUnitsToMeltNextBatch
		{
			get
			{
				if (!(kettleStorage.MassStored() > 0f))
				{
					return true;
				}
				return FuelUnitsAvailable >= FuelRequiredForNextBratch;
			}
		}

		public bool LiquidTankHasCapacityForNextBatch => outputStorage.RemainingCapacity() >= base.def.KGToMeltPerBatch;

		public float LiquidTankCapacity => outputStorage.capacityKg;

		public float LiquidStored => outputStorage.MassStored();

		public float FuelRequiredForNextBratch => GetUnitsOfFuelRequiredToMelt(elementToMelt, base.def.KGToMeltPerBatch, CurrentTemperatureOfSolidsStored);

		public float InUseWorkableDuration => dupeWorkable.workTime;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			elementToMelt = ElementLoader.GetElement(def.targetElementTag);
			LiquidMeter = new MeterController(animController, LIQUID_METER_TARGET_NAME, LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			fuelStorage = components[0];
			kettleStorage = components[1];
			outputStorage = components[2];
		}

		public override void StartSM()
		{
			base.StartSM();
			UpdateMeter();
		}

		public void UpdateMeter()
		{
			LiquidMeter.SetPositionPercent(outputStorage.MassStored() / outputStorage.capacityKg);
		}

		public void MeltNextBatch()
		{
			if (HasAtLeastOneBatchOfSolidsWaitingToMelt)
			{
				PrimaryElement component = kettleStorage.FindFirst(base.def.targetElementTag).GetComponent<PrimaryElement>();
				float num = Mathf.Min(GetUnitsOfFuelRequiredToMelt(elementToMelt, base.def.KGToMeltPerBatch, component.Temperature), FuelUnitsAvailable);
				float amount_consumed = 0f;
				float aggregate_temperature = 0f;
				kettleStorage.ConsumeAndGetDisease(elementToMelt.id.CreateTag(), base.def.KGToMeltPerBatch, out amount_consumed, out var disease_info, out aggregate_temperature);
				outputStorage.AddElement(elementToMelt.highTempTransitionTarget, amount_consumed, base.def.TargetTemperature, disease_info.idx, disease_info.count);
				float temperature = fuelStorage.FindFirst(base.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
				fuelStorage.ConsumeIgnoringDisease(base.def.fuelElementTag, num);
				float mass = num * base.def.ExhaustMassPerUnitOfLumber;
				Element element = ElementLoader.FindElementByHash(base.def.exhaust_tag);
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.gameObject), element.id, null, mass, temperature, byte.MaxValue, 0);
			}
		}

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
	}

	public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";

	public static string LIQUID_METER_ANIM_NAME = "meter_kettle";

	public static string IDEL_ANIM_STATE = "on";

	public static string BOILING_PRE_ANIM_NAME = "boiling_pre";

	public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";

	public static string BOILING_PST_ANIM_NAME = "boiling_pst";

	private const float InUseTimeout = 5f;

	public State noOperational;

	public OperationalStates operational;

	public State inUse;

	public FloatParameter MeltingTimer;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = noOperational;
		root.EventHandlerTransition(GameHashes.WorkableStartWork, inUse, (Instance smi, object obj) => true).EventHandler(GameHashes.OnStorageChange, delegate(Instance smi)
		{
			smi.UpdateMeter();
		});
		noOperational.TagTransition(GameTags.Operational, operational);
		operational.TagTransition(GameTags.Operational, noOperational, on_remove: true).DefaultState(operational.idle);
		operational.idle.PlayAnim(IDEL_ANIM_STATE).DefaultState(operational.idle.waitingForSolids);
		operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids).EventTransition(GameHashes.OnStorageChange, operational.idle.waitingForSpaceInLiquidTank, (Instance smi) => HasEnoughSolidsToMelt(smi));
		operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace).EventTransition(GameHashes.OnStorageChange, operational.idle.notEnoughFuel, LiquidTankHasCapacityForNextBatch);
		operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel).EventTransition(GameHashes.OnStorageChange, operational.melting, CanMeltNextBatch);
		operational.melting.Toggle("Operational Active State", SetOperationalActiveStatesTrue, SetOperationalActiveStatesFalse).DefaultState(operational.melting.entering);
		operational.melting.entering.PlayAnim(BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(operational.melting.working);
		operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting).DefaultState(operational.melting.working.idle).PlayAnim(BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
		operational.melting.working.idle.ParamTransition(MeltingTimer, operational.melting.working.complete, IsDoneMelting).Update(MeltingTimerUpdate);
		operational.melting.working.complete.Enter(ResetMeltingTimer).Enter(MeltNextBatch).EnterTransition(operational.melting.working.idle, CanMeltNextBatch)
			.EnterTransition(operational.melting.exit, GameStateMachine<IceKettle, Instance, IStateMachineTarget, Def>.Not(CanMeltNextBatch));
		operational.melting.exit.PlayAnim(BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(operational.idle);
		inUse.EventHandlerTransition(GameHashes.WorkableStopWork, noOperational, (Instance smi, object obj) => true).ScheduleGoTo(GetInUseTimeout, noOperational);
	}

	public static void SetOperationalActiveStatesTrue(Instance smi)
	{
		smi.operational.SetActive(value: true);
	}

	public static void SetOperationalActiveStatesFalse(Instance smi)
	{
		smi.operational.SetActive(value: false);
	}

	public static float GetInUseTimeout(Instance smi)
	{
		return smi.InUseWorkableDuration + 1f;
	}

	public static void ResetMeltingTimer(Instance smi)
	{
		smi.sm.MeltingTimer.Set(0f, smi);
	}

	public static bool HasEnoughSolidsToMelt(Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
	}

	public static bool LiquidTankHasCapacityForNextBatch(Instance smi)
	{
		return smi.LiquidTankHasCapacityForNextBatch;
	}

	public static bool HasEnoughFuelForNextBacth(Instance smi)
	{
		return smi.HasEnoughFuelUnitsToMeltNextBatch;
	}

	public static bool CanMeltNextBatch(Instance smi)
	{
		if (smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && LiquidTankHasCapacityForNextBatch(smi))
		{
			return HasEnoughFuelForNextBacth(smi);
		}
		return false;
	}

	public static bool IsDoneMelting(Instance smi, float timePassed)
	{
		return timePassed >= smi.MeltDurationPerBatch;
	}

	public static void MeltingTimerUpdate(Instance smi, float dt)
	{
		float num = smi.sm.MeltingTimer.Get(smi);
		smi.sm.MeltingTimer.Set(num + dt, smi);
	}

	public static void MeltNextBatch(Instance smi)
	{
		smi.MeltNextBatch();
	}
}
