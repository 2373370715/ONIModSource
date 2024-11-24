using System;

public class MorbRoverMakerDisplay : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
	public class Def : BaseDef
	{
		public float Timeout = 1f;
	}

	public class OffStates : State
	{
		public State entering;

		public State idle;

		public State exiting;
	}

	public class OnStates : State
	{
		public State idle;

		public State shake;

		public State noGerm;

		public State germ;

		public State checkmark;
	}

	public new class Instance : GameInstance
	{
		private float lastTimeGermsConsumed = -1f;

		[MyCmpReq]
		private Operational operational;

		private MorbRoverMaker.Instance morbRoverMaker;

		private MeterController meter;

		public bool HasRecentlyConsumedGerms => GameClock.Instance.GetTime() - lastTimeGermsConsumed < base.def.Timeout;

		public bool GermsAreNeeded => morbRoverMaker.MorbDevelopment_Progress < 1f;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			meter = new MeterController(component, "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront);
			base.sm.monitor.Set(meter.gameObject, base.smi);
		}

		public override void StartSM()
		{
			morbRoverMaker = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			MorbRoverMaker.Instance instance = morbRoverMaker;
			instance.GermsAdded = (Action<long>)Delegate.Combine(instance.GermsAdded, new Action<long>(OnGermsAdded));
			MorbRoverMaker.Instance instance2 = morbRoverMaker;
			instance2.OnUncovered = (System.Action)Delegate.Combine(instance2.OnUncovered, new System.Action(OnUncovered));
			base.StartSM();
		}

		private void OnGermsAdded(long amount)
		{
			lastTimeGermsConsumed = GameClock.Instance.GetTime();
		}

		public bool ShouldBeOn()
		{
			if (morbRoverMaker.HasBeenRevealed)
			{
				return operational.IsOperational;
			}
			return false;
		}

		private void OnUncovered()
		{
			if (IsInsideState(base.sm.off.idle))
			{
				GoTo(base.sm.off.exiting);
			}
		}
	}

	public const string METER_TARGET_NAME = "meter_display_target";

	public const string OFF_IDLE_ANIM_NAME = "display_off_idle";

	public const string OFF_ENTERING_ANIM_NAME = "display_off";

	public const string OFF_EXITING_ANIM_NAME = "display_on";

	public const string GERM_ICON_ANIM_NAME = "display_germ";

	public const string NO_GERM_ANIM_NAME = "display_no_germ";

	public const string ON_IDLE_ANIM_NAME = "display_idle";

	public TargetParameter monitor;

	public OffStates off;

	public OnStates on;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.Never;
		default_state = off.idle;
		root.Target(monitor);
		off.DefaultState(off.idle);
		off.entering.PlayAnim("display_off").OnAnimQueueComplete(off.idle);
		off.idle.Target(masterTarget).EventTransition(GameHashes.TagsChanged, off.exiting, ShouldBeOn).Target(monitor)
			.PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
		off.exiting.PlayAnim("display_on").OnAnimQueueComplete(on);
		on.Target(masterTarget).TagTransition(GameTags.Operational, off.entering, on_remove: true).Target(monitor)
			.DefaultState(on.idle);
		on.idle.Transition(on.germ, HasGermsAddedAndGermsAreNeeded).Transition(on.noGerm, NoGermsAddedAndGermsAreNeeded).PlayAnim("display_idle", KAnim.PlayMode.Loop);
		on.noGerm.Transition(on.idle, GermsNoLongerNeeded).Transition(on.germ, HasGermsAddedAndGermsAreNeeded).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
		on.germ.Transition(on.idle, GermsNoLongerNeeded).Transition(on.noGerm, NoGermsAddedAndGermsAreNeeded).PlayAnim("display_germ", KAnim.PlayMode.Loop);
	}

	public static bool NoGermsAddedAndGermsAreNeeded(Instance smi)
	{
		if (smi.GermsAreNeeded)
		{
			return !smi.HasRecentlyConsumedGerms;
		}
		return false;
	}

	public static bool HasGermsAddedAndGermsAreNeeded(Instance smi)
	{
		if (smi.GermsAreNeeded)
		{
			return smi.HasRecentlyConsumedGerms;
		}
		return false;
	}

	public static bool ShouldBeOn(Instance smi)
	{
		return smi.ShouldBeOn();
	}

	public static bool GermsNoLongerNeeded(Instance smi)
	{
		return !smi.GermsAreNeeded;
	}
}
