using System;
using UnityEngine;

// Token: 0x02001A2D RID: 6701
public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x06008BBD RID: 35773 RVA: 0x00360C6C File Offset: 0x0035EE6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.OverrideAnim);
		this.pre_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreFrontAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreBackAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.LoopAnim, KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (WindTunnelWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstBackAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstFrontAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x04006923 RID: 26915
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x04006924 RID: 26916
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x04006925 RID: 26917
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x04006926 RID: 26918
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x04006927 RID: 26919
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x04006928 RID: 26920
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x04006929 RID: 26921
	public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x02001A2E RID: 6702
	public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x06008BBF RID: 35775 RVA: 0x000FB5B3 File Offset: 0x000F97B3
		public StatesInstance(WorkerBase master, VerticalWindTunnelWorkable workable) : base(master)
		{
			this.workable = workable;
			base.sm.worker.Set(master, base.smi);
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06008BC0 RID: 35776 RVA: 0x000FB5DA File Offset: 0x000F97DA
		public HashedString OverrideAnim
		{
			get
			{
				return this.workable.overrideAnim;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06008BC1 RID: 35777 RVA: 0x000FB5E7 File Offset: 0x000F97E7
		public string PreFrontAnim
		{
			get
			{
				return this.workable.preAnims[0];
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06008BC2 RID: 35778 RVA: 0x000FB5F6 File Offset: 0x000F97F6
		public string PreBackAnim
		{
			get
			{
				return this.workable.preAnims[1];
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06008BC3 RID: 35779 RVA: 0x000FB605 File Offset: 0x000F9805
		public string LoopAnim
		{
			get
			{
				return this.workable.loopAnim;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06008BC4 RID: 35780 RVA: 0x000FB612 File Offset: 0x000F9812
		public string PstBackAnim
		{
			get
			{
				return this.workable.pstAnims[0];
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06008BC5 RID: 35781 RVA: 0x000FB621 File Offset: 0x000F9821
		public string PstFrontAnim
		{
			get
			{
				return this.workable.pstAnims[1];
			}
		}

		// Token: 0x0400692A RID: 26922
		private VerticalWindTunnelWorkable workable;
	}
}
