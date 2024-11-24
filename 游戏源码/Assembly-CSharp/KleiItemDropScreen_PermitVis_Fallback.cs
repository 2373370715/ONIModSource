using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D4E RID: 7502
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	// Token: 0x06009CB2 RID: 40114 RVA: 0x00105F63 File Offset: 0x00104163
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.sprite.sprite = info.Sprite;
	}

	// Token: 0x04007ADD RID: 31453
	[SerializeField]
	private Image sprite;
}
