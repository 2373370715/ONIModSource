using System;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class DeliverToSweepLockerStates : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>
{
	// Token: 0x060017DC RID: 6108 RVA: 0x0019BFFC File Offset: 0x0019A1FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.movingToStorage;
		this.idle.ScheduleGoTo(1f, this.movingToStorage);
		this.movingToStorage.MoveTo(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			if (!(this.GetSweepLocker(smi) == null))
			{
				return Grid.PosToCell(this.GetSweepLocker(smi));
			}
			return Grid.InvalidCell;
		}, this.unloading, this.idle, false);
		this.unloading.Enter(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
			float num = Mathf.Max(0f, Mathf.Min(storage.ExactMassStored(), sweepLocker.RemainingCapacity()));
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					float num2 = Mathf.Min(gameObject.GetComponent<PrimaryElement>().Mass, num);
					if (num2 != 0f)
					{
						storage.Transfer(sweepLocker, gameObject.GetComponent<KPrefabID>().PrefabTag, num2, false, false);
					}
					num -= num2;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			smi.master.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
			sweepLocker.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			if (storage.MassStored() > 0f)
			{
				smi.ScheduleGoTo(2f, this.lockerFull);
				return;
			}
			smi.ScheduleGoTo(2f, this.behaviourcomplete);
		});
		this.lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToStorage);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour, false);
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x0019C094 File Offset: 0x0019A294
	public Storage GetSweepLocker(DeliverToSweepLockerStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000F6C RID: 3948
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State idle;

	// Token: 0x04000F6D RID: 3949
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State movingToStorage;

	// Token: 0x04000F6E RID: 3950
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State unloading;

	// Token: 0x04000F6F RID: 3951
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State lockerFull;

	// Token: 0x04000F70 RID: 3952
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State behaviourcomplete;

	// Token: 0x02000546 RID: 1350
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000547 RID: 1351
	public new class Instance : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.GameInstance
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x000B00B4 File Offset: 0x000AE2B4
		public Instance(Chore<DeliverToSweepLockerStates.Instance> chore, DeliverToSweepLockerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.UnloadBehaviour);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000B00D8 File Offset: 0x000AE2D8
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.UnloadingStorage, base.gameObject);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000B0110 File Offset: 0x000AE310
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage, false);
		}
	}
}
