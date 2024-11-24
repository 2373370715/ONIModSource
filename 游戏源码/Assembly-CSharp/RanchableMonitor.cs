using System;

// Token: 0x02000A23 RID: 2595
public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
	// Token: 0x06002F6C RID: 12140 RVA: 0x000BECB8 File Offset: 0x000BCEB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (RanchableMonitor.Instance smi) => smi.ShouldGoGetRanched(), null);
	}

	// Token: 0x02000A24 RID: 2596
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A25 RID: 2597
	public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x000BECFB File Offset: 0x000BCEFB
		// (set) Token: 0x06002F70 RID: 12144 RVA: 0x000BED03 File Offset: 0x000BCF03
		public ChoreConsumer ChoreConsumer { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x000BED0C File Offset: 0x000BCF0C
		public Navigator NavComponent
		{
			get
			{
				return this.navComponent;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000BED14 File Offset: 0x000BCF14
		public RanchedStates.Instance States
		{
			get
			{
				if (this.states == null)
				{
					this.states = this.controller.GetSMI<RanchedStates.Instance>();
				}
				return this.states;
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000BED35 File Offset: 0x000BCF35
		public Instance(IStateMachineTarget master, RanchableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.navComponent = base.GetComponent<Navigator>();
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000BED57 File Offset: 0x000BCF57
		public bool ShouldGoGetRanched()
		{
			return this.TargetRanchStation != null && this.TargetRanchStation.IsRunning() && this.TargetRanchStation.IsRancherReady;
		}

		// Token: 0x0400200E RID: 8206
		public RanchStation.Instance TargetRanchStation;

		// Token: 0x0400200F RID: 8207
		private Navigator navComponent;

		// Token: 0x04002010 RID: 8208
		private RanchedStates.Instance states;
	}
}
