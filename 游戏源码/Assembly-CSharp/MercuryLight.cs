using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000EA0 RID: 3744
public class MercuryLight : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
	// Token: 0x06004B7D RID: 19325 RVA: 0x0025EC28 File Offset: 0x0025CE28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<float>(this.Charge, this.noOperational.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.noOperational.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.noOperational.depleating.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.noOperational.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.noOperational.depleated.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.idle.TagTransition(GameTags.Operational, this.noOperational.exit, false).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null);
		this.noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.darkness).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ConsumeFuelUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<bool>(this.HasEnoughFuel, this.operational.light, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsTrue).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.operational.darkness.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.darkness.idle);
		this.operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero);
		this.operational.light.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOn)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.HasEnoughFuel, this.operational.darkness, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).DefaultState(this.operational.light.charging);
		this.operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging, null).ParamTransition<float>(this.Charge, this.operational.light.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTEOne).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ChargeUpdate), UpdateRate.SIM_200ms, false);
		this.operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged, null).ParamTransition<float>(this.Charge, this.operational.light.charging, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTOne);
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x000D0BAD File Offset: 0x000CEDAD
	public static void SetOperationalActiveFlagOn(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x000D0BBC File Offset: 0x000CEDBC
	public static void SetOperationalActiveFlagOff(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x000D0BCB File Offset: 0x000CEDCB
	public static void DepleteUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.DepleteUpdate(dt);
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x000D0BD4 File Offset: 0x000CEDD4
	public static void ChargeUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ChargeUpdate(dt);
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x000D0BDD File Offset: 0x000CEDDD
	public static void ConsumeFuelUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ConsumeFuelUpdate(dt);
	}

	// Token: 0x0400344B RID: 13387
	private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();

	// Token: 0x0400344C RID: 13388
	private const string ON_ANIM_NAME = "on";

	// Token: 0x0400344D RID: 13389
	private const string ON_PRE_ANIM_NAME = "on_pre";

	// Token: 0x0400344E RID: 13390
	private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";

	// Token: 0x0400344F RID: 13391
	private const string DEPLEATING_ANIM_NAME = "depleating";

	// Token: 0x04003450 RID: 13392
	private const string OFF_ANIM_NAME = "off";

	// Token: 0x04003451 RID: 13393
	private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";

	// Token: 0x04003452 RID: 13394
	private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";

	// Token: 0x04003453 RID: 13395
	public MercuryLight.Darknesstates noOperational;

	// Token: 0x04003454 RID: 13396
	public MercuryLight.OperationalStates operational;

	// Token: 0x04003455 RID: 13397
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.FloatParameter Charge;

	// Token: 0x04003456 RID: 13398
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.BoolParameter HasEnoughFuel;

	// Token: 0x02000EA1 RID: 3745
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004B85 RID: 19333 RVA: 0x0025F068 File Offset: 0x0025D268
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			string arg = MercuryLight.ELEMENT_TAG.ProperName();
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
			return list;
		}

		// Token: 0x04003457 RID: 13399
		public float MAX_LUX;

		// Token: 0x04003458 RID: 13400
		public float TURN_ON_DELAY;

		// Token: 0x04003459 RID: 13401
		public float FUEL_MASS_PER_SECOND;
	}

	// Token: 0x02000EA2 RID: 3746
	public class LightStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x0400345A RID: 13402
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State charging;

		// Token: 0x0400345B RID: 13403
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
	}

	// Token: 0x02000EA3 RID: 3747
	public class Darknesstates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x0400345C RID: 13404
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleating;

		// Token: 0x0400345D RID: 13405
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleated;

		// Token: 0x0400345E RID: 13406
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;

		// Token: 0x0400345F RID: 13407
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State exit;
	}

	// Token: 0x02000EA4 RID: 3748
	public class OperationalStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04003460 RID: 13408
		public MercuryLight.LightStates light;

		// Token: 0x04003461 RID: 13409
		public MercuryLight.Darknesstates darkness;
	}

	// Token: 0x02000EA5 RID: 3749
	public new class Instance : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.GameInstance
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06004B8A RID: 19338 RVA: 0x000D0C07 File Offset: 0x000CEE07
		public bool HasEnoughFuel
		{
			get
			{
				return base.sm.HasEnoughFuel.Get(this);
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06004B8B RID: 19339 RVA: 0x000D0C1A File Offset: 0x000CEE1A
		public int LuxLevel
		{
			get
			{
				return Mathf.FloorToInt(base.smi.ChargeLevel * base.def.MAX_LUX);
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06004B8C RID: 19340 RVA: 0x000D0C39 File Offset: 0x000CEE39
		public float ChargeLevel
		{
			get
			{
				return base.smi.sm.Charge.Get(this);
			}
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x0025F0DC File Offset: 0x0025D2DC
		public Instance(IStateMachineTarget master, MercuryLight.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.lightIntensityMeterController = new MeterController(component, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x000D0C51 File Offset: 0x000CEE51
		public override void StartSM()
		{
			base.StartSM();
			this.SetChargeLevel(this.ChargeLevel);
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x0025F118 File Offset: 0x0025D318
		public void DepleteUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel - dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x0025F150 File Offset: 0x0025D350
		public void ChargeUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel + dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x0025F188 File Offset: 0x0025D388
		public void SetChargeLevel(float value)
		{
			base.sm.Charge.Set(value, this, false);
			this.light.Lux = this.LuxLevel;
			this.light.FullRefresh();
			bool flag = this.ChargeLevel > 0f;
			if (this.light.enabled != flag)
			{
				this.light.enabled = flag;
			}
			this.lightIntensityMeterController.SetPositionPercent(value);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x0025F1FC File Offset: 0x0025D3FC
		public void ConsumeFuelUpdate(float dt)
		{
			float num = base.def.FUEL_MASS_PER_SECOND * dt;
			if (this.storage.MassStored() < num)
			{
				base.sm.HasEnoughFuel.Set(false, this, false);
				return;
			}
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float num3;
			this.storage.ConsumeAndGetDisease(MercuryLight.ELEMENT_TAG, num, out num2, out diseaseInfo, out num3);
			base.sm.HasEnoughFuel.Set(true, this, false);
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanRun()
		{
			return true;
		}

		// Token: 0x04003462 RID: 13410
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04003463 RID: 13411
		[MyCmpGet]
		private Light2D light;

		// Token: 0x04003464 RID: 13412
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04003465 RID: 13413
		[MyCmpGet]
		private ConduitConsumer conduitConsumer;

		// Token: 0x04003466 RID: 13414
		private MeterController lightIntensityMeterController;
	}
}
