using System;

// Token: 0x02001698 RID: 5784
public class POITechItemUnlockWorkable : Workable
{
	// Token: 0x06007779 RID: 30585 RVA: 0x0030E1E8 File Offset: 0x0030C3E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_unlock_kanim")
		};
		this.synchronizeAnims = true;
	}

	// Token: 0x0600777A RID: 30586 RVA: 0x0030E244 File Offset: 0x0030C444
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		POITechItemUnlocks.Instance smi = this.GetSMI<POITechItemUnlocks.Instance>();
		smi.UnlockTechItems();
		smi.sm.pendingChore.Set(false, smi, false);
		base.gameObject.Trigger(1980521255, null);
		Prioritizable.RemoveRef(base.gameObject);
	}
}
