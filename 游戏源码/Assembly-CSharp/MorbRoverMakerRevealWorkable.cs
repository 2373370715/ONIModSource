using System;

// Token: 0x020004BC RID: 1212
public class MorbRoverMakerRevealWorkable : Workable
{
	// Token: 0x06001560 RID: 5472 RVA: 0x00193440 File Offset: 0x00191640
	protected override void OnPrefabInit()
	{
		this.workAnims = new HashedString[]
		{
			"reveal_working_pre",
			"reveal_working_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"reveal_working_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"reveal_working_pst"
		};
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerBuildingRevealed;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerWorkingOnRevealing);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000AB715 File Offset: 0x000A9915
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000E6D RID: 3693
	public const string WORKABLE_PRE_ANIM_NAME = "reveal_working_pre";

	// Token: 0x04000E6E RID: 3694
	public const string WORKABLE_LOOP_ANIM_NAME = "reveal_working_loop";

	// Token: 0x04000E6F RID: 3695
	public const string WORKABLE_PST_ANIM_NAME = "reveal_working_pst";
}
