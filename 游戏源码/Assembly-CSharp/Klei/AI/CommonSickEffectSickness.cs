using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B2C RID: 15148
	public class CommonSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600E928 RID: 59688 RVA: 0x004C5638 File Offset: 0x004C3838
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx_loop", KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x0600E929 RID: 59689 RVA: 0x0013BD89 File Offset: 0x00139F89
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}
	}
}
