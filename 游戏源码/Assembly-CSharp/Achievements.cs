using System;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
[AddComponentMenu("KMonoBehaviour/scripts/Achievements")]
public class Achievements : KMonoBehaviour
{
	// Token: 0x06003AC6 RID: 15046 RVA: 0x000C5ED9 File Offset: 0x000C40D9
	public void Unlock(string id)
	{
		if (SteamAchievementService.Instance)
		{
			SteamAchievementService.Instance.Unlock(id);
		}
	}
}
