public class ToggleGeothermalVentConnection : Toggleable
{
	[MyCmpGet]
	private KBatchedAnimController buildingAnimController;

	private Facing workerFacing;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SetWorkTime(10f);
		overrideAnims = new KAnimFile[1] { Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE) };
		workAnims = new HashedString[1] { GeothermalVentConfig.TOGGLE_ANIMATION };
		workingPstComplete = null;
		workingPstFailed = null;
		workLayer = Grid.SceneLayer.Front;
		synchronizeAnims = false;
		workAnimPlayMode = KAnim.PlayMode.Once;
		SetOffsets(new CellOffset[1] { CellOffset.none });
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		buildingAnimController.Play(GeothermalVentConfig.TOGGLE_ANIMATION);
		if (workerFacing == null || workerFacing.gameObject != worker.gameObject)
		{
			workerFacing = worker.GetComponent<Facing>();
		}
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (workerFacing != null)
		{
			workerFacing.Face(workerFacing.transform.GetLocalPosition().x + 0.5f);
		}
		return base.OnWorkTick(worker, dt);
	}
}
