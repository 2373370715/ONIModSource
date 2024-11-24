using System;

// Token: 0x02000DB7 RID: 3511
public class ToggleGeothermalVentConnection : Toggleable
{
	// Token: 0x06004504 RID: 17668 RVA: 0x0024A310 File Offset: 0x00248510
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE)
		};
		this.workAnims = new HashedString[]
		{
			GeothermalVentConfig.TOGGLE_ANIMATION
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.workLayer = Grid.SceneLayer.Front;
		this.synchronizeAnims = false;
		this.workAnimPlayMode = KAnim.PlayMode.Once;
		base.SetOffsets(new CellOffset[]
		{
			CellOffset.none
		});
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x0024A3A0 File Offset: 0x002485A0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.buildingAnimController.Play(GeothermalVentConfig.TOGGLE_ANIMATION, KAnim.PlayMode.Once, 1f, 0f);
		if (this.workerFacing == null || this.workerFacing.gameObject != worker.gameObject)
		{
			this.workerFacing = worker.GetComponent<Facing>();
		}
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x000CC978 File Offset: 0x000CAB78
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.workerFacing != null)
		{
			this.workerFacing.Face(this.workerFacing.transform.GetLocalPosition().x + 0.5f);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x04002F86 RID: 12166
	[MyCmpGet]
	private KBatchedAnimController buildingAnimController;

	// Token: 0x04002F87 RID: 12167
	private Facing workerFacing;
}
