using System;

// Token: 0x02001765 RID: 5989
public class WorkerGunkRemover : Workable
{
	// Token: 0x06007B42 RID: 31554 RVA: 0x0031AB08 File Offset: 0x00318D08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
		};
		this.workAnims = WorkerGunkRemover.WORK_ANIMS;
		this.workingPstComplete = WorkerGunkRemover.WORK_PST_ANIM;
		this.workingPstFailed = WorkerGunkRemover.WORK_PST_ANIM;
		this.synchronizeAnims = true;
		this.triggerWorkReactions = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerDraining;
		this.workTime = (this.workTimeRemaining = float.PositiveInfinity);
	}

	// Token: 0x06007B43 RID: 31555 RVA: 0x0031AB9C File Offset: 0x00318D9C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		Storage component = worker.GetComponent<Storage>();
		if (component != null)
		{
			float massAvailable = component.GetMassAvailable(SimHashes.LiquidGunk);
			float num = Math.Min(massAvailable, 1f * dt);
			this.progressBar.PercentFull = 1f - massAvailable / 600f;
			if (num > 0f)
			{
				component.TransferMass(this.storage, SimHashes.LiquidGunk.CreateTag(), num, false, false, true);
				return false;
			}
		}
		return true;
	}

	// Token: 0x04005C69 RID: 23657
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"drain_gunk_pre",
		"drain_gunk_loop"
	};

	// Token: 0x04005C6A RID: 23658
	private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
	{
		"drain_gunk_pst"
	};

	// Token: 0x04005C6B RID: 23659
	[MyCmpGet]
	private Storage storage;
}
