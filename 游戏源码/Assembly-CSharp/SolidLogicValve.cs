using System;
using KSerialization;

// Token: 0x02000F99 RID: 3993
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>
{
	// Token: 0x060050C5 RID: 20677 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x000D4B11 File Offset: 0x000D2D11
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x000D4B24 File Offset: 0x000D2D24
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04003852 RID: 14418
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003853 RID: 14419
	[MyCmpReq]
	private SolidConduitBridge bridge;

	// Token: 0x02000F9A RID: 3994
	public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
	{
		// Token: 0x060050C9 RID: 20681 RVA: 0x0026F76C File Offset: 0x0026D96C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidLogicValve.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidLogicValve.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
			this.on.idle.PlayAnim("on").Transition(this.on.working, (SolidLogicValve.StatesInstance smi) => smi.IsDispensing(), UpdateRate.SIM_200ms);
			this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).Transition(this.on.idle, (SolidLogicValve.StatesInstance smi) => !smi.IsDispensing(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04003854 RID: 14420
		public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;

		// Token: 0x04003855 RID: 14421
		public SolidLogicValve.States.ReadyStates on;

		// Token: 0x02000F9B RID: 3995
		public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
		{
			// Token: 0x04003856 RID: 14422
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;

			// Token: 0x04003857 RID: 14423
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
		}
	}

	// Token: 0x02000F9D RID: 3997
	public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
	{
		// Token: 0x060050D4 RID: 20692 RVA: 0x000D4B63 File Offset: 0x000D2D63
		public StatesInstance(SolidLogicValve master) : base(master)
		{
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x000D4B6C File Offset: 0x000D2D6C
		public bool IsDispensing()
		{
			return base.master.bridge.IsDispensing;
		}
	}
}
