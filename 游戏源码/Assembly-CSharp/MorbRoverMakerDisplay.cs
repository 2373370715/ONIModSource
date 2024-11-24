using System;

// Token: 0x020004B0 RID: 1200
public class MorbRoverMakerDisplay : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
	// Token: 0x06001537 RID: 5431 RVA: 0x00192BB8 File Offset: 0x00190DB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.off.idle;
		this.root.Target(this.monitor);
		this.off.DefaultState(this.off.idle);
		this.off.entering.PlayAnim("display_off").OnAnimQueueComplete(this.off.idle);
		this.off.idle.Target(this.masterTarget).EventTransition(GameHashes.TagsChanged, this.off.exiting, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.ShouldBeOn)).Target(this.monitor).PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
		this.off.exiting.PlayAnim("display_on").OnAnimQueueComplete(this.on);
		this.on.Target(this.masterTarget).TagTransition(GameTags.Operational, this.off.entering, true).Target(this.monitor).DefaultState(this.on.idle);
		this.on.idle.Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_idle", KAnim.PlayMode.Loop);
		this.on.noGerm.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
		this.on.germ.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_germ", KAnim.PlayMode.Loop);
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000AF72E File Offset: 0x000AD92E
	public static bool NoGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && !smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000AF743 File Offset: 0x000AD943
	public static bool HasGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x000AF755 File Offset: 0x000AD955
	public static bool ShouldBeOn(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.ShouldBeOn();
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000AF75D File Offset: 0x000AD95D
	public static bool GermsNoLongerNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return !smi.GermsAreNeeded;
	}

	// Token: 0x04000E45 RID: 3653
	public const string METER_TARGET_NAME = "meter_display_target";

	// Token: 0x04000E46 RID: 3654
	public const string OFF_IDLE_ANIM_NAME = "display_off_idle";

	// Token: 0x04000E47 RID: 3655
	public const string OFF_ENTERING_ANIM_NAME = "display_off";

	// Token: 0x04000E48 RID: 3656
	public const string OFF_EXITING_ANIM_NAME = "display_on";

	// Token: 0x04000E49 RID: 3657
	public const string GERM_ICON_ANIM_NAME = "display_germ";

	// Token: 0x04000E4A RID: 3658
	public const string NO_GERM_ANIM_NAME = "display_no_germ";

	// Token: 0x04000E4B RID: 3659
	public const string ON_IDLE_ANIM_NAME = "display_idle";

	// Token: 0x04000E4C RID: 3660
	public StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.TargetParameter monitor;

	// Token: 0x04000E4D RID: 3661
	public MorbRoverMakerDisplay.OffStates off;

	// Token: 0x04000E4E RID: 3662
	public MorbRoverMakerDisplay.OnStates on;

	// Token: 0x020004B1 RID: 1201
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000E4F RID: 3663
		public float Timeout = 1f;
	}

	// Token: 0x020004B2 RID: 1202
	public class OffStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04000E50 RID: 3664
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State entering;

		// Token: 0x04000E51 RID: 3665
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04000E52 RID: 3666
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State exiting;
	}

	// Token: 0x020004B3 RID: 1203
	public class OnStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04000E53 RID: 3667
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04000E54 RID: 3668
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State shake;

		// Token: 0x04000E55 RID: 3669
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State noGerm;

		// Token: 0x04000E56 RID: 3670
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State germ;

		// Token: 0x04000E57 RID: 3671
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State checkmark;
	}

	// Token: 0x020004B4 RID: 1204
	public new class Instance : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.GameInstance
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x000AF78B File Offset: 0x000AD98B
		public bool HasRecentlyConsumedGerms
		{
			get
			{
				return GameClock.Instance.GetTime() - this.lastTimeGermsConsumed < base.def.Timeout;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x000AF7AB File Offset: 0x000AD9AB
		public bool GermsAreNeeded
		{
			get
			{
				return this.morbRoverMaker.MorbDevelopment_Progress < 1f;
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00192DD0 File Offset: 0x00190FD0
		public Instance(IStateMachineTarget master, MorbRoverMakerDisplay.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.meter = new MeterController(component, "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			base.sm.monitor.Set(this.meter.gameObject, base.smi, false);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00192E38 File Offset: 0x00191038
		public override void StartSM()
		{
			this.morbRoverMaker = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			MorbRoverMaker.Instance instance = this.morbRoverMaker;
			instance.GermsAdded = (Action<long>)Delegate.Combine(instance.GermsAdded, new Action<long>(this.OnGermsAdded));
			MorbRoverMaker.Instance instance2 = this.morbRoverMaker;
			instance2.OnUncovered = (System.Action)Delegate.Combine(instance2.OnUncovered, new System.Action(this.OnUncovered));
			base.StartSM();
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000AF7BF File Offset: 0x000AD9BF
		private void OnGermsAdded(long amount)
		{
			this.lastTimeGermsConsumed = GameClock.Instance.GetTime();
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x000AF7D1 File Offset: 0x000AD9D1
		public bool ShouldBeOn()
		{
			return this.morbRoverMaker.HasBeenRevealed && this.operational.IsOperational;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x000AF7ED File Offset: 0x000AD9ED
		private void OnUncovered()
		{
			if (base.IsInsideState(base.sm.off.idle))
			{
				this.GoTo(base.sm.off.exiting);
			}
		}

		// Token: 0x04000E58 RID: 3672
		private float lastTimeGermsConsumed = -1f;

		// Token: 0x04000E59 RID: 3673
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04000E5A RID: 3674
		private MorbRoverMaker.Instance morbRoverMaker;

		// Token: 0x04000E5B RID: 3675
		private MeterController meter;
	}
}
