using System;

// Token: 0x02001763 RID: 5987
public class ExitableDock : Workable
{
	// Token: 0x06007B3A RID: 31546 RVA: 0x000F1052 File Offset: 0x000EF252
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = ExitableDock.WORK_ANIMS;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06007B3B RID: 31547 RVA: 0x000F1082 File Offset: 0x000EF282
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<RemoteWorkerSM>().Docked = false;
	}

	// Token: 0x04005C66 RID: 23654
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"exit_dock"
	};
}
