using System;
using Steamworks;
using UnityEngine;

// Token: 0x02001CD3 RID: 7379
public class FeedbackTextFix : MonoBehaviour
{
	// Token: 0x06009A13 RID: 39443 RVA: 0x0010441A File Offset: 0x0010261A
	private void Awake()
	{
		if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.locText.key = this.newKey;
	}

	// Token: 0x04007847 RID: 30791
	public string newKey;

	// Token: 0x04007848 RID: 30792
	public LocText locText;
}
