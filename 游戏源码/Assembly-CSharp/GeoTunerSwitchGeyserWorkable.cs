using System;

// Token: 0x02000DA5 RID: 3493
public class GeoTunerSwitchGeyserWorkable : Workable
{
	// Token: 0x0600448B RID: 17547 RVA: 0x000CC590 File Offset: 0x000CA790
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x0600448C RID: 17548 RVA: 0x000CC5C4 File Offset: 0x000CA7C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x04002F23 RID: 12067
	private const string animName = "anim_use_remote_kanim";
}
