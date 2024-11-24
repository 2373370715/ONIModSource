using System;
using TUNING;

// Token: 0x020007BB RID: 1979
public class RoboDancer : GameStateMachine<RoboDancer, RoboDancer.Instance>
{
	// Token: 0x0600237B RID: 9083 RVA: 0x001C56EC File Offset: 0x001C38EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<float>(this.timeSpentDancing, this.overjoyed.exitEarly, (RoboDancer.Instance smi, float p) => p >= TRAITS.JOY_REACTIONS.ROBO_DANCER.DANCE_DURATION).Exit(delegate(RoboDancer.Instance smi)
		{
			this.timeSpentDancing.Set(0f, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(RoboDancer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.dancing);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.dancing, (RoboDancer.Instance smi) => smi.IsRecTime());
		this.overjoyed.dancing.ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerDancing, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (RoboDancer.Instance smi) => !smi.IsRecTime()).ToggleChore((RoboDancer.Instance smi) => new RoboDancerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(RoboDancer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04001771 RID: 6001
	public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.FloatParameter timeSpentDancing;

	// Token: 0x04001772 RID: 6002
	public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04001773 RID: 6003
	public RoboDancer.OverjoyedStates overjoyed;

	// Token: 0x020007BC RID: 1980
	public class OverjoyedStates : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001774 RID: 6004
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001775 RID: 6005
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State dancing;

		// Token: 0x04001776 RID: 6006
		public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x020007BD RID: 1981
	public new class Instance : GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06002380 RID: 9088 RVA: 0x000B71D1 File Offset: 0x000B53D1
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000B71DA File Offset: 0x000B53DA
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x001C58B4 File Offset: 0x001C3AB4
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
