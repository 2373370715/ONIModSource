using System;
using UnityEngine;

// Token: 0x02001595 RID: 5525
public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	// Token: 0x060072C0 RID: 29376 RVA: 0x002FE7F8 File Offset: 0x002FC9F8
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

	// Token: 0x040055CD RID: 21965
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x040055CE RID: 21966
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	// Token: 0x040055CF RID: 21967
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x040055D0 RID: 21968
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	// Token: 0x040055D1 RID: 21969
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x040055D2 RID: 21970
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x040055D3 RID: 21971
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x040055D4 RID: 21972
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x02001596 RID: 5526
	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060072C2 RID: 29378 RVA: 0x002FE9F0 File Offset: 0x002FCBF0
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.CanBeIncapacitated = true;
			}
		}

		// Token: 0x060072C3 RID: 29379 RVA: 0x000EB28A File Offset: 0x000E948A
		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		// Token: 0x060072C4 RID: 29380 RVA: 0x002FEA1C File Offset: 0x002FCC1C
		public void RecoverStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		// Token: 0x060072C5 RID: 29381 RVA: 0x000EB2B2 File Offset: 0x000E94B2
		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		// Token: 0x060072C6 RID: 29382 RVA: 0x002FEA78 File Offset: 0x002FCC78
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
