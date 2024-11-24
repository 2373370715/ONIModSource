using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B2D RID: 15149
	public class CustomSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600E92B RID: 59691 RVA: 0x0013BD9B File Offset: 0x00139F9B
		public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
		{
			this.kanim = effect_kanim;
			this.animName = effect_anim_name;
		}

		// Token: 0x0600E92C RID: 59692 RVA: 0x004C5698 File Offset: 0x004C3898
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(this.animName, KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x0600E92D RID: 59693 RVA: 0x0013BD89 File Offset: 0x00139F89
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}

		// Token: 0x0400E4BF RID: 58559
		private string kanim;

		// Token: 0x0400E4C0 RID: 58560
		private string animName;
	}
}
