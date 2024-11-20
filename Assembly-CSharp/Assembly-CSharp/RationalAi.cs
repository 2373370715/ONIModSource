﻿using System;
using UnityEngine;

public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RationalAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RationalAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.TagTransition(GameTags.Dead, this.dead, false).ToggleStateMachine((RationalAi.Instance smi) => new ThoughtGraph.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new Dreamer.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new StaminaMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new StressMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new EmoteMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new SneezeMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new DecorMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new IncapacitationMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new IdleMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new RationMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new CalorieMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new DoctorMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new SicknessMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new GermExposureMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new RoomMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new TemperatureMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new ExternalTemperatureMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new ScaldingMonitor.Instance(smi.master, new ScaldingMonitor.Def
		{
			defaultScaldingTreshold = 345f
		})).ToggleStateMachine((RationalAi.Instance smi) => new ColdImmunityMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new HeatImmunityMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new BladderMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new LightMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new RadiationMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new RedAlertMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new CringeMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new HygieneMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new FallMonitor.Instance(smi.master, true, "anim_emotes_default_kanim")).ToggleStateMachine((RationalAi.Instance smi) => new ThreatMonitor.Instance(smi.master, new ThreatMonitor.Def
		{
			fleethresholdState = Health.HealthState.Critical,
			offsets = MinionConfig.ATTACK_OFFSETS
		})).ToggleStateMachine((RationalAi.Instance smi) => new WoundMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new TiredMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new MoveToLocationMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new RocketPassengerMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new ReactionMonitor.Instance(smi.master, new ReactionMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new SuitWearer.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new TubeTraveller.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new MingleMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new MournMonitor.Instance(smi.master)).ToggleStateMachine((RationalAi.Instance smi) => new SpeechMonitor.Instance(smi.master, new SpeechMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new BlinkMonitor.Instance(smi.master, new BlinkMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new ConversationMonitor.Instance(smi.master, new ConversationMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new CoughMonitor.Instance(smi.master, new CoughMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new GameplayEventMonitor.Instance(smi.master, new GameplayEventMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new GasLiquidExposureMonitor.Instance(smi.master, new GasLiquidExposureMonitor.Def())).ToggleStateMachine((RationalAi.Instance smi) => new InspirationEffectMonitor.Instance(smi.master, new InspirationEffectMonitor.Def()));
		this.dead.ToggleStateMachine((RationalAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).ToggleBrain("dead").Enter("RefreshUserMenu", delegate(RationalAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RationalAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;

	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

	public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		public Instance(IStateMachineTarget master) : base(master)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
		}

		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}
	}
}
