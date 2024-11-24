using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class MercuryLight : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public float MAX_LUX;

		public float TURN_ON_DELAY;

		public float FUEL_MASS_PER_SECOND;

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			string arg = ELEMENT_TAG.ProperName();
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Requirement);
			list.Add(item);
			return list;
		}
	}

	public class LightStates : State
	{
		public State charging;

		public State idle;
	}

	public class Darknesstates : State
	{
		public State depleating;

		public State depleated;

		public State idle;

		public State exit;
	}

	public class OperationalStates : State
	{
		public LightStates light;

		public Darknesstates darkness;
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		private Light2D light;

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private ConduitConsumer conduitConsumer;

		private MeterController lightIntensityMeterController;

		public bool HasEnoughFuel => base.sm.HasEnoughFuel.Get(this);

		public int LuxLevel => Mathf.FloorToInt(base.smi.ChargeLevel * base.def.MAX_LUX);

		public float ChargeLevel => base.smi.sm.Charge.Get(this);

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			lightIntensityMeterController = new MeterController(component, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building);
		}

		public override void StartSM()
		{
			base.StartSM();
			SetChargeLevel(ChargeLevel);
		}

		public void DepleteUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(ChargeLevel - dt / base.def.TURN_ON_DELAY, 0f, 1f);
			SetChargeLevel(chargeLevel);
		}

		public void ChargeUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(ChargeLevel + dt / base.def.TURN_ON_DELAY, 0f, 1f);
			SetChargeLevel(chargeLevel);
		}

		public void SetChargeLevel(float value)
		{
			base.sm.Charge.Set(value, this);
			light.Lux = LuxLevel;
			light.FullRefresh();
			bool flag = ChargeLevel > 0f;
			if (light.enabled != flag)
			{
				light.enabled = flag;
			}
			lightIntensityMeterController.SetPositionPercent(value);
		}

		public void ConsumeFuelUpdate(float dt)
		{
			float num = base.def.FUEL_MASS_PER_SECOND * dt;
			if (storage.MassStored() < num)
			{
				base.sm.HasEnoughFuel.Set(value: false, this);
				return;
			}
			storage.ConsumeAndGetDisease(ELEMENT_TAG, num, out var _, out var _, out var _);
			base.sm.HasEnoughFuel.Set(value: true, this);
		}

		public bool CanRun()
		{
			return true;
		}
	}

	private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();

	private const string ON_ANIM_NAME = "on";

	private const string ON_PRE_ANIM_NAME = "on_pre";

	private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";

	private const string DEPLEATING_ANIM_NAME = "depleating";

	private const string OFF_ANIM_NAME = "off";

	private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";

	private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";

	public Darknesstates noOperational;

	public OperationalStates operational;

	public FloatParameter Charge;

	public BoolParameter HasEnoughFuel;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = noOperational;
		noOperational.Enter(SetOperationalActiveFlagOff).ParamTransition(Charge, noOperational.depleating, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsGTZero).ParamTransition(Charge, noOperational.idle, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsLTEZero);
		noOperational.depleating.TagTransition(GameTags.Operational, operational).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight)
			.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating)
			.ParamTransition(Charge, noOperational.depleated, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsLTEZero)
			.Update(DepleteUpdate);
		noOperational.depleated.TagTransition(GameTags.Operational, operational).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(noOperational.idle);
		noOperational.idle.TagTransition(GameTags.Operational, noOperational.exit).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated);
		noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(operational);
		operational.TagTransition(GameTags.Operational, noOperational, on_remove: true).DefaultState(operational.darkness).Update(ConsumeFuelUpdate);
		operational.darkness.Enter(SetOperationalActiveFlagOff).ParamTransition(HasEnoughFuel, operational.light, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsTrue).ParamTransition(Charge, operational.darkness.depleating, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsGTZero)
			.ParamTransition(Charge, operational.darkness.idle, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsLTEZero);
		operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating)
			.ParamTransition(Charge, operational.darkness.depleated, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsLTEZero)
			.Update(DepleteUpdate);
		operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(operational.darkness.idle);
		operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated).ParamTransition(Charge, operational.darkness.depleating, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsGTZero);
		operational.light.Enter(SetOperationalActiveFlagOn).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition(HasEnoughFuel, operational.darkness, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsFalse)
			.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight)
			.DefaultState(operational.light.charging);
		operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging).ParamTransition(Charge, operational.light.idle, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsGTEOne).Update(ChargeUpdate);
		operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged).ParamTransition(Charge, operational.light.charging, GameStateMachine<MercuryLight, Instance, IStateMachineTarget, Def>.IsLTOne);
	}

	public static void SetOperationalActiveFlagOn(Instance smi)
	{
		smi.operational.SetActive(value: true);
	}

	public static void SetOperationalActiveFlagOff(Instance smi)
	{
		smi.operational.SetActive(value: false);
	}

	public static void DepleteUpdate(Instance smi, float dt)
	{
		smi.DepleteUpdate(dt);
	}

	public static void ChargeUpdate(Instance smi, float dt)
	{
		smi.ChargeUpdate(dt);
	}

	public static void ConsumeFuelUpdate(Instance smi, float dt)
	{
		smi.ConsumeFuelUpdate(dt);
	}
}
