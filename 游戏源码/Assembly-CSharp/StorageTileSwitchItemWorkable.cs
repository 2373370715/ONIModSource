public class StorageTileSwitchItemWorkable : Workable
{
	private const string animName = "anim_use_remote_kanim";

	public int LastCellWorkerUsed { get; private set; } = -1;


	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_use_remote_kanim") };
		SetOffsetTable(OffsetGroups.InvertedStandardTable);
		faceTargetWhenWorking = true;
		synchronizeAnims = false;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetWorkTime(3f);
	}

	protected override void OnCompleteWork(Worker worker)
	{
		if (worker != null)
		{
			LastCellWorkerUsed = Grid.PosToCell(worker.transform.GetPosition());
		}
		base.OnCompleteWork(worker);
	}
}
