public class GeoTunerSwitchGeyserWorkable : Workable
{
	private const string animName = "anim_use_remote_kanim";

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_use_remote_kanim") };
		faceTargetWhenWorking = true;
		synchronizeAnims = false;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetWorkTime(3f);
	}
}
