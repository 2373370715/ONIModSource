public class MorbRoverMakerRevealWorkable : Workable
{
	public const string WORKABLE_PRE_ANIM_NAME = "reveal_working_pre";

	public const string WORKABLE_LOOP_ANIM_NAME = "reveal_working_loop";

	public const string WORKABLE_PST_ANIM_NAME = "reveal_working_pst";

	protected override void OnPrefabInit()
	{
		workAnims = new HashedString[2] { "reveal_working_pre", "reveal_working_loop" };
		workingPstComplete = new HashedString[1] { "reveal_working_pst" };
		workingPstFailed = new HashedString[1] { "reveal_working_pst" };
		base.OnPrefabInit();
		workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerBuildingRevealed;
		SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerWorkingOnRevealing);
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim") };
		lightEfficiencyBonus = true;
		synchronizeAnims = true;
		SetWorkTime(15f);
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
	}
}
