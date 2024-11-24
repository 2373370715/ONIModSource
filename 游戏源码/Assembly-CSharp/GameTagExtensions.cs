using System;
using UnityEngine;

// Token: 0x02001336 RID: 4918
public static class GameTagExtensions
{
	// Token: 0x060064F3 RID: 25843 RVA: 0x000E1E21 File Offset: 0x000E0021
	public static GameObject Prefab(this Tag tag)
	{
		return Assets.GetPrefab(tag);
	}

	// Token: 0x060064F4 RID: 25844 RVA: 0x000E1E29 File Offset: 0x000E0029
	public static string ProperName(this Tag tag)
	{
		return TagManager.GetProperName(tag, false);
	}

	// Token: 0x060064F5 RID: 25845 RVA: 0x000E1E32 File Offset: 0x000E0032
	public static string ProperNameStripLink(this Tag tag)
	{
		return TagManager.GetProperName(tag, true);
	}

	// Token: 0x060064F6 RID: 25846 RVA: 0x000E1E3B File Offset: 0x000E003B
	public static Tag Create(SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}

	// Token: 0x060064F7 RID: 25847 RVA: 0x000E1E3B File Offset: 0x000E003B
	public static Tag CreateTag(this SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}
}
