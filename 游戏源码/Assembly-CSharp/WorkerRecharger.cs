using System;

// Token: 0x02001764 RID: 5988
public class WorkerRecharger : Workable
{
	// Token: 0x06007B3E RID: 31550 RVA: 0x0031A9EC File Offset: 0x00318BEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = WorkerRecharger.WORK_ANIMS;
		this.workingPstComplete = WorkerRecharger.WORK_PST_ANIM;
		this.workingPstFailed = WorkerRecharger.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerRecharging;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06007B3F RID: 31551 RVA: 0x0031AA60 File Offset: 0x00318C60
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
		if (component != null)
		{
			this.progressBar.PercentFull = component.ChargeRatio;
			return component.ApplyDeltaEnergy(4f * dt) == 0f;
		}
		return true;
	}

	// Token: 0x04005C67 RID: 23655
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"recharge_pre",
		"recharge_loop"
	};

	// Token: 0x04005C68 RID: 23656
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"recharge_pst"
	};
}
