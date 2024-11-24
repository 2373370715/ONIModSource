using System;
using TUNING;
using UnityEngine;

// Token: 0x02000680 RID: 1664
public class DataRainerChore : Chore<DataRainerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001E44 RID: 7748 RVA: 0x001B356C File Offset: 0x001B176C
	public DataRainerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DataRainerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x000B3FC3 File Offset: 0x000B21C3
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04001350 RID: 4944
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x02000681 RID: 1665
	public class States : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore>
	{
		// Token: 0x06001E46 RID: 7750 RVA: 0x001B3608 File Offset: 0x001B1808
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.dataRainer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.goToStand, (DataRainerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((DataRainerChore.StatesInstance smi) => smi.GetTargetCell(), this.raining, this.idle, false);
			this.raining.ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.raining.loop).Update(delegate(DataRainerChore.StatesInstance smi, float dt)
			{
				this.nextBankTimer.Delta(dt, smi);
				if (this.nextBankTimer.Get(smi) >= DataRainer.databankSpawnInterval)
				{
					this.nextBankTimer.Delta(-DataRainer.databankSpawnInterval, smi);
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("PowerStationTools"), smi.master.transform.position + Vector3.up);
					gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Iron, true);
					gameObject.SetActive(true);
					KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
					float num = (float)component.currentFrame / (float)component.GetCurrentNumFrames();
					Vector2 initial_velocity = new Vector2((num < 0.5f) ? -2.5f : 2.5f, 4f);
					if (GameComps.Fallers.Has(gameObject))
					{
						GameComps.Fallers.Remove(gameObject);
					}
					GameComps.Fallers.Add(gameObject, initial_velocity);
					DataRainer.Instance smi2 = this.dataRainer.Get(smi).GetSMI<DataRainer.Instance>();
					DataRainer sm = smi2.sm;
					sm.databanksCreated.Set(sm.databanksCreated.Get(smi2) + 1, smi2, false);
				}
			}, UpdateRate.SIM_33ms, false);
			this.raining.loop.PlayAnim("makeitrain2", KAnim.PlayMode.Loop);
		}

		// Token: 0x04001351 RID: 4945
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.TargetParameter dataRainer;

		// Token: 0x04001352 RID: 4946
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter nextBankTimer = new StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter(DataRainer.databankSpawnInterval / 2f);

		// Token: 0x04001353 RID: 4947
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State idle;

		// Token: 0x04001354 RID: 4948
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State goToStand;

		// Token: 0x04001355 RID: 4949
		public DataRainerChore.States.RainingStates raining;

		// Token: 0x02000682 RID: 1666
		public class RainingStates : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State
		{
			// Token: 0x04001356 RID: 4950
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pre;

			// Token: 0x04001357 RID: 4951
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State loop;

			// Token: 0x04001358 RID: 4952
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pst;
		}
	}

	// Token: 0x02000684 RID: 1668
	public class StatesInstance : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.GameInstance
	{
		// Token: 0x06001E4E RID: 7758 RVA: 0x000B4013 File Offset: 0x000B2213
		public StatesInstance(DataRainerChore master, GameObject dataRainer) : base(master)
		{
			this.dataRainer = dataRainer;
			base.sm.dataRainer.Set(dataRainer, base.smi, false);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000B403C File Offset: 0x000B223C
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x001B3810 File Offset: 0x001B1A10
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x0400135C RID: 4956
		private GameObject dataRainer;
	}
}
