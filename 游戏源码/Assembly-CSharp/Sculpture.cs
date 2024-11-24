using System;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class Sculpture : Artable
{
	// Token: 0x060034E5 RID: 13541 RVA: 0x000C2769 File Offset: 0x000C0969
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (Sculpture.sculptureOverrides == null)
		{
			Sculpture.sculptureOverrides = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};
		}
		this.overrideAnims = Sculpture.sculptureOverrides;
		this.synchronizeAnims = false;
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x0020C6D4 File Offset: 0x0020A8D4
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		bool flag = base.CurrentStage == "Default";
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			global::Debug.LogError("Missing stage: " + stage_id);
		}
		if (!skip_effect && !flag)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("sculpture_fx_kanim", base.transform.GetPosition(), base.transform, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.destroyOnAnimComplete = true;
			kbatchedAnimController.transform.SetLocalPosition(Vector3.zero);
			kbatchedAnimController.Play("poof", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040023EF RID: 9199
	private static KAnimFile[] sculptureOverrides;
}
