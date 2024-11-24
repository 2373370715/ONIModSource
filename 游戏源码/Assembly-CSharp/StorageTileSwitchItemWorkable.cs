using System;

// Token: 0x02000FC8 RID: 4040
public class StorageTileSwitchItemWorkable : Workable
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x060051E1 RID: 20961 RVA: 0x000D5637 File Offset: 0x000D3837
	// (set) Token: 0x060051E0 RID: 20960 RVA: 0x000D562E File Offset: 0x000D382E
	public int LastCellWorkerUsed { get; private set; } = -1;

	// Token: 0x060051E2 RID: 20962 RVA: 0x000D563F File Offset: 0x000D383F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x060051E3 RID: 20963 RVA: 0x000CC5C4 File Offset: 0x000CA7C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x060051E4 RID: 20964 RVA: 0x000D567E File Offset: 0x000D387E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (worker != null)
		{
			this.LastCellWorkerUsed = Grid.PosToCell(worker.transform.GetPosition());
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x0400394C RID: 14668
	private const string animName = "anim_use_remote_kanim";
}
