using System;

// Token: 0x02001762 RID: 5986
public class EnterableDock : Workable
{
	// Token: 0x06007B36 RID: 31542 RVA: 0x0031A99C File Offset: 0x00318B9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
		this.workAnims = EnterableDock.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06007B37 RID: 31543 RVA: 0x000F101F File Offset: 0x000EF21F
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetComponent<RemoteWorkerSM>().Docked = true;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04005C65 RID: 23653
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"enter_dock"
	};
}
