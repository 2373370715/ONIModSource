public class POITechItemUnlockWorkable : Workable
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
		alwaysShowProgressBar = true;
		resetProgressOnStop = false;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_research_unlock_kanim") };
		synchronizeAnims = true;
	}

	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		POITechItemUnlocks.Instance sMI = this.GetSMI<POITechItemUnlocks.Instance>();
		sMI.UnlockTechItems();
		sMI.sm.pendingChore.Set(value: false, sMI);
		base.gameObject.Trigger(1980521255);
		Prioritizable.RemoveRef(base.gameObject);
	}
}
