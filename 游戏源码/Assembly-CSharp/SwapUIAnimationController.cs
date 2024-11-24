using System;
using UnityEngine;

// Token: 0x02002024 RID: 8228
public class SwapUIAnimationController : MonoBehaviour
{
	// Token: 0x0600AF24 RID: 44836 RVA: 0x0041EB6C File Offset: 0x0041CD6C
	public void SetState(bool Primary)
	{
		this.AnimationControllerObject_Primary.SetActive(Primary);
		if (!Primary)
		{
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = new Color(1f, 1f, 1f, 0.5f);
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
		this.AnimationControllerObject_Alternate.SetActive(!Primary);
		if (Primary)
		{
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.white;
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
	}

	// Token: 0x040089EC RID: 35308
	public GameObject AnimationControllerObject_Primary;

	// Token: 0x040089ED RID: 35309
	public GameObject AnimationControllerObject_Alternate;
}
