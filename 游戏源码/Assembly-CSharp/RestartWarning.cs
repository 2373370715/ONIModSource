using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020017B6 RID: 6070
public class RestartWarning : MonoBehaviour
{
	// Token: 0x06007D07 RID: 32007 RVA: 0x000F2567 File Offset: 0x000F0767
	private void Update()
	{
		if (RestartWarning.ShouldWarn)
		{
			this.text.enabled = true;
			this.image.enabled = true;
		}
	}

	// Token: 0x04005E93 RID: 24211
	public static bool ShouldWarn;

	// Token: 0x04005E94 RID: 24212
	public LocText text;

	// Token: 0x04005E95 RID: 24213
	public Image image;
}
