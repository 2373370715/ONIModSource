using System;

// Token: 0x020017D3 RID: 6099
public class StorageUnloadMonitor : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
	// Token: 0x06007D96 RID: 32150 RVA: 0x00327108 File Offset: 0x00325308
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notFull;
		this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload), UpdateRate.SIM_200ms);
		this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull, (StorageUnloadMonitor.Instance smi) => smi.gameObject).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StorageUnloadMonitor.Instance data) => true, null).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter(delegate(StorageUnloadMonitor.Instance smi)
		{
			if (smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() <= 0f)
			{
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_full");
			}
		});
	}

	// Token: 0x06007D97 RID: 32151 RVA: 0x003271E4 File Offset: 0x003253E4
	public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
	{
		Storage storage = smi.sm.sweepLocker.Get(smi);
		return !(storage == null) && !(smi.sm.internalStorage.Get(smi) == null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (storage != null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell(storage) == Grid.PosToCell(smi)));
	}

	// Token: 0x04005F20 RID: 24352
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x04005F21 RID: 24353
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;

	// Token: 0x04005F22 RID: 24354
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;

	// Token: 0x04005F23 RID: 24355
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

	// Token: 0x020017D4 RID: 6100
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017D5 RID: 6101
	public new class Instance : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance
	{
		// Token: 0x06007D9A RID: 32154 RVA: 0x000F2BDA File Offset: 0x000F0DDA
		public Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def) : base(master, def)
		{
		}
	}
}
