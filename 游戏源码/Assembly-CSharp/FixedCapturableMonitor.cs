using System;

// Token: 0x02000A08 RID: 2568
public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	// Token: 0x06002F0D RID: 12045 RVA: 0x001F7018 File Offset: 0x001F5218
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (FixedCapturableMonitor.Instance smi) => smi.ShouldGoGetCaptured(), null).Enter(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}

	// Token: 0x02000A09 RID: 2569
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A0A RID: 2570
	public new class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
	{
		// Token: 0x06002F10 RID: 12048 RVA: 0x001F70A8 File Offset: 0x001F52A8
		public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.Navigator = base.GetComponent<Navigator>();
			this.PrefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000BE8A8 File Offset: 0x000BCAA8
		public bool ShouldGoGetCaptured()
		{
			return this.targetCapturePoint != null && this.targetCapturePoint.IsRunning() && this.targetCapturePoint.shouldCreatureGoGetCaptured && (!this.isBaby || this.targetCapturePoint.def.allowBabies);
		}

		// Token: 0x04001FB3 RID: 8115
		public FixedCapturePoint.Instance targetCapturePoint;

		// Token: 0x04001FB4 RID: 8116
		public ChoreConsumer ChoreConsumer;

		// Token: 0x04001FB5 RID: 8117
		public Navigator Navigator;

		// Token: 0x04001FB6 RID: 8118
		public Tag PrefabTag;

		// Token: 0x04001FB7 RID: 8119
		public bool isBaby;
	}
}
