using System;

// Token: 0x02001761 RID: 5985
public class NewWorker : Workable
{
	// Token: 0x06007B32 RID: 31538 RVA: 0x0031A940 File Offset: 0x00318B40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
		this.workAnims = NewWorker.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.workTime = 0.8f;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06007B33 RID: 31539 RVA: 0x000F0FEC File Offset: 0x000EF1EC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<RemoteWorkerSM>().Docked = true;
	}

	// Token: 0x04005C64 RID: 23652
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"new_worker"
	};
}
