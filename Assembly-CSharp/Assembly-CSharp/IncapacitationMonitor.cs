﻿using System;
using UnityEngine;

public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.healthy.TagTransition(GameTags.CaloriesDepleted, this.incapacitated, false).TagTransition(GameTags.HitPointsDepleted, this.incapacitated, false).TagTransition(GameTags.HitByHighEnergyParticle, this.incapacitated, false).TagTransition(GameTags.RadiationSicknessIncapacitation, this.incapacitated, false).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.RecoverStamina(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.start_recovery.TagTransition(new Tag[]
		{
			GameTags.CaloriesDepleted,
			GameTags.HitPointsDepleted
		}, this.healthy, true);
		this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.start_recovery, null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((IncapacitationMonitor.Instance smi) => new BeIncapacitatedChore(smi.master), null).ParamTransition<float>(this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.Trigger(-1506500077, null);
		}).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.Bleed(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.die.Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation());
		});
	}

	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.CanBeIncapacitated = true;
			}
		}

		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		public void RecoverStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		public Death GetCauseOfIncapacitation()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (component.HasTag(GameTags.HitByHighEnergyParticle))
			{
				return Db.Get().Deaths.HitByHighEnergyParticle;
			}
			if (component.HasTag(GameTags.RadiationSicknessIncapacitation))
			{
				return Db.Get().Deaths.Radiation;
			}
			if (component.HasTag(GameTags.CaloriesDepleted))
			{
				return Db.Get().Deaths.Starvation;
			}
			if (component.HasTag(GameTags.HitPointsDepleted))
			{
				return Db.Get().Deaths.Slain;
			}
			return Db.Get().Deaths.Generic;
		}
	}
}
