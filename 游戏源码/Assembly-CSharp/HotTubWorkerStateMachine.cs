using System;
using UnityEngine;

// Token: 0x020013FF RID: 5119
public class HotTubWorkerStateMachine : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x06006932 RID: 26930 RVA: 0x002D976C File Offset: 0x002D796C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims("anim_interacts_hottub_kanim", 0f);
		this.pre_front.PlayAnim("working_pre_front").OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim("working_pre_back").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((HotTubWorkerStateMachine.StatesInstance smi) => HotTubWorkerStateMachine.workAnimLoopVariants[UnityEngine.Random.Range(0, HotTubWorkerStateMachine.workAnimLoopVariants.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.loop_reenter).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.loop_reenter.GoTo(this.loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim("working_pst_back").OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim("working_pst_front").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x04004F63 RID: 20323
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x04004F64 RID: 20324
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x04004F65 RID: 20325
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x04004F66 RID: 20326
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop_reenter;

	// Token: 0x04004F67 RID: 20327
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x04004F68 RID: 20328
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x04004F69 RID: 20329
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x04004F6A RID: 20330
	public StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x04004F6B RID: 20331
	public static string[] workAnimLoopVariants = new string[]
	{
		"working_loop1",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x02001400 RID: 5120
	public class StatesInstance : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x06006935 RID: 26933 RVA: 0x000E4F9B File Offset: 0x000E319B
		public StatesInstance(WorkerBase master) : base(master)
		{
			base.sm.worker.Set(master, base.smi);
		}
	}
}
