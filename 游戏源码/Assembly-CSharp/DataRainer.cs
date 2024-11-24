using System;
using TUNING;

// Token: 0x020007B3 RID: 1971
public class DataRainer : GameStateMachine<DataRainer, DataRainer.Instance>
{
	// Token: 0x0600235C RID: 9052 RVA: 0x001C5240 File Offset: 0x001C3440
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.databanksCreated, this.overjoyed.exitEarly, (DataRainer.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.DATA_RAINER.NUM_MICROCHIPS).Exit(delegate(DataRainer.Instance smi)
		{
			this.databanksCreated.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(DataRainer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.raining);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.raining, (DataRainer.Instance smi) => smi.IsRecTime());
		this.overjoyed.raining.ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerRaining, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (DataRainer.Instance smi) => !smi.IsRecTime()).ToggleChore((DataRainer.Instance smi) => new DataRainerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(DataRainer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04001757 RID: 5975
	public StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.IntParameter databanksCreated;

	// Token: 0x04001758 RID: 5976
	public static float databankSpawnInterval = 1.8f;

	// Token: 0x04001759 RID: 5977
	public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x0400175A RID: 5978
	public DataRainer.OverjoyedStates overjoyed;

	// Token: 0x020007B4 RID: 1972
	public class OverjoyedStates : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400175B RID: 5979
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400175C RID: 5980
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State raining;

		// Token: 0x0400175D RID: 5981
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x020007B5 RID: 1973
	public new class Instance : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06002362 RID: 9058 RVA: 0x000B7041 File Offset: 0x000B5241
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000B704A File Offset: 0x000B524A
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x001C5408 File Offset: 0x001C3608
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
