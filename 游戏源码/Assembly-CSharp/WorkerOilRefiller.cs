using System;

// Token: 0x02001766 RID: 5990
public class WorkerOilRefiller : Workable
{
	// Token: 0x06007B46 RID: 31558 RVA: 0x0031AC74 File Offset: 0x00318E74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
		};
		this.workAnims = WorkerOilRefiller.WORK_ANIMS;
		this.workingPstComplete = WorkerOilRefiller.WORK_PST_ANIM;
		this.workingPstFailed = WorkerOilRefiller.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerOiling;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06007B47 RID: 31559 RVA: 0x0031AD08 File Offset: 0x00318F08
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		Storage component = worker.GetComponent<Storage>();
		if (component != null)
		{
			float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
			float num = Math.Min(60f - massAvailable, 1f * dt);
			this.progressBar.PercentFull = massAvailable / 60f;
			if (num > 0f)
			{
				this.storage.TransferMass(component, GameTags.LubricatingOil, num, false, false, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x04005C6C RID: 23660
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"oil_pre",
		"oil_loop"
	};

	// Token: 0x04005C6D RID: 23661
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"oil_pst"
	};

	// Token: 0x04005C6E RID: 23662
	[MyCmpGet]
	private Storage storage;
}
