using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public class ReturnToChargeStationStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>
{
	// Token: 0x060017E5 RID: 6117 RVA: 0x0019C21C File Offset: 0x0019A41C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.emote;
		this.emote.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).PlayAnim("react_lobatt", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToChargingStation);
		this.idle.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).ScheduleGoTo(1f, this.movingToChargingStation);
		this.movingToChargingStation.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).MoveTo(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (!(sweepLocker == null))
			{
				return Grid.PosToCell(sweepLocker);
			}
			return Grid.InvalidCell;
		}, this.chargingstates.waitingForCharging, this.idle, false);
		this.chargingstates.Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker != null)
			{
				smi.master.GetComponent<Facing>().Face(sweepLocker.gameObject.transform.position + Vector3.right);
				Vector3 position = smi.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
				smi.transform.SetPosition(position);
				KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				component.enabled = true;
			}
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			smi.transform.SetPosition(position);
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}).Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, true);
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, false);
		});
		this.chargingstates.waitingForCharging.PlayAnim("react_base", KAnim.PlayMode.Loop).TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.charging, (ReturnToChargeStationStates.Instance smi) => smi.StationReadyToCharge(), UpdateRate.SIM_200ms);
		this.chargingstates.charging.TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.interupted, (ReturnToChargeStationStates.Instance smi) => !smi.StationReadyToCharge(), UpdateRate.SIM_200ms).ToggleEffect("Charging").PlayAnim("sleep_pre").QueueAnim("sleep_idle", true, null).Enter(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StartCharging)).Exit(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StopCharging));
		this.chargingstates.interupted.PlayAnim("sleep_pst").TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).OnAnimQueueComplete(this.chargingstates.waitingForCharging);
		this.chargingstates.completed.PlayAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.RechargeBehaviour, false);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x0019C514 File Offset: 0x0019A714
	public Storage GetSweepLocker(ReturnToChargeStationStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x0019C548 File Offset: 0x0019A748
	public void Station_StartCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StartCharging();
		}
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x0019C574 File Offset: 0x0019A774
	public void Station_StopCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StopCharging();
		}
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x0019C5A0 File Offset: 0x0019A7A0
	public void Station_DockRobot(ReturnToChargeStationStates.Instance smi, bool dockState)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().DockRobot(dockState);
		}
	}

	// Token: 0x04000F71 RID: 3953
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State emote;

	// Token: 0x04000F72 RID: 3954
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State idle;

	// Token: 0x04000F73 RID: 3955
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State movingToChargingStation;

	// Token: 0x04000F74 RID: 3956
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State behaviourcomplete;

	// Token: 0x04000F75 RID: 3957
	public ReturnToChargeStationStates.ChargingStates chargingstates;

	// Token: 0x02000549 RID: 1353
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200054A RID: 1354
	public new class Instance : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.GameInstance
	{
		// Token: 0x060017F0 RID: 6128 RVA: 0x000B0150 File Offset: 0x000AE350
		public Instance(Chore<ReturnToChargeStationStates.Instance> chore, ReturnToChargeStationStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.RechargeBehaviour);
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0019C67C File Offset: 0x0019A87C
		public bool ChargeAborted()
		{
			return base.smi.sm.GetSweepLocker(base.smi) == null || !base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0019C6CC File Offset: 0x0019A8CC
		public bool StationReadyToCharge()
		{
			return base.smi.sm.GetSweepLocker(base.smi) != null && base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}
	}

	// Token: 0x0200054B RID: 1355
	public class ChargingStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State
	{
		// Token: 0x04000F76 RID: 3958
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State waitingForCharging;

		// Token: 0x04000F77 RID: 3959
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State charging;

		// Token: 0x04000F78 RID: 3960
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State interupted;

		// Token: 0x04000F79 RID: 3961
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State completed;
	}
}
